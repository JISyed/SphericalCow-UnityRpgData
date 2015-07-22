using UnityEngine;
using System;

namespace SphericalCow
{
	/// <summary>
	/// 	Defines any class that can be saved to file.
	/// 	Works with BinarySerializer
	/// </summary>
	[Serializable]
	public abstract class IDataSaveable<T> where T : IDataSaveable<T>
	{
		/// <summary>
		/// 	Constructor
		/// </summary>
		public IDataSaveable()
		{
			// Make sure that any child class of IDataSaveable has the [Serializable] attribute!
			Type t = GetType();
			if(t.IsDefined(typeof(SerializableAttribute), false) == false)
			{
				Debug.LogError("An instance of IDataSavable doesn't have the [Serializable] attribute!");
				Debug.LogException(new InvalidOperationException());
			}
		}

		/// <summary>
		/// 	Gets called before the object is saved into a file
		/// </summary>
		protected abstract void OnBeforeSave();

		/// <summary>
		/// 	Gets called right after an object is loaded from file
		/// </summary>
		/// <param name="loadedData">The ojbect that was just loaded from file</param>
		protected abstract void OnAfterLoad(IDataSaveable<T> loadedData);

		/// <summary>
		/// 	Saves the current object's state into a file named after itself
		/// </summary>
		public void Save()
		{
			// Call the pre-save procedure
			this.OnBeforeSave();

			// Save this object to file
			BinarySerializer.Save<T>((T) this);
		}

		/// <summary>
		/// 	Loads this object from a file named after itself
		/// </summary>
		public void Load()
		{
			// Load this object from file
			T loadedDataObject = BinarySerializer.Load<T>();

			// Call the post-load procedure
			this.OnAfterLoad(loadedDataObject);
		}


	} // end IDataSaveable
} // end namespace SphericalCow