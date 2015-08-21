using UnityEngine;
using System.Collections.Generic;

namespace SphericalCow
{
	/// <summary>
	/// 	The Stats and Attributes Registry is responsible for holding queriable instances 
	///     of AbstractStat and Attributes (not instance).
	/// 	There can only be one of these ever (as in Singleton).
	/// </summary>
	public class StatsAndAttributesRegistry : MonoBehaviour 
	{
		[SerializeField] private GameObject prefab;
		
		[SerializeField] private List<BasicStat> everyBasicStat;
		[SerializeField] private List<SecondaryStat> everySecondaryStat;
		[SerializeField] private List<SkillStat> everySkillStat;
		
		private static StatsAndAttributesRegistry registryInstance;
		
		// Use this for initialization
		void Start () 
		{
			
		}
	}
}
	