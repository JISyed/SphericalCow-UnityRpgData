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
		/// 	Creates a new asset file containing a new instance of #######.
		/// 	Makes this method accessible from the Unity menu.
		/// </summary>
		[MenuItem("Assets/Create/SphericalCow/Test/RPG Data System/Blank RPG Data Type")]
		public static void CreateBlankRpgDataAsset()
		{
			//StatsAndAttributesRegistry registry = StatAssetUtility.FindStatRegistry();
			AbstractRpgDataType newData = CustomDataAssetUtility.CreateAndReturnDataAsset<AbstractRpgDataType>();
			newData.Init();
			//registry.AddBasicStat(newStat);
		}
		
		
		/// <summary>
		/// 	Creates a new asset file containing a new instance of #######.
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
		
	}
}
