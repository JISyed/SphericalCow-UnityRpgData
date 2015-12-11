using UnityEngine;
using Guid = System.Guid;

namespace SphericalCow
{
	[System.Serializable]
	public class AbilityModifierInstance 
	{
		private string abilityName;
		private AbilityModifierType type;
		private string statName;
		private Guid statInstanceGuid;	// Using GUID for serialize a reference to another serialized object
		private int targetValue;
		private int originalValue;
		private bool modifierApplied;
		[System.NonSerialized] private AbstractStatInstance statRef;
		[System.NonSerialized] private RpgCharacterData character;
		
		
		
		//
		// Constructor (not called during deserialization)
		//

		public AbilityModifierInstance(AbilityModifier abilityModifierRef, RpgCharacterData characterData, AbilityInstance abilityInstance)
		{
			this.abilityName = abilityInstance.AbilityName;
			this.type = abilityModifierRef.Type;
			this.statName = abilityModifierRef.StatToModify.StatName;
			this.statRef = characterData.FindAnyStatInstance(this.statName);
			this.statInstanceGuid = this.statRef.StatGuid;
			this.character = characterData;
			this.targetValue = abilityModifierRef.TargetValue;
			this.originalValue = this.statRef.LocalXpPoolWithoutAbilities;
			this.modifierApplied = false;
		}
		
		
		//
		// Methods
		//
		
		/// <summary>
		/// 	Will only procede if the statRef is null
		/// </summary>
		public void RefreshStatReference()
		{
			if(this.statRef == null)
			{
				this.statRef = this.character.FindAnyStatInstance(this.statName);
			}
		}
		
		/// <summary>
		/// 	Will revert application of an ability upon the given stat, if applicable
		/// </summary>
		public void UnapplySpecificStat(AbstractStatInstance statToUnapply)
		{
			if(Debug.isDebugBuild)
			{
				Debug.Assert(statToUnapply != null, "AbilityModifierInstance: UnapplySpecificStat(): Stat is null!");
			}
			if(ReferenceEquals(this.statRef, statToUnapply))
			{
				this.Unapply();
				this.statRef = null;
			}
		}
		
		/// <summary>
		/// 	Apply this modifier to its referenced stat
		/// </summary>
		public void Apply()
		{
			// TODO: Apply abilityModifier
			if(!this.modifierApplied)
			{
				// Make sure that the stat to modify exists
				if(this.statRef == null)
				{
					this.RefreshStatReference();
					if(this.statRef == null)
					{
						// Skip; stat to apply is not available
						return;
					}
				}
				
				// Actually apply this modifier unto the stat
				this.ModifyStat();
				
				// Mark this modifier as applied
				this.modifierApplied = true;
			}
		}
		
		/// <summary>
		/// 	Unapply this modifier to its referenced stat
		/// </summary>
		public void Unapply()
		{
			// TODO: Unapply ability modifier
			if(this.statRef != null)
			{
				
			}
		}
		
		
		/// <summary>
		/// 	Private routine for Apply()
		/// </summary>
		private void ModifyStat()
		{
			this.originalValue = this.statRef.LocalXpPoolWithoutAbilities;
			
			switch(this.type)
			{
				case AbilityModifierType.IncreaseBy:
				{
					// Increment the stat by the target value
					this.statRef.SetLocalXpPoolManually(this.originalValue + this.targetValue);
					break;
				}
				case AbilityModifierType.IncreaseTo:
				{
					// Set the stat to the target value, only if the target value is more than the original
					if(this.originalValue < this.targetValue)
					{
						this.statRef.SetLocalXpPoolManually(this.targetValue);
					}
					break;
				}
				case AbilityModifierType.DecreaseBy:
				{
					// Decrement the stat by the target value
					this.statRef.SetLocalXpPoolManually(this.originalValue - this.targetValue);
					break;
				}
				case AbilityModifierType.DecreaseTo:
				{
					// Set the stat to the target value, only if the target value is less than the original
					if(this.originalValue > this.targetValue)
					{
						this.statRef.SetLocalXpPoolManually(this.targetValue);
					}
					break;
				}
				default:
					Debug.Assert(false, "Unhandled condition for AbilityModifierType!");
					break;
			}
			
		}
		
		/// <summary>
		/// 	Private routines for Unapply()
		/// </summary>
		private void RevertStat()
		{
			this.statRef.SetLocalXpPoolManually(this.originalValue);
		}
		
		


		//
		// Setters
		//

		public string AbilityName
		{
			get
			{
				return this.abilityName;
			}
		}

		public AbilityModifierType Type
		{
			get
			{
				return this.type;
			}
		}

		public string StatName
		{
			get
			{
				return this.statName;
			}
		}

		public Guid StatInstanceGuid
		{
			get
			{
				return this.statInstanceGuid;
			}
		}

		public AbstractStatInstance StatReference
		{
			get
			{
				return this.statRef;
			}
		}

		public RpgCharacterData Character
		{
			get
			{
				return this.character;
			}
		}

		public int TargetValue
		{
			get
			{
				return this.targetValue;
			}
		}

		public int OriginalValue
		{
			get
			{
				return this.originalValue;
			}
		}
		
		public bool IsModifierApplied
		{
			get
			{
				return this.modifierApplied;
			}
		}
	}
}
