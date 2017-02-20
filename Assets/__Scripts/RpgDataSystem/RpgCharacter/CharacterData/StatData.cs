using UnityEngine;
using Guid = System.Guid;
using System.Collections.Generic;

namespace SphericalCow
{
	/// <summary>
	/// 	Data pertaining to any single Stat that includes Stat-Points (SP) for an RpgCharacter
	/// </summary>
	public class StatData 
	{
		private AbstractStat statReference;
		private SaveableGuid statId;
		private StatType type;
		private int rawSpPool;
		private int useFactor;
		private AbstractSpDeriver linkedStatsDerivedPool;
		private AbilityAggregator abilityModifications;
		private List<StatData> characterAppliedStats;
		
		
		/// <summary>
		/// 	StatData Constructor needs the stat definition file and the RpgCharacter's list of stats
		/// </summary>
		public StatData(AbstractStat newStat, List<StatData> appliedStats)
		{
			Debug.Assert(newStat != null, "New StatData being given a null AbstractStat!");
			Debug.Assert(appliedStats != null, "New StatData being given null list of applied stats!");
			
			this.statReference = newStat;
			this.characterAppliedStats = appliedStats;
			this.statId = new SaveableGuid(newStat.Id);
			this.type = newStat.GetStatType();
			this.rawSpPool = 0;
			this.useFactor = 1;
			
			
			// Initialize the SpDeriver
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
			
			
			// Initialize the AbilityAggregator
			this.abilityModifications = new AbilityAggregator(this.statId.GuidData);
			
		}
		
		
		/// <summary>
		/// 	Deserialization Constructor
		/// </summary>
		public StatData(StatPacket statPacket, List<StatData> appliedStats)
		{
			Debug.Assert(statPacket != null, "New StatData being given a null StatPacket!");
			Debug.Assert(appliedStats != null, "New StatData being given null list of applied stats!");
			
			this.statId = new SaveableGuid(statPacket.statId);
			this.statReference = RpgDataRegistry.Instance.SearchAnyStat(this.statId.GuidData);
			
			Debug.Assert(this.statReference != null, "Could not deserialize a stat because its definition cannot be found. ID: " + statPacket.statId);
			
			this.type = this.statReference.GetStatType();
			this.rawSpPool = statPacket.rawSpPool;
			this.useFactor = statPacket.useFactor;
			this.characterAppliedStats = appliedStats;
			
			// Initialize the SpDeriver
			switch (this.type) 
			{
			case StatType.Base:
				this.linkedStatsDerivedPool = new BaseSpDeriver(this.statReference as BaseStat);
				break;
			case StatType.Secondary:
				this.linkedStatsDerivedPool = new SecondarySpDeriver(this.statReference as SecondaryStat);
				break;
			case StatType.Skill:
				this.linkedStatsDerivedPool = new SkillSpDeriver(this.statReference as SkillStat);
				break;
			default:
				Debug.LogError("Somebody added a new entry into the enum StatType");
				break;
			}
			
			
			// Initialize the AbilityAggregator
			this.abilityModifications = new AbilityAggregator(this.statId.GuidData);
			
		}
		
		
		
		
		
		/// <summary>
		/// 	Increases the SP of this stat's individual (raw) pool. This action cannot be undone! 
		/// 	Do NOT call directly! Only RpgCharacterData is allowed to call
		/// </summary>
		/// <param name="spToAdd">SP to add to this stat. Will be made positve if negative</param>
		public void AddStatPointsToRawPool(int spToAdd)
		{
			if(spToAdd < 0)
			{
				spToAdd = -spToAdd;
			}
			
			this.rawSpPool += spToAdd;
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
		/// 	Invokes the calculation of the SP derived from this stat's linked stats
		/// </summary>
		public void RecalculateLinkedStatPoints()
		{
			this.linkedStatsDerivedPool.DeriveSp(this.characterAppliedStats);
		}
		
		
		
		
		/// <summary>
		/// 	Apply an ability onto this stat. Don't call directly; call from RpgCharacterData
		/// </summary>
		public void ApplyOneAbility(AbilityData newAbility)
		{
			this.abilityModifications.ApplyAbility(newAbility.AbilityReference);
		}
		
		
		/// <summary>
		/// 	Remove an ability from this stat. Don't call directly; call from RpgCharacterData
		/// </summary>
		public void RemoveOneAbility(AbilityData oldAbility)
		{
			this.abilityModifications.RemoveAbility(oldAbility.AbilityReference);
		}
		
		
		
		/// <summary>
		/// 	Only to be called by the RpgCharacterSerializer
		/// </summary>
		public StatPacket ExportSerializationPacket()
		{
			StatPacket newPacket = new StatPacket();
			
			newPacket.statId = this.statId.GuidString;
			newPacket.rawSpPool = this.RawStatPoints;
			newPacket.useFactor = this.UseFactor;
			
			return newPacket;
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
				this.CalculateAbilityModifiedSp(ref finalSp);
				
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
		/// 	The amount of SP that is modified from abilities but not calculated from linked stats
		/// </summary>
		public int ModifiedUnlinkedStatPoints
		{
			get
			{
				// Start with the SP from the raw pool (the SP that is not modified by anything)
				int finalSp = this.rawSpPool;
				
				// Determing additional SP from Abilities (may alter the SP greatly)
				this.CalculateAbilityModifiedSp(ref finalSp);
				
				return finalSp;
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
		
		
		
		/// <summary>
		/// 	Calculate the this stat's ability modifications applied onto this stat's SP
		/// </summary>
		/// <param name="finalSp">A reference to the accumulation SP calculations from the raw and derived pools</param>
		private void CalculateAbilityModifiedSp(ref int ref_finalSp)
		{
			AbilityAggregator.NetModifications statMods = this.abilityModifications.AbilityModifiedSp;
			if(statMods.netLimit == ModifierLimitType.NoLimit)	// IncreaseBy or DecreaseBy only
			{
				ref_finalSp += statMods.netValue;
				
				// SP shouldn't drop below 0
				if(ref_finalSp < 0)
				{
					ref_finalSp = 0;
				}
			}
			else
			{
				// For Stats that are stuck with IncreaseTo or DecreaseTo
				if(statMods.netLimit == ModifierLimitType.UpperLimit)	// DecreaseTo
				{
					if(ref_finalSp > statMods.netValue)
					{
						ref_finalSp = statMods.netValue;
					}
				}
				else if(statMods.netLimit == ModifierLimitType.LowerLimit)	// IncreaseTo
				{
					if(ref_finalSp < statMods.netValue)
					{
						ref_finalSp = statMods.netValue;
					}
				}
				else
				{
					Debug.LogError("Somebody added a new enum entry into ModifierLimitType!");
				}
			}
		}
		
		
	}
}
