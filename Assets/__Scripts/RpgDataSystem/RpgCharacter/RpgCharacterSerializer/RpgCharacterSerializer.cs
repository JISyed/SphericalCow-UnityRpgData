using UnityEngine;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace SphericalCow
{
	/// <summary>
	/// 	Utility class that is responsible for saving and loading RpgCharacterData instances from file
	/// </summary>
	public static class RpgCharacterSerializer
	{
		private const string FileSubPath = "RpgCharacters";
		
		
		/// <summary>
		/// 	Save the RpgCharacter to file. The file will be named based off the character's name and GUID
		/// </summary>
		public static void SaveCharacter(RpgCharacterData character)
		{
			if(character == null)
			{
				Debug.LogError("Cannot save a null RpgCharacter!");
				return;
			}
			
			RpgCharacterPacket packet = character.ExportSerializationPacket();
			string savePath = RpgCharacterSerializer.GetFullPath(character.Name, character.Id.ToString());
			
			RpgCharacterSerializer.WriteToXml(packet, savePath);
		}
		
		
		/// <summary>
		/// 	Load an RpgCharacter from file, given its name and it ID (GUID as string).
		/// 	It is the developer's responsibility to somehow save the GUID of the player before saving the character.
		/// 	Saving the GUID can be done in PlayerPrefs or use the SaveSlot system provided in this project
		/// </summary>
		/// <param name="characterName">Character name.</param>
		/// <param name="characterId">Character GUID as a string.</param>
		public static RpgCharacterData LoadCharacter(string characterName, string characterId)
		{
			if(string.IsNullOrEmpty(characterName))
			{
				Debug.LogError("Cannot load an RpgCharacter with a blank name!");
				return null;
			}
			if(string.IsNullOrEmpty(characterId))
			{
				Debug.LogError("Cannot load an RpgCharacter with a blank GUID!");
				return null;
			}
			
			
			string loadPath = RpgCharacterSerializer.GetFullPath(characterName, characterId);
			RpgCharacterPacket packet = RpgCharacterSerializer.ReadFromXml(loadPath);
			RpgCharacterData character = new RpgCharacterData(packet);
			
			Debug.Assert(character != null, "RpgCharacter of name \"" + characterName + "\" failed to load!");
			
			return character;
		}
		
		
		
		/// <summary>
		/// 	Get the full path of the RpgCharacter given its name and GUID
		/// </summary>
		private static string GetFullPath(string name, string guid)
		{
			Debug.Assert(string.IsNullOrEmpty(name) == false);
			Debug.Assert(string.IsNullOrEmpty(guid) == false);
			
			return Path.Combine(Path.Combine(Application.persistentDataPath, 
			                                 RpgCharacterSerializer.FileSubPath),
			                    name + "-" + guid + ".xml");
		}
		
		
		
		
		
		/// <summary>
		/// 	Writes the RpgCharacterPacket into a file at the given path.
		/// 	The path should be an XML file
		/// </summary>
		private static void WriteToXml(RpgCharacterPacket packet, string path)
		{
			// Create the subdiretory if it doesn't exist
			FileInfo saveFileInfo = new FileInfo(path);
			saveFileInfo.Directory.Create();	// Does nothing if it already exists
			
			// Write actual file
			var serializer = new XmlSerializer(typeof(RpgCharacterPacket));
			using(var stream = new FileStream(path, FileMode.Create))
			{
				serializer.Serialize(stream, packet);
			}
		}
		
		
		/// <summary>
		/// 	Reads the RpgCharacterPacket from a file at the given path.
		/// 	The path should be an XML file
		/// </summary>
		private static RpgCharacterPacket ReadFromXml(string path)
		{
			// Create the subdiretory if it doesn't exist
			FileInfo saveFileInfo = new FileInfo(path);
			saveFileInfo.Directory.Create();	// Does nothing if it already exists
			
			try
			{
				var serializer = new XmlSerializer(typeof(RpgCharacterPacket));
				using(var stream = new FileStream(path, FileMode.Open))
				{
					return serializer.Deserialize(stream) as RpgCharacterPacket;
				}
			}
			catch(FileNotFoundException e)
			{
				Debug.LogError("The file \"" + e.FileName + "\" could not be found!");
				
				return null;
			}
		}
		
		
	}
}