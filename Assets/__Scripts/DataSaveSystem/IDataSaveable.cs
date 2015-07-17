
namespace SphericalCow
{
	/// <summary>
	/// 	Defines any class that can be saved to file.
	/// 	Works with BinarySerializer
	/// </summary>
	public abstract class IDataSaveable
	{
		/// <summary>
		/// 	Gets called before the object is saved into a file
		/// </summary>
		protected abstract void OnBeforeSave();

		/// <summary>
		/// 	Gets called right after an object is loaded from file
		/// </summary>
		/// <param name="loadedData">The ojbect that was just loaded from file</param>
		protected abstract void OnAfterLoad(IDataSaveable loadedData);

		/// <summary>
		/// 	Saves the current object's state into a file named after itself
		/// </summary>
		public void Save()
		{

		}

		/// <summary>
		/// 	Loads this object from a file named after itself
		/// </summary>
		public void Load()
		{

		}



	}
}