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
				int finalSp = this.rawSpPool;
				
				// TODO: Get additional SP from the SpDeriver
				
				// TODO: Determing additional SP from Abilities
				
				
				
				return finalSp;
			}
		}
		
		
	}
}
