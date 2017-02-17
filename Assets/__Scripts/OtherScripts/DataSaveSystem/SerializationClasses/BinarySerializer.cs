using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary; 
using System.IO;

namespace SphericalCow
{
	/// <summary>
	/// 	Responsible for saving and loading an entire object's state into a file
	/// 	Will only work on data that has the [System.Serializable] attribute.
	/// 	NOT recommended to serialize references; your milage will vary
	/// </summary>
	public static class BinarySerializer
	{
		//
		// Methods
		//

		/// <summary>
		/// 	Save the given object to a file named after itself in Unty's persistent data path
		/// </summary>
		/// <param name="objToBeSaved">Object to be saved.</param>
		/// <typeparam name="T">Must be serializable</typeparam>
		public static void Save<T>(T objToBeSaved)
		{
			// Creating a file will make a new one if one doesn't exist,
			// or it will overwrite if the file already exists
			FileStream file = File.Create(GetFullDataPath<T>());

			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(file, objToBeSaved);
			file.Close();
		}

		/// <summary>
		/// 	Load an object back into memory from Unity's persistent data path
		/// </summary>
		/// <typeparam name="T">Must be serializable</typeparam>
		public static T Load<T>()
		{
			if(File.Exists( GetFullDataPath<T>() ))
			{
				BinaryFormatter bf = new BinaryFormatter();
				FileStream file = File.Open(GetFullDataPath<T>(), FileMode.Open);
				T loadedData = (T) bf.Deserialize(file);
				file.Close();

				return loadedData;
			}

			Debug.LogWarning("BinarySerializer: Trying to load a file that doesn't exist: \"" 
			                 + GetFullDataPath<T>() 
			                 + "\" Returning null.");

			// Default returns 0 for value-types and null for reference-types
			return default(T);
		}

		public static string GetFullDataPath<T>()
		{
			return Application.persistentDataPath + "/" + typeof(T).Name + "." + SerializationUtility.EXTENSION;
		}

	}	// end BinarySerializer
}	// end namespace SphericalCow
