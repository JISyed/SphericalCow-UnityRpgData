using UnityEngine;
using UnityEditor;

namespace SphericalCow
{
	public class AbilityAssetUtility 
	{
	
		/// <summary>
		/// 	Creates a new asset file containing a new instance of Ability.
		/// 	Makes this method accessible from the Unity menu.
		/// </summary>
		[MenuItem("Assets/Create/SphericalCow/RPG Data System/Ability")]
		public static void CreateAbilityDataAsset()
		{
			Ability newAbility = CustomDataAssetUtility.CreateAndReturnDataAsset<Ability>();
			StatsAndAttributesRegistry registry = AbilityAssetUtility.FindStatRegistry();
			registry.AddAbility(newAbility);
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
				
				// Get the registry from the game object
				StatsAndAttributesRegistry theRegistry = baseObject.GetComponent<StatsAndAttributesRegistry>();
				
				return theRegistry;
			}
			return null;
		}
	}
}
	