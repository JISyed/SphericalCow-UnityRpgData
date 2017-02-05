using UnityEngine;
using Guid = System.Guid;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SphericalCow
{
	/// <summary>
	/// 	A system that tracks modifications applied from Abilities onto Stats
	/// </summary>
	[System.Serializable]
	public class AbilityAggregator 
	{
		/// <summary>
		/// 	A piece of data that indicates the total modfications applied by all AbilityModifierData onto the StatData
		/// </summary>
		public struct NetModifications
		{
			/// <summary>
			/// 	Are there any limits imposed by any of the AbilityModifiers?
			/// </summary>
			public ModifierLimitType netLimit;
			
			/// <summary>
			/// 	The total modifications applied by the AbilityModifiers
			/// 	If there is no limit, this adds or subtracts from the final SP value of a stat.
			/// 	If there is a limit, this value indcates what that limit is.
			/// </summary>
			public int netValue;
		}
		
		
		[SerializeField] private ModifierLimitType imposedLimit = ModifierLimitType.NoLimit;
		[SerializeField] private int limitValue = 0;
		[SerializeField] private SaveableGuid statId;
		[SerializeField] private List<AbilityModifierData> appliedModifiers = new List<AbilityModifierData>();
		[SerializeField] private int relativeModificationValue = 0;
		
		
		
		
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
			// TODO: Implement by iterating appliedModifiers
		}
		
		
		
		/// <summary>
		/// 	Applies all the AbilityModifiers of the given Ability onto this aggregator
		/// </summary>
		public void ApplyAbility(Ability abilityToAdd)
		{
			// TODO: Implement by iterating the AbilityModifiers with the given Ability
		}
		
		
		
		/// <summary>
		/// 	Removes all the AbilityModifiers of the given Ability onto this aggregator
		/// </summary>
		public void RemoveAbility(Ability abilityToRemove)
		{
			// TODO: Implement by iterating the AbilityModifiers with the given Ability
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
		
	}
}
