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
		[SerializeField] private int useFactor;
		[System.NonSerialized] private AbstractSpDeriver linkedStatsDerivedPool;
		[System.NonSerialized] private AbilityAggregator abilityModifications;
		
		
		
		
		
		public StatData(AbstractStat newStat)
		{
			this.statReference = newStat;
			this.statId = new SaveableGuid(newStat.Id);
			this.type = newStat.GetStatType();
			this.rawSpPool = 0;
			this.useFactor = 1;
			
			// TODO: Initialize the SPDeriver
			switch (this.type) 
			{
			case StatType.Base:
				this.linkedStatsDerivedPool = new BaseSpDeriver(newStat as BaseStat);
				break;
			case StatType.Secondary:
				this.linkedStatsDerivedPool = new SecondarySpDeriver(newStat as SecondaryStat);
				break;
			case StatType.Skill:
				this.linkedStatsDerivedPool = new SkillSpDeriver(newStat as SkillStat);
				break;
			default:
				Debug.LogError("Somebody added a new entry into the enum StatType");
				break;
			}
			
			// TODO: Initialize the AbilityAggregator
			
			
		}
		
		
		
		
		/// <summary>
		/// 	Increases the SP of this stat's individual (raw) pool. This action cannot be undone!
		/// </summary>
		/// <param name="spToAdd">SP to add to this stat. Will be made positve if negative</param>
		public void AddStatPointsToRawPool(int spToAdd)
		{
			if(spToAdd < 0)
			{
				spToAdd = -spToAdd;
			}
			
			this.rawSpPool += spToAdd;
			
			// TODO: Invoke an SP recalculation on linked stats and abilties
		}
		
		
		
		/// <summary>
		/// 	Tells the system that this stat was used. This is needed for UseAssigned SP calculations.
		/// 	Call this whenever this stat was used in your game logic. Call as many times as this stat is used.
		/// </summary>
		public void MarkStatAsUsed()
		{
			this.useFactor++;
		}
		
		
		
		
		
		/// <summary>
		/// 	The total SP (stat points) of this stat. This value will be influenced by other stats and abilties
		/// </summary>
		public int StatPoints
		{
			get
			{
				// Start with the SP from the raw pool (the SP that is not modified by anything)
				int finalSp = this.rawSpPool;
				
				// Add additional SP from the SpDeriver
				finalSp += this.linkedStatsDerivedPool.DerivedSpPool;
				
				// Determing additional SP from Abilities (may alter the SP greatly)
				/*
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
				//*/
				
				return finalSp;
			}
		}
		
		
		/// <summary>
		/// 	The unaltered SP of this stat, un-influenced from other stats or abilities
		/// </summary>
		public int RawStatPoints
		{
			get
			{
				return this.rawSpPool;
			}
		}
		
		
		
		/// <summary>
		/// 	A numerical factor for this stat that increases the more the stat is used
		/// </summary>
		public int UseFactor
		{
			get
			{
				return this.useFactor;
			}
		}
		
		
		
		/// <summary>
		/// 	The ID of the associated Stat
		/// </summary>
		public Guid Id
		{
			get
			{
				return this.statId.GuidData;
			}
		}
		
		
		/// <summary>
		/// 	The name of the associated Stat
		/// </summary>
		public string Name
		{
			get
			{
				return this.statReference.Name;
			}
		}
		
		
		/// <summary>
		/// 	The Stat definition from the RPG data assets
		/// </summary>
		/// <value>The stat reference.</value>
		public AbstractStat StatReference
		{
			get
			{
				return this.statReference;
			}
		}
		
		
		/// <summary>
		/// 	The type of stat (Base, Secondary, Skill)
		/// </summary>
		public StatType Type
		{
			get
			{
				return this.type;
			}
		}
		
	}
}
