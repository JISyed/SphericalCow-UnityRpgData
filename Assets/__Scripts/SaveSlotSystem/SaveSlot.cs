using System.Xml.Serialization;

namespace SphericalCow
{
	/// <summary>
	/// 	A set of relevant data used to save the game's progress. 
	/// 	Put whatever is needed to "save the game" on disk
	/// </summary>
	public class SaveSlot
	{
		//
		// Do NOT edit these variables!
		// Player name and ID are used in aiding the RpgCharacter system serialization
		//
		
		/// <summary>
		/// 	If false, the slot is empty. You must mark this as true if you are going to use it
		/// </summary>
		[XmlAttribute("occupied")]
		public bool isSlotOccupied = false;
		
		/// <summary>
		/// 	The name of the player of the game. Used to test RpgCharacterData serialization
		/// </summary>
		[XmlElement("PlayerName")]
		public string playerName = "";
		
		/// <summary>
		/// 	The ID of the player of the game. Used to test RpgCharacterData serialization
		/// </summary>
		[XmlElement("PlayerId")]
		public string playerId = "";
		
		//
		// You are encouraged to add more data here as XML Elements 
		// (whatever the C#'s XML system can serialize)
		//
		
		
		
		
		
	}
}
