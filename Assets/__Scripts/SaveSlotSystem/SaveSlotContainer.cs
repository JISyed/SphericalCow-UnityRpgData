using System.Xml.Serialization;

namespace SphericalCow
{
	[XmlRoot("SaveSlotsCollection")]
	public class SaveSlotsContainer
	{
		[XmlArray("SaveSlots"), XmlArrayItem("SaveSlot")]
		public SaveSlot[] saveSlots;
		
		
		/// <summary>
		/// 	Constructor needs the number of slots the game should support
		/// </summary>
		public SaveSlotsContainer(int numberOfSlots)
		{
			this.saveSlots = new SaveSlot[numberOfSlots];
		}
	}
}