using UnityEngine;
using UnityEditor;

namespace SphericalCow.OldCode
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
			StatsAndAttributesRegistry registry = AbilityAssetUtility.FindStatRegistry();
			Ability newAbility = CustomDataAssetUtility.CreateAndReturnDataAsset<Ability>();
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
	