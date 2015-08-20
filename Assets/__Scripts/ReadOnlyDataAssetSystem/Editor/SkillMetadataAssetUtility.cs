using UnityEngine;
using UnityEditor;

namespace SphericalCow
{
	/// <summary>
	/// 	Used by developers to make a file instance of SkillMetadata in the Unity project using CustomDataAssetUtility
	/// </summary>
	public static class SkillMetadataAssetUtility 
	{
		/// <summary>
		/// 	Creates a new asset file containing a new instance of SkillMetadata.
		/// 	Makes this method accessible from the Unity menu.
		/// </summary>
		[MenuItem("Assets/Create/SphericalCow/SkillMetadata")]
		public static void CreateDataAsset()
		{
			CustomDataAssetUtility.CreateDataAsset<Old.SkillMetadata>();
		}
	}
}
	