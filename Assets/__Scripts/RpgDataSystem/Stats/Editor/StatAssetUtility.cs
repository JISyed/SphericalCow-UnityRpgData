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
		/// 	Creates a new asset file containing a new instance of BasicStat.
		/// 	Makes this method accessible from the Unity menu.
		/// </summary>
		[MenuItem("Assets/Create/SphericalCow/RPG Data System/Basic Stat")]
		public static void CreateBasicStatDataAsset()
		{
			StatsAndAttributesRegistry registry = StatAssetUtility.FindStatRegistry();
			BasicStat newStat = CustomDataAssetUtility.CreateAndReturnDataAsset<BasicStat>();
			registry.AddBasicStat(newStat);
		}
		
		/// <summary>
		/// 	Creates a new asset file containing a new instance of SecondaryStat.
		/// 	Makes this method accessible from the Unity menu.
		/// </summary>
		[MenuItem("Assets/Create/SphericalCow/RPG Data System/Secondary Stat")]
		public static void CreateSecondaryStatDataAsset()
		{
			StatsAndAttributesRegistry registry = StatAssetUtility.FindStatRegistry();
			SecondaryStat newStat = CustomDataAssetUtility.CreateAndReturnDataAsset<SecondaryStat>();
			registry.AddSecondaryStat(newStat);
		}
		
		/// <summary>
		/// 	Creates a new asset file containing a new instance of SkillStat.
		/// 	Makes this method accessible from the Unity menu.
		/// </summary>
		[MenuItem("Assets/Create/SphericalCow/RPG Data System/Skill Stat")]
		public static void CreateSkillStatDataAsset()
		{
			StatsAndAttributesRegistry registry = StatAssetUtility.FindStatRegistry();
			SkillStat newStat = CustomDataAssetUtility.CreateAndReturnDataAsset<SkillStat>();
			registry.AddSkillStat(newStat);
		}
		
		
		/// <summary>
		///  	Searches the Unity project for the prefab that holds the only instance of StatsAndAttributesRegistry.
		///  	Can only be used in the Unity Editor, not game!
		/// </summary>
		private static StatsAndAttributesRegistry FindStatRegistry()
		{
			string[] folders = {"Assets/__Scripts/RpgDataSystem"};
			string[] searchResults = AssetDatabase.FindAssets("StatsAndAttributesRegistryObject", folders);
			if(searchResults == null)
			{
				Debug.LogError("Could not find the prefab StatsAndAttributesRegistryObject in the project! Did someone delete it?");
			}
			else
			{
				// Get the path of the given GUIDs
				string path = AssetDatabase.GUIDToAssetPath(searchResults[0]);
				
				// Get the GameObject from the path
				GameObject baseObject = AssetDatabase.LoadAssetAtPath<GameObject>(path);
				Object oldSelection = Selection.activeObject;
				Selection.activeObject = baseObject;

				// Get the registry from the game object
				StatsAndAttributesRegistry theRegistry = baseObject.GetComponent<StatsAndAttributesRegistry>();
				if(theRegistry == null)
				{
					Debug.LogError("StatsAndAttributesRegistry is missing from the associated prefab!");
				}

				Selection.activeObject = oldSelection;
				return theRegistry;
			}
			return null;
		}
	}
}
