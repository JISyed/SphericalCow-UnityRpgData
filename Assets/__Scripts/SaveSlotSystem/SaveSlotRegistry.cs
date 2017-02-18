using UnityEngine;
using System.Collections;
using System.Xml.Serialization;
using System.IO;

namespace SphericalCow
{
	public class SaveSlotRegistry : MonoBehaviour 
	{
		[Range(1, 15)][SerializeField] private int numberOfSaveSlots = 3;
		[SerializeField] private string fileName = "SaveSlots";	// Does not include file extension
		
		private SaveSlotsContainer saveSlots = null;
		
		
		
		/// <summary>
		/// 	The name of the prefab holding the SaveSlotRegistry script
		/// </summary>
		public const string SaveSlotRegistryPrefabName = "SaveSlotRegistryObject";
		
		private static SaveSlotRegistry instance = null;
		
		
		
		
		/// <summary>
		/// 	Singeton access to SaveSlotRegistry
		/// </summary>
		public static SaveSlotRegistry Instance
		{
			get
			{
				if(SaveSlotRegistry.instance == null)
				{
					GameObject registryObject = GameObject.Instantiate(Resources.Load<GameObject>(SaveSlotRegistry.SaveSlotRegistryPrefabName));
					GameObject.DontDestroyOnLoad(registryObject);
					SaveSlotRegistry newRegistry = registryObject.GetComponent<SaveSlotRegistry>();
					if(newRegistry == null)
					{
						Debug.LogError("SaveSlotRegistry script is missing from the prefab " + SaveSlotRegistry.SaveSlotRegistryPrefabName);
						return null;
					}
					SaveSlotRegistry.instance = newRegistry;
					newRegistry.Init();
				}
				
				return SaveSlotRegistry.instance;
			}
		}
		
		
		
		// Called before Start()
		void Awake()
		{
			if(SaveSlotRegistry.instance != null)
			{
				Debug.LogError("There are multiple instances of SaveSlotRegistry running!", this);
			}
		}
		
		
		
		
		
		
		
		/// <summary>
		/// 	Save all the save slots avaliable on disk
		/// </summary>
		/// <returns><c>true</c>, if the slots exist, <c>false</c> otherwise.</returns>
		public bool SaveTheSlots()
		{
			if(this.saveSlots == null)
			{
				Debug.LogError("Trying to save the Save Slots to file, but the save data does not exist!", this);
				return false;
			}
			
			var serializer = new XmlSerializer(typeof(SaveSlotsContainer));
			using(var stream = new FileStream(this.GetSavePath(), FileMode.Create))
			{
				serializer.Serialize(stream, saveSlots);
			}
			
			return true;
		}
		
		
		/// <summary>
		/// 	Gets a save slot at the given slot number. The slot number stats at 1, not 0.
		/// 	After writing into the returned save slot, you MUST call SaveSlotRegistry.Instance.SaveTheSlots()
		/// 	in order to make any changes permanent. You should also mark the slot as occupied (isSlotOccupied = true).
		/// 	Will return null if the given slot number is invalid (too big or too small)
		/// </summary>
		/// <param name="index">The slot number (stats at 1, not 0)</param>
		public SaveSlot GetSlotAt(int slotNumber)
		{
			if(slotNumber < 1)
			{
				Debug.LogError("Trying to get a save slot at an invalid slot number! (Less that Slot 1)", this);
				return null;
			}
			
			if(slotNumber > this.numberOfSaveSlots)
			{
				Debug.LogError("Trying to get a save slot at an invalid slot number! (Exceeding the number of avaliable slots)", this);
				return null;
			}
			
			return this.saveSlots.saveSlots[slotNumber-1];
		}
		
		
		
		
		
		
		/// <summary>
		/// 	Get the number of Save Slots avaliable for use.
		/// 	Only developers can change the amount of avaliable slots from the Unity inspector
		/// </summary>
		public int NumberOfSaveSlots
		{
			get
			{
				return this.numberOfSaveSlots;
			}
		}
		
		
		
		
		
		
		
		
		
		private SaveSlotsContainer LoadTheSlots()
		{
			var serializer = new XmlSerializer(typeof(SaveSlotsContainer));
			using(var stream = new FileStream(this.GetSavePath(), FileMode.Open))
			{
				return serializer.Deserialize(stream) as SaveSlotsContainer;
			}
		}
		
		
		
		private string GetSavePath()
		{
			return Path.Combine(Application.persistentDataPath, this.fileName + ".xml");
		}
		
		
		
		
		/// <summary>
		/// 	Intialization code for the registry
		/// </summary>
		private void Init()
		{
			// Try to load the save slots
			this.saveSlots = this.LoadTheSlots();
			
			// If the file doesn't exist, make a new save slot collection and save it
			if(this.saveSlots == null)
			{
				this.saveSlots = new SaveSlotsContainer(this.numberOfSaveSlots);
				this.SaveTheSlots();
			}
			else
			{
				this.numberOfSaveSlots = this.saveSlots.saveSlots.Length;
			}
			
		}
	}
}
