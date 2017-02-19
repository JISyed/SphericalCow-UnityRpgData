using UnityEngine;
using System.Xml.Serialization;

namespace SphericalCow
{
	/// <summary>
	/// 	A serialization packet to save one applied stat of an RPG character (StatData) to file
	/// </summary>
	public class StatPacket
	{
		[XmlElement("ID")]
		public string statId;
		
		[XmlElement("RawStatPoints")]
		public int rawSpPool;
		
		[XmlElement("UseFactor")]
		public int useFactor;
	}
}
