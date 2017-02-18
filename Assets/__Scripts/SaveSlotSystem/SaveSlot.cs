using System.Xml.Serialization;

namespace SphericalCow
{
	public class SaveSlot
	{
		//
		// Do NOT edit these variables!
		// Player name and ID are used in aiding the RpgCharacter system serialization
		//
		
		/// <summary>
		/// 	If false, the slot is empty
		/// </summary>
		[XmlAttribute("occupied")]
		public bool isSlotOccupied = false;
		
		[XmlElement("PlayerName")]
		public string playerName = "";
		
		[XmlElement("PlayerId")]
		public string playerId = "";
		
		//
		// You are encouraged to add more data here as XML Elements 
		// (whatever the C#'s XML system can serialize)
		//
		
		
		
		
		
	}
}
