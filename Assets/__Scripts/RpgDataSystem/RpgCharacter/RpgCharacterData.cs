using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Guid = System.Guid;

namespace SphericalCow
{
	/// <summary>
	/// 	Data that represents a role-playing character in the RPG system
	/// </summary>
	[System.Serializable]
	public class RpgCharacterData
	{
		[SerializeField] private SaveableGuid id;
		[SerializeField] private string name;
		[SerializeField] private int unassignedSpPool = 0;
		[SerializeField] private XpData xpData;
		[SerializeField] private List<StatData> appliedStats;
		[SerializeField] private List<AbilityData> appliedAbilities;
		
		
		/// <summary>
		/// 	Constructor requires an XpProgressor
		/// </summary>
		public RpgCharacterData(XpProgressor newXpProgressor)
		{
			this.id = new SaveableGuid(true);
			this.name = "Unnamed";
			this.xpData = new XpData(newXpProgressor);
			this.appliedAbilities = new List<AbilityData>();
			this.appliedStats = new List<StatData>();
		}
		
		
		
	}
}
