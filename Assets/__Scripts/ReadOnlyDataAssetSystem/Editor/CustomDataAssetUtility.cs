// Based off code from Unity3D Wiki
// http://wiki.unity3d.com/index.php/CreateScriptableObjectAsset2

// Modified and annotated by Jibran Syed

using ProjectWindowUtil = UnityEditor.ProjectWindowUtil;
using ScriptableObject = UnityEngine.ScriptableObject;

namespace SphericalCow
{
	/// <summary>
	/// 	Editor-only class used to create read-only data assets in the Unity project.
	/// 	Cannot be invoked in final game code!
	/// </summary>
	public static class CustomDataAssetUtility 
	{
		// Creates a Unity .asset file in your project
		// storing data from the child class of ScriptableObject
		public static void CreateDataAsset<T>() where T : ScriptableObject
		{
			// Create a new data asset instance that is child of ScriptableObject
			T newAsset = ScriptableObject.CreateInstance<T>();
			
			// Create a new file in the Project View on where ever the last selection was
			// and automagically offer the ability to rename the file!
			ProjectWindowUtil.CreateAsset(newAsset, typeof(T).Name + ".asset");
		}
		
	}
}