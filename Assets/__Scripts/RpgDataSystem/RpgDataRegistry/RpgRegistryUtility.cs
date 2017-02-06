using UnityEngine;

namespace SphericalCow
{
	/// <summary>
	/// 	A static utility class to helo the RpgDataRegistry
	/// </summary>
	public static class RpgRegistryUtility
	{
		// The following block is code that only runs in the Unity Editor
		
		#if UNITY_EDITOR
			
			/// <summary>
			/// 	A package sturcture for XpProgressors
			/// </summary>
			public struct AdderOfXpProgressor
			{
				public XpProgressor xpProgressor;
			}
			
			/// <summary>
			/// 	A package sturcture for BaseStats
			/// </summary>
			public struct AdderOfBaseStat
			{
				public BaseStat baseStat;
			}
			
			/// <summary>
			/// 	A package sturcture for SecondaryStats
			/// </summary>
			public struct AdderOfSecondaryStat
			{
				public SecondaryStat secondaryStat;
			}
			
			/// <summary>
			/// 	A package sturcture for SkillStats
			/// </summary>
			public struct AdderOfSkillStat
			{
				public SkillStat skillStat;
			}
			
		
			/// <summary>
			/// 	A package sturcture for Abilities
			/// </summary>
			public struct AdderOfAbility
			{
				public Ability ability;
			}
		
		#endif
		
	}
}
