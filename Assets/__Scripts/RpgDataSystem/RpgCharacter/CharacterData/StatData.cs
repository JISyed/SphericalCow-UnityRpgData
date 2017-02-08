using UnityEngine;
using Guid = System.Guid;

namespace SphericalCow
{
	/// <summary>
	/// 	Data pertaining to any single Stat that includes Stat-Points (SP) for an RpgCharacter
	/// </summary>
	[System.Serializable]
	public class StatData 
	{
		[System.NonSerialized] private AbstractStat statReference;
		[SerializeField] private SaveableGuid statId;
		[SerializeField] private StatType type;
		[SerializeField] private int rawSpPool;
		[System.NonSerialized] private AbstractSpDeriver linkedStatsDerivedPool;
		[System.NonSerialized] private AbilityAggregator abilityModifications;
		[SerializeField] private int statUseCounter;
		
		
		
		
		public int RawStatPoints
		{
			get
			{
				return this.rawSpPool;
			}
		}
		
		
		public int StatPoints
		{
			get
			{
				// Start with the SP from the raw pool (the SP that is not modified by anything)
				int finalSp = this.rawSpPool;
				
				// Add additional SP from the SpDeriver
				finalSp += this.linkedStatsDerivedPool.DerivedSpPool;
				
				// Determing additional SP from Abilities (may alter the SP greatly)
				AbilityAggregator.NetModifications statMods = this.abilityModifications.AbilityModifiedSp;
				if(statMods.netLimit == ModifierLimitType.NoLimit)	// IncreaseBy or DecreaseBy only
				{
					finalSp += statMods.netValue;
				}
				else
				{
					// For Stats that are stuck with IncreaseTo or DecreaseTo
					if(statMods.netLimit == ModifierLimitType.UpperLimit)	// DecreaseTo
					{
						if(finalSp > statMods.netValue)
						{
							finalSp = statMods.netValue;
						}
					}
					else if(statMods.netLimit == ModifierLimitType.LowerLimit)	// IncreaseTo
					{
						if(finalSp < statMods.netValue)
						{
							finalSp = statMods.netValue;
						}
					}
					else
					{
						Debug.LogError("Somebody added a new enum entry into ModifierLimitType!");
					}
				}
				
				
				return finalSp;
			}
		}
		
		
	}
}
