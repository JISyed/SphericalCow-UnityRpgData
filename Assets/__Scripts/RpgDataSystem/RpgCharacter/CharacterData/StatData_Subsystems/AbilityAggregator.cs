using UnityEngine;
using Guid = System.Guid;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SphericalCow
{
	/// <summary>
	/// 	A system that tracks modifications applied from Abilities onto Stats
	/// </summary>
	public class AbilityAggregator 
	{
		/// <summary>
		/// 	A piece of data that indicates the total modfications applied by all AbilityModifierData onto the StatData
		/// </summary>
		public struct NetModifications
		{
			/// <summary>
			/// 	Are there any limits imposed by any of the AbilityModifiers?
			/// 	If there are no modifications, this should be NoLimit
			/// </summary>
			public ModifierLimitType netLimit;
			
			/// <summary>
			/// 	The total modifications applied by the AbilityModifiers
			/// 	If there is no limit, this adds or subtracts from the final SP value of a stat.
			/// 	If there is a limit, this value indcates what that limit is.
			/// 	If there are no modifications at all, the limit should be NoLimit and the value should be 0
			/// </summary>
			public int netValue;
		}
		
		
		private ModifierLimitType imposedLimit = ModifierLimitType.NoLimit;
		private int limitValue = 0;
		private SaveableGuid statId;
		private List<AbilityModifierData> appliedModifiers = new List<AbilityModifierData>();
		private int relativeModificationValue = 0;
		
		
		
		/// <summary>
		/// 	Constructor needs the Stat's ID being held in StatData
		/// </summary>
		public AbilityAggregator(Guid newStatId)
		{
			this.statId = new SaveableGuid(newStatId);
		}
		
		
		
		/// <summary>
		/// 	Calculate to total modifications applied by the AbilityModifiiers
		/// </summary>
		public void AggregateAppliedModifications()
		{
			// Reset certian values
			this.limitValue = 0;
			this.relativeModificationValue = 0;
			this.imposedLimit = ModifierLimitType.NoLimit;
			
			// Add up all of the modifications on this stat
			foreach(AbilityModifierData modifier in this.appliedModifiers)
			{
				// Check if a modifier is imposing a limit on the SP
				AbilityModifierType modType = modifier.AbilityModifierReference.Type;
				if(modType == AbilityModifierType.DecreaseTo ||
				   modType == AbilityModifierType.IncreaseTo)
				{
					// Only the latest applied limit will be appliciable on a stat
					this.limitValue = modifier.AbilityModifierReference.ModifierValue;
					
					if(modType == AbilityModifierType.IncreaseTo)
					{
						this.imposedLimit = ModifierLimitType.LowerLimit;
					}
					else if(modType == AbilityModifierType.DecreaseTo)
					{
						this.imposedLimit = ModifierLimitType.UpperLimit;
					}
				}
				// The modifier either increases by or decreases by
				else
				{
					if(modType == AbilityModifierType.IncreaseBy)
					{
						// Add to the relative modification of the SP
						this.relativeModificationValue += modifier.AbilityModifierReference.ModifierValue;
					}
					else if(modType == AbilityModifierType.DecreaseBy)
					{
						// Remove from the relative modification of the SP
						this.relativeModificationValue -= modifier.AbilityModifierReference.ModifierValue;
					}
				}
			}
		}
		
		
		
		/// <summary>
		/// 	Applies all the AbilityModifiers of the given Ability onto this aggregator
		/// </summary>
		public void ApplyAbility(Ability abilityToAdd)
		{
			// Look over every modifier in the new ability
			var abilityModifiers = abilityToAdd.StatModifiers;
			foreach(AbilityModifier modifier in abilityModifiers)
			{
				// Only add the modifier if it modifies this stat, not some other stat
				if(this.statId.GuidData.Equals(modifier.ModifiedStat.Id))
				{
					AbilityModifierData modData = new AbilityModifierData(modifier, abilityToAdd);
					this.appliedModifiers.Add(modData);
				}
			}
			
			// Calculate total modifications applied from all abilities on this stat
			this.AggregateAppliedModifications();
		}
		
		
		
		/// <summary>
		/// 	Removes all the AbilityModifiers of the given Ability onto this aggregator
		/// </summary>
		public void RemoveAbility(Ability abilityToRemove)
		{
			// Make a list to remove old modifiers without messing with the actual list for iteration
			List<AbilityModifierData> removalList = new List<AbilityModifierData>();
			
			// Look over every modifierData instance in the aggregator
			foreach(AbilityModifierData modifier in this.appliedModifiers)
			{
				// Only remove modifiers that originate from the ability to be removed
				if(modifier.AbilityId.Equals(abilityToRemove.Id))
				{
					removalList.Add(modifier);
				}
			}
			
			// Remove the old modifiers from the actual modifier list
			foreach(AbilityModifierData oldModifier in removalList)
			{
				this.appliedModifiers.Remove(oldModifier);
			}
			
			// Clear the removal list
			removalList.Clear();
			
			// Calculate total modifications applied from all abilities on this stat
			this.AggregateAppliedModifications();
		}
		
		
		
		
		/// <summary>
		/// 	The total modifications applied by all ability modifiers upon this stat
		/// </summary>
		public NetModifications AbilityModifiedSp
		{
			get
			{
				NetModifications netMods;
				
				// If there is no limit imposed by AbilityModifiers
				if(this.imposedLimit == ModifierLimitType.NoLimit)
				{
					netMods.netLimit = ModifierLimitType.NoLimit;
					netMods.netValue = this.relativeModificationValue;
				}
				// If there IS a limit imposed (doesn't matter which limit)
				else
				{
					netMods.netLimit = this.imposedLimit;
					netMods.netValue = this.limitValue;
				}
				
				return netMods;
			}
		}
		
		
		/// <summary>
		/// 	The ID of the Stat associated with this aggregator
		/// </summary>
		public Guid StatId
		{
			get
			{
				return this.statId.GuidData;
			}
		}
		
		
		
		/// <summary>
		/// 	The AbilityModifiers applied onto the Stat associated with this aggregator
		/// </summary>
		public ReadOnlyCollection<AbilityModifierData> AppliedModifiers
		{
			get
			{
				return this.appliedModifiers.AsReadOnly();
			}
		}
		
		
	}
}
