using UnityEngine;
using System.Xml.Serialization;
using System.Collections.Generic;

namespace SphericalCow
{
	/// <summary>
	/// 	A serialization packet to save an entire RpgCharacterData instance to file, including its subsystems
	/// </summary>
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
		
		// TODO: Serialize XpPacket
		
		
		// TODO: Serialize a list of StatPackets
		
		
		// TODO: Serialize a list of AbilityPackets
		
		
		
	}
}