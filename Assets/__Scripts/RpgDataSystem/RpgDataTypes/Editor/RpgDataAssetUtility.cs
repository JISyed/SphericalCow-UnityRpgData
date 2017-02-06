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
		private const string RpgSystemProjectPath = "Assets/__Scripts/RpgDataSystem";
		
		private static RpgDataRegistry rpgRegistryInstance = null;
		
		
		
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
		
		
		
		
		
		
		
		/// <summary>
		///  	Searches the Unity project for the prefab that holds the only instance of RpgDataRegistryObject.
		///  	Editor only.
		/// </summary>
		private static RpgDataRegistry FindRpgDataRegistry()
		{
			if(RpgDataAssetUtility.rpgRegistryInstance == null)
			{
				string[] folders = {RpgDataAssetUtility.RpgSystemProjectPath};
				string[] searchResults = AssetDatabase.FindAssets(RpgDataRegistry.RpgRegistryPrefabName, folders);
				
				if(searchResults == null)
				{
					Debug.LogError("Could not find the prefab RpgDataRegistryObject in the project! Did someone move or delete it?");
					RpgDataAssetUtility.rpgRegistryInstance = null;
				}
				else
				{
					// Get the path of the given GUIDs
					string path = AssetDatabase.GUIDToAssetPath(searchResults[0]);
					
					// Get the GameObject from the path
					GameObject registryObject = AssetDatabase.LoadAssetAtPath<GameObject>(path);
					
					Object oldSelection = Selection.activeObject;
					Selection.activeObject = registryObject;
					
					// Get the registry from the game object
					RpgDataRegistry theRegistry = registryObject.GetComponent<RpgDataRegistry>();
					if(theRegistry == null)
					{
						Debug.LogError("RpgDataRegistry is missing from the associated prefab RpgDataRegistryObject!");
					}
					
					Selection.activeObject = oldSelection;
					
					RpgDataAssetUtility.rpgRegistryInstance = theRegistry;
				}
			}
			
			return RpgDataAssetUtility.rpgRegistryInstance;
		}
		
		
		
		
		
	}
}
