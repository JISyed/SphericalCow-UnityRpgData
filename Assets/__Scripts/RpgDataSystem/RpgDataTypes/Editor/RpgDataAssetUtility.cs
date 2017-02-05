using UnityEngine;
using UnityEditor;

namespace SphericalCow
{
	/// <summary>
	/// 	Used by developers to make file instances of various AbstractRpgDatType 
	/// 	instances in the Unity project using CustomDataAssetUtility
	/// </summary>
	public static class RpgDataAssetUtility 
	{
		/// <summary>
		/// 	Creates a new asset file containing a new instance of XpProgressor
		/// 	Makes this method accessible from the Unity menu.
		/// </summary>
		[MenuItem("Assets/Create/SphericalCow/RPG Data System/XP Progressor")]
		public static void CreateXpProgressorAsset()
		{
			//StatsAndAttributesRegistry registry = StatAssetUtility.FindStatRegistry();
			XpProgressor newData = CustomDataAssetUtility.CreateAndReturnDataAsset<XpProgressor>();
			newData.Init();
			//registry.AddBasicStat(newStat);
		}
		
		
		/// <summary>
		/// 	Creates a new asset file containing a new instance of BaseStat.
		/// 	Makes this method accessible from the Unity menu.
		/// </summary>
		[MenuItem("Assets/Create/SphericalCow/RPG Data System/Base Stat")]
		public static void CreateBaseStatAsset()
		{
			//StatsAndAttributesRegistry registry = StatAssetUtility.FindStatRegistry();
			BaseStat newData = CustomDataAssetUtility.CreateAndReturnDataAsset<BaseStat>();
			newData.Init();
			//registry.AddBasicStat(newStat);
		}
		
		
		/// <summary>
		/// 	Creates a new asset file containing a new instance of BaseStat.
		/// 	Makes this method accessible from the Unity menu.
		/// </summary>
		[MenuItem("Assets/Create/SphericalCow/RPG Data System/Secondary Stat")]
		public static void CreateSecondaryStatAsset()
		{
			//StatsAndAttributesRegistry registry = StatAssetUtility.FindStatRegistry();
			SecondaryStat newData = CustomDataAssetUtility.CreateAndReturnDataAsset<SecondaryStat>();
			newData.Init();
			//registry.AddBasicStat(newStat);
		}
		
		
		/// <summary>
		/// 	Creates a new asset file containing a new instance of SkillStat.
		/// 	Makes this method accessible from the Unity menu.
		/// </summary>
		[MenuItem("Assets/Create/SphericalCow/RPG Data System/Skill Stat")]
		public static void CreateSkillStatAsset()
		{
			//StatsAndAttributesRegistry registry = StatAssetUtility.FindStatRegistry();
			SkillStat newData = CustomDataAssetUtility.CreateAndReturnDataAsset<SkillStat>();
			newData.Init();
			//registry.AddBasicStat(newStat);
		}
		
		
		
		/// <summary>
		/// 	Creates a new asset file containing a new instance of Ability.
		/// 	Makes this method accessible from the Unity menu.
		/// </summary>
		[MenuItem("Assets/Create/SphericalCow/RPG Data System/Ability")]
		public static void CreateAbilityAsset()
		{
			//StatsAndAttributesRegistry registry = StatAssetUtility.FindStatRegistry();
			Ability newData = CustomDataAssetUtility.CreateAndReturnDataAsset<Ability>();
			newData.Init();
			//registry.AddBasicStat(newStat);
		}
		
	}
}
