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
		// AbilityAggregator
		[SerializeField] private int statUseCounter;
	}
}
