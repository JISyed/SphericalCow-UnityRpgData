using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System.Collections.Generic;
using Guid = System.Guid;

namespace SphericalCow.Testing
{
	public class QuickSaveSlotTester : MonoBehaviour 
	{
		private const string SaveSlotSubPath = "SaveSlots.xml";
		
		private enum QSST_SaveOrLoad
		{
			Save,
			Load
		}
		
		private QSST_SaveOrLoad saveOrLoad = QSST_SaveOrLoad.Load;
		
		private SaveSlotPacketsContainer saveSlots;
		private List<string> randomNames = new List<string>();
		
		
		// Use this for initialization
		void Start () 
		{
			// Setup the random names
			this.SetUpRandomNames();
			
			if(this.saveOrLoad == QSST_SaveOrLoad.Save)
			{
				// Make the save slots collection
				this.saveSlots = new SaveSlotPacketsContainer();
				
				// Make random save slots
				int numberOfSlots = 3;
				for(int i = 1; i <= numberOfSlots; i++)
				{
					SaveSlotPacket newPacket = new SaveSlotPacket();
					newPacket.slotNumber = i;
					newPacket.playerName = this.GetRandomName();
					newPacket.playerId = this.GetRandomId();
					
					this.saveSlots.saveSlots.Add(newPacket);
				}
				
				string slotCollectionPath = Path.Combine(Application.persistentDataPath, QuickSaveSlotTester.SaveSlotSubPath);
				
				this.Save(slotCollectionPath, this.saveSlots);
			}
			else if(this.saveOrLoad == QSST_SaveOrLoad.Load)
			{
				string slotCollectionPath = Path.Combine(Application.persistentDataPath, QuickSaveSlotTester.SaveSlotSubPath);
				
				this.saveSlots = this.Load(slotCollectionPath);
				
				foreach(SaveSlotPacket saveSlot in this.saveSlots.saveSlots)
				{
					Debug.Log("SlotNumber: " + saveSlot.slotNumber.ToString() +
					          "  |  PlayerName: " + saveSlot.playerName +
					          "  |  PlayerId:  " + saveSlot.playerId
					);
				}
			}
			
			
			
		}
		
		
		
		public void Save(string path, SaveSlotPacketsContainer saveSlots)
		{
			var serializer = new XmlSerializer(typeof(SaveSlotPacketsContainer));
			using(var stream = new FileStream(path, FileMode.Create))
			{
				serializer.Serialize(stream, saveSlots);
			}
		}
		
		public SaveSlotPacketsContainer Load(string path)
		{
			var serializer = new XmlSerializer(typeof(SaveSlotPacketsContainer));
			using(var stream = new FileStream(path, FileMode.Open))
			{
				return serializer.Deserialize(stream) as SaveSlotPacketsContainer;
			}
		}
		
		
		
		
		
		
		private void SetUpRandomNames()
		{
			this.randomNames.Add("Bob");
			this.randomNames.Add("Joe");
			this.randomNames.Add("Jack");
			this.randomNames.Add("Luke");
			this.randomNames.Add("Skywalker");
			this.randomNames.Add("Vader");
			this.randomNames.Add("Tyler");
			this.randomNames.Add("Sam");
			this.randomNames.Add("Frodo");
			this.randomNames.Add("Bilbo");
			this.randomNames.Add("Harry");
			this.randomNames.Add("Wess");
			this.randomNames.Add("Salvador");
			this.randomNames.Add("Dali");
			this.randomNames.Add("Irene");
			this.randomNames.Add("Vikki");
			this.randomNames.Add("Zolly");
		}
		
		private string GetRandomName()
		{
			string chosenName = "ERROR";
			
			int numOfNames = this.randomNames.Count;
			int randomIndex = Random.Range(0, numOfNames);
			
			int indexTally = 0;
			foreach(string name in this.randomNames)
			{
				if(indexTally == randomIndex)
				{
					chosenName = name;
					break;
				}
				
				indexTally++;
			}
			
			return chosenName;
		}
		
		private string GetRandomId()
		{
			return Guid.NewGuid().ToString();
		}
	}
}
