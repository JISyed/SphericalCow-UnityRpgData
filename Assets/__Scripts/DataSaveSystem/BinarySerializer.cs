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
		// Static Data
		//

		/// <summary>
		/// 	The file extension for anything saved with this serializer.
		/// 	Does NOT include the dot in the beginning.
		/// </summary>
		private const string EXTENSION = "scdata";


		//
		// Methods
		//

		public static void Save<T>(T objToBeSaved)
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Create(GetFullDataPath<T>());

			bf.Serialize(file, objToBeSaved);
			file.Close();
		}

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

			Debug.LogWarning("BinarySerializer: Trying to load a file that doesn't exist. Returning null.");
			return default(T);
		}

		private static string GetFullDataPath<T>()
		{
			return Application.persistentDataPath + "/" + typeof(T).GetType().ToString() + "." + EXTENSION;
		}

	}	// end BinarySerializer
}	// end namespace SphericalCow
