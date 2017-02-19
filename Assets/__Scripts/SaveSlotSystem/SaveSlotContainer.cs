using System.Xml.Serialization;

namespace SphericalCow
{
	/// <summary>
	/// 	A container class for SaveSlots that can be serialized to an XML file
	/// </summary>
	[XmlRoot("SaveSlotsCollection")]
	public class SaveSlotsContainer
	{
		/// <summary>
		/// 	The array of SaveSlots
		/// </summary>
		[XmlArray("SaveSlots"), XmlArrayItem("SaveSlot")]
		public SaveSlot[] saveSlots;
		
		
		/// <summary>
		/// 	Constructor needs the number of slots the game should support
		/// </summary>
		public SaveSlotsContainer(int numberOfSlots)
		{
			this.saveSlots = new SaveSlot[numberOfSlots];
			for(int i = 0; i < numberOfSlots; i++)
			{
				this.saveSlots[i] = new SaveSlot();
			}
		}
		
		/// <summary>
		/// 	C# XML library needs this
		/// </summary>
		public SaveSlotsContainer()
		{
		}
	}
}