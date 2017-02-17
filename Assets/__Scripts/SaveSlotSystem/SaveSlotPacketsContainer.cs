using System.Xml.Serialization;
using System.Collections.Generic;

namespace SphericalCow
{
	[XmlRoot("SaveSlotsCollection")]
	public class SaveSlotPacketsContainer
	{
		[XmlArray("SaveSlots")]
		[XmlArrayItem("SaveSlot")]
		public List<SaveSlotPacket> saveSlots = new List<SaveSlotPacket>();
	}
}