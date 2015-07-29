// Written by Jacob Pennock
// www.jacobpennock.com/Blog/?page_id=715

// Modified and annotated by Jibran Syed

using ScriptableObject = UnityEngine.ScriptableObject;
using EditorUtility = UnityEditor.EditorUtility;
using AssetDatabase = UnityEditor.AssetDatabase;
using Selection = UnityEditor.Selection;
using Path = System.IO.Path;

namespace SphericalCow
{
	public static class CustomDataAssetUtility 
	{
		// Creates a Unity .asset file in your project
		// storing data from the child class of ScriptableObject
		public static void CreateDataAsset<T>() where T : ScriptableObject
		{
			// Create a new data asset instance that is child of ScriptableObject
			T newAsset = ScriptableObject.CreateInstance<T>();
			
			// Retrieve the path of the current selection in the Unity Project view
			string assetPath = AssetDatabase.GetAssetPath(Selection.activeObject);
			if(assetPath.Equals("") == true)
			{
				assetPath = "Assets";
			}
			else if(Path.GetExtension(assetPath).Equals("") == false)
			{
				string filename = Path.GetFileName(assetPath);
				assetPath = assetPath.Replace(filename , "");
			}
			
			// Determine the actual path to put the new asset
			string assetFilename = "/New " + typeof(T).ToString() + ".asset";
			string newAssetFullPath = AssetDatabase.GenerateUniqueAssetPath(assetPath + assetFilename);
			
			// Create a new data asset into a file on the chosen path
			AssetDatabase.CreateAsset(newAsset, newAssetFullPath);
			
			// Save and Focus
			AssetDatabase.SaveAssets();
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = newAsset;
		}
		
	}
}