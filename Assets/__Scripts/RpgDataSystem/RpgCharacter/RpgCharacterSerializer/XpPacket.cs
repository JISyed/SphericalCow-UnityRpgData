using UnityEngine;
using System.Xml.Serialization;

namespace SphericalCow
{
	/// <summary>
	/// 	A serialization packet to save the XP and level progression data of an RPG character (XpData) to file
	/// </summary>
	public class XpPacket 
	{
		[XmlElement("ProgressorId")]
		public string xpProgessorId;
		
		[XmlElement("CurrentLevel")]
		public int level;
		
		[XmlElement("XP")]
		public int xp;
		
		[XmlElement("XpToTheNextLevel")]
		public int xpToNextLevel;
		
		[XmlElement("LevelMultiplier")]
		public int currentLevelMultiplier;
		
		[XmlElement("OldValueMultiplier")]
		public int currentOldValueMultiplier;
	}
}