using UnityEngine;
using Guid = System.Guid;

namespace SphericalCow.OldCode
{
	[System.Serializable]
	public class AbilityModifierInstance 
	{
		private string abilityName;
		private AbilityModifierType type;
		private string statName;
		private Guid statInstanceGuid;	// Using GUID for serialize a reference to another serialized object
		private int targetValue;
		private bool modifierApplied;
		[System.NonSerialized] private AbstractStatInstance statInst;
		[System.NonSerialized] private RpgCharacterData character;
		
		
		
		//
		// Constructor (not called during deserialization)
		//

		public AbilityModifierInstance(AbilityModifier abilityModifierRef, RpgCharacterData characterData, AbilityInstance abilityInstance)
		{
			this.abilityName = abilityInstance.AbilityName;
			this.type = abilityModifierRef.Type;
			this.statName = abilityModifierRef.StatToModify.StatName;
			this.statInst = characterData.FindAnyStatInstance(this.statName);
			this.statInstanceGuid = this.statInst.StatGuid;
			this.character = characterData;
			this.targetValue = abilityModifierRef.TargetValue;
			this.modifierApplied = false;
		}
		
		
		//
		// Methods
		//
		
		/// <summary>
		/// 	Will only procede if the statInst is null
		/// </summary>
		public void RefreshStatReference()
		{
			if(this.statInst == null)
			{
				this.statInst = this.character.FindAnyStatInstance(this.statName);
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
			if(ReferenceEquals(this.statInst, statToUnapply))
			{
				this.Unapply();
				this.statInst = null;
			}
		}
		
		/// <summary>
		/// 	Will revert application of an ability upon the given stat, if applicable, and only for the given modifier type
		/// </summary>
		public void UnlinkSpecificStat(AbstractStatInstance statToUnapply, AbilityModifierType modifierType)
		{
			if(Debug.isDebugBuild)
			{
				Debug.Assert(statToUnapply != null, "AbilityModifierInstance: UnapplySpecificStat(): Stat is null!");
			}
			if(ReferenceEquals(this.statInst, statToUnapply))
			{
				if(this.type == modifierType)
				{
					this.Unapply();
					this.statInst = null;
				}
			}
		}
		
		/// <summary>
		/// 	Apply this modifier to its referenced stat
		/// </summary>
		public void Apply()
		{
			if(!this.modifierApplied)
			{
				// Make sure that the stat to modify exists
				if(this.statInst == null)
				{
					// Search for the appropriate stat and try again
					this.RefreshStatReference();
					if(this.statInst == null)
					{
						// Skip; stat to apply is not available
						return;
					}
				}
				
				// Actually apply this modifier unto the stat
				this.modifierApplied = this.statInst.ApplyModifier(this);
			}
		}
		
		/// <summary>
		/// 	Unapply this modifier to its referenced stat
		/// </summary>
		public void Unapply()
		{
			if(this.modifierApplied)
			{
				// If the stat to unapply from exists
				if(this.statInst != null)
				{
					// Unapply
					this.modifierApplied = this.statInst.UnapplyModifier(this);
				}
				else
				{
					// Stat no longer exists. Mark as unapplied
					this.modifierApplied = false;
				}
			}
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

		public AbstractStatInstance StatInstance
		{
			get
			{
				return this.statInst;
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
		
		public bool IsModifierApplied
		{
			get
			{
				return this.modifierApplied;
			}
		}
	}
}
