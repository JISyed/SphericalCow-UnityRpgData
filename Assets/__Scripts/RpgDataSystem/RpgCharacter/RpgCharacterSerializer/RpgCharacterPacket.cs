using UnityEngine;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace SphericalCow
{
	/// <summary>
	/// 	A serialization packet to save an entire RpgCharacterData instance to file, including its subsystems
	/// </summary>
	[XmlRoot("RpgCharacter")]
	public class RpgCharacterPacket
	{
		[XmlAttribute("id")]
		public string id;
		
		[XmlElement("Name")]
		public string name;
		
		[XmlElement("HP")]
		public int hp;
		
		[XmlElement("MaximumHP")]
		public int maxHp;
		
		[XmlElement("AdditionsToMaximumHP")]
		public int additionalMaxHp;
		
		[XmlElement("StatPointAssignmentMethod")]
		public GlobalSpAssignmentType assignmentType;
		
		[XmlElement("XpData")]
		public XpPacket xpDataPacket;
		
		[XmlArray("AppliedStats")]
		[XmlArrayItem("Stat")]
		public List<StatPacket> appliedStats = new List<StatPacket>();
		
		[XmlArray("AppliedAbilities")]
		[XmlArrayItem("Ability")]
		public List<AbilityPacket> appliedAbilities = new List<AbilityPacket>();
	}
}