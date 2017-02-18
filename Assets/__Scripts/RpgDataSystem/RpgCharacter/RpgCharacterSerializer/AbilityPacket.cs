using UnityEngine;
using System.Xml.Serialization;

namespace SphericalCow
{
	/// <summary>
	/// 	A serialization packet to save one applied ability of an RPG character (AbiltyData) to file
	/// </summary>
	public class AbilityPacket 
	{
		[XmlElement("ID")]
		public string abilityId;
	}
}
