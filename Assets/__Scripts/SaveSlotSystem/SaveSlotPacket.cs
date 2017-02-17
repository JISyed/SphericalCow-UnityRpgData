using System.Xml.Serialization;

namespace SphericalCow
{
	public class SaveSlotPacket
	{
		[XmlAttribute("slotNumber")]
		public int slotNumber;
		
		[XmlElement("PlayerName")]
		public string playerName;
		
		[XmlElement("PlayerId")]
		public string playerId;
		
		//
		// You are encouraged to add more data here as XML Elements 
		// (whatever the C#'s XML system can serialize)
		//
	}
}
