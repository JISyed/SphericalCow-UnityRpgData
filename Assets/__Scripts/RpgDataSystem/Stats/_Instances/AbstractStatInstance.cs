using Guid = System.Guid;

namespace SphericalCow
{
	[System.Serializable]
	public abstract class AbstractStatInstance
	{
		//
		// Constants
		//
		
		private const int THRESHOLD_UNAPPLIED = -1;
		
		
		
		//
		// Data
		//
		
		private string statName;
		private Guid statGuid;
		private int localStatPoints;		// Total accumulated stat points for this particular stat (used in both Use and Point assignment)
		private int netAbilityOffset;		// The total amount of stat modification done by abilities (can be negative)
		private int abilityMaxThreshold;	// The modified stat value if an ability applied "IncreaseTo" modification
		private int abilityMinThreshold;	// The modified stat value if an ability applied "DecreaseTo" modification
		private bool abilityThreholdApplied;// Was the stat modified by an "IncreaseTo" or "DecreaseTo" modification
		[System.NonSerialized] private RpgCharacterData character;

		
		
		//
		// Constuctor
		//
		
		protected AbstractStatInstance(string newStatName, RpgCharacterData newCharacter)
		{
			this.statName = newStatName;
			this.statGuid = this.GenerateGuid();
			this.localStatPoints = 0;					// TODO: Determine good default value for local stat point pool				
			this.netAbilityOffset = 0;
			this.abilityMaxThreshold = THRESHOLD_UNAPPLIED;		// "-1" means threshold wasn't applied
			this.abilityMinThreshold = THRESHOLD_UNAPPLIED;		// "-1" means threshold wasn't applied
			this.abilityThreholdApplied = false;
			this.character = newCharacter;
		}
		
		
		
		//
		// Contracts
		//
		
		public abstract StatType GetStatType();
		
		protected abstract void SetupStatReference();
		
		
		
		
		//
		// Methods
		//
		
		
		private Guid GenerateGuid()
		{
			return Guid.NewGuid();
		}
		
		
		/// <summary>
		/// 	Only to be called by AbilityModifierInstance
		/// </summary>
		/// <returns>True if the modifier is applied to this stat, false otherwise</returns>
		public bool ApplyModifier(AbilityModifierInstance modifier)
		{
			if(modifier.IsModifierApplied)
			{
				return true;
			}
			
			bool statApplied = false;
			
			
			if(modifier.Type == AbilityModifierType.IncreaseBy)
			{
				this.netAbilityOffset += modifier.TargetValue;
				statApplied = true;
			}
			else if(modifier.Type == AbilityModifierType.DecreaseBy)
			{
				this.netAbilityOffset -= modifier.TargetValue;
				statApplied = true;
			}
			
			// For IncreaseTo and DecreaseTo, the first such modifier will apply. Subsequent modifiers will not apply
			else if(modifier.Type == AbilityModifierType.IncreaseTo)
			{
				// If the an IncreaseTo or DecreaseTo mod was already applied
				if(this.abilityThreholdApplied)
				{
					// Do not apply the modification
					statApplied = false;
				}
				else
				{
					// If the current stat value is less than the target value to increase to
					if(this.localStatPoints < modifier.TargetValue)
					{
						this.abilityMaxThreshold = modifier.TargetValue;
						statApplied = true;
						this.abilityThreholdApplied = true;
					}
					else
					{
						statApplied = false;
					}
				}
			}
			else if(modifier.Type == AbilityModifierType.DecreaseTo)
			{
				// If the an IncreaseTo or DecreaseTo mod was already applied
				if(this.abilityThreholdApplied)
				{
					// Do not apply the modification
					statApplied = false;
				}
				else
				{
					// If the current stat value is greater than the target value to decrease to
					if(this.localStatPoints > modifier.TargetValue)
					{
						this.abilityMinThreshold = modifier.TargetValue;
						statApplied = true;
						this.abilityThreholdApplied = true;
					}
					else
					{
						statApplied = false;
					}
				}
			}
			
			return statApplied;
		}
		
		/// <summary>
		/// 	Only to be called by AbilityModifierInstance
		/// </summary>
		/// /// <returns>True if the modifier is APPLIED (not unapplied) to this stat, false otherwise</returns>
		public bool UnapplyModifier(AbilityModifierInstance modifier)
		{
			if(!modifier.IsModifierApplied)
			{
				return false;
			}
			
			bool statUnapplied = false;
			
			
			if(modifier.Type == AbilityModifierType.IncreaseBy)
			{
				this.netAbilityOffset -= modifier.TargetValue;
				statUnapplied = true;
			}
			else if(modifier.Type == AbilityModifierType.DecreaseBy)
			{
				this.netAbilityOffset += modifier.TargetValue;
				statUnapplied = true;
			}
			
			// For IncreaseTo and DecreaseTo, the first such modifier will apply. Subsequent modifiers will not apply
			else if(modifier.Type == AbilityModifierType.IncreaseTo)
			{
				// If the an IncreaseTo or DecreaseTo mod was already unapplied
				if(!this.abilityThreholdApplied)
				{
					// Do not unapply the modification
					statUnapplied = false;
				}
				else
				{
					// If the Max Threshold was applied
					if(this.abilityMaxThreshold > THRESHOLD_UNAPPLIED)
					{
						this.abilityMaxThreshold = THRESHOLD_UNAPPLIED;
						statUnapplied = true;
						this.abilityThreholdApplied = false;
					}
					else
					{
						statUnapplied = false;
					}
				}
			}
			else if(modifier.Type == AbilityModifierType.DecreaseTo)
			{
				// If the an IncreaseTo or DecreaseTo mod was already unapplied
				if(!this.abilityThreholdApplied)
				{
					// Do not apply the modification
					statUnapplied = false;
				}
				else
				{
					// If the MIN Threshold was applied
					if(this.abilityMinThreshold > THRESHOLD_UNAPPLIED)
					{
						this.abilityMinThreshold = THRESHOLD_UNAPPLIED;
						statUnapplied = true;
						this.abilityThreholdApplied = false;
					}
					else
					{
						statUnapplied = false;
					}
				}
			}
			
			
			// Returning the oppposite so that AbilityModifierInstnace can know if it is applied onto this stat
			return !statUnapplied;
		}	
		
		
		
		
		
		//
		// Properties
		//
		
		public void SetLocalStatPointsManually(int newSpAmount)
		{
			this.LocalStatPointsWithoutAbilities = newSpAmount;
		}
		
		public string StatName
		{
			get
			{
				return this.statName;
			}
		}

		public Guid StatGuid
		{
			get
			{
				return this.statGuid;
			}
		}
		
		public int LocalStatPointsWithoutAbilities
		{
			get
			{
				return this.localStatPoints;
			}

			protected set
			{
				this.localStatPoints = value;
			}
		}
		
		public int NetAbilityStatPointOffset
		{
			get
			{
				return this.netAbilityOffset;
			}
		}
		
		/// <summary>
		///		This is the local stat point pool plus any modifications applied by abilities
		/// </summary>
		public int NetLocalStatPoints
		{
			get
			{
				// Get the SP plus any modifications done by IncreaseBy or DecreaseBy mods
				int subtotalSP = this.localStatPoints + this.netAbilityOffset;
				
				// Subtotal cannot be negative
				if(subtotalSP < 0)
				{
					subtotalSP = 0;
				}
				
				// Check if IncreaseTo or DecreaseTo mods apply to this stat
				if(this.abilityThreholdApplied)
				{
					// If IncreaseTo was applied
					if(this.abilityMaxThreshold > THRESHOLD_UNAPPLIED)
					{
						subtotalSP = this.abilityMaxThreshold;
					}
					// If DecreaseTo was applied
					else if(this.abilityMinThreshold > THRESHOLD_UNAPPLIED)
					{
						subtotalSP = this.abilityMaxThreshold;
					}
				}
				
				return subtotalSP;
			}
		}
		
		/// <summary>
		/// 	Get the maximum amount of stat points possible for this particular stat. If 0 or less, then there is no max.
		/// </summary>
		public abstract int AbsoluteMaxStatPoints
		{
			get;
		}

		public RpgCharacterData Character
		{
			get
			{
				return this.character;
			}
			
			protected set
			{
				this.character = value;
			}
		}
		
	}
}
	