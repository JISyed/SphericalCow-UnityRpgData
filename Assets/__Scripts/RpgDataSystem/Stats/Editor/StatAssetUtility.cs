using UnityEngine;
using UnityEditor;

namespace SphericalCow
{
	/// <summary>
	/// 	Used by developers to make file instances of various AbstractStat 
	/// 	instances in the Unity project using CustomDataAssetUtility
	/// </summary>
	public static class StatAssetUtility
	{
		/// <summary>
		/// 	Creates a new asset file containing a new instance of SkillMetadata.
		/// 	Makes this method accessible from the Unity menu.
		/// </summary>
		[MenuItem("Assets/Create/SphericalCow/RPG Data System/Basic Stat")]
		public static void CreateBasicStatDataAsset()
		{
			CustomDataAssetUtility.CreateDataAsset<BasicStat>();
		}
	}
}
