using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Guid = System.Guid;

namespace SphericalCow
{
	/// <summary>
	/// 	Data that represents a role-playing character in the RPG system (can be player or NPC)
	/// </summary>
	[System.Serializable]
	public class RpgCharacterData
	{
		[SerializeField] private SaveableGuid id;	// This is the ID of the character itself
		[SerializeField] private string name;
		[SerializeField] private int unallocatedSpPool = 0;
		[SerializeField] private XpData xpData;
		[SerializeField] private List<StatData> appliedStats;
		[SerializeField] private List<AbilityData> appliedAbilities;
		
		[System.NonSerialized] private ReadOnlyCollection<StatData> readOnlyStatsList;
		[System.NonSerialized] private ReadOnlyCollection<AbilityData> readOnlyAbilitiesList;
		
		
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
			
			this.UpdateReadOnlyStatsList();
			this.UpdateReadOnlyAbilitiesList();
		}
		
		
		
		
		/// <summary>
		/// 	The ID of this RPG Character
		/// </summary>
		public Guid Id
		{
			get
			{
				return this.id.GuidData;
			}
		}
		
		
		/// <summary>
		/// 	The name of this RPG Character
		/// </summary>
		public string Name
		{
			get
			{
				return this.name;
			}
		}
		
		/// <summary>
		/// 	The stat points (SP) of this Character that are *not* allocated onto any stats
		/// </summary>
		public int UnallocatedSp
		{
			get
			{
				return this.unallocatedSpPool;
			}
		}
		
		
		/// <summary>
		/// 	The experience points (XP) of this Character
		/// </summary>
		public int Xp
		{
			get
			{
				return this.xpData.XP;
			}
		}
		
		
		/// <summary>
		/// 	The total accumulated amount of XP needed for this Character before leveling up to a new level
		/// </summary>
		public int XpToNextLevel
		{
			get
			{
				return this.xpData.XpToNextLevel;
			}
		}
		
		
		/// <summary>
		/// 	The current Level of this Character
		/// </summary>
		public int Level
		{
			get
			{
				return this.xpData.Level;
			}
		}
		
		
		
		
		/// <summary>
		/// 	A list of all stats applied onto this Character
		/// </summary>
		public ReadOnlyCollection<StatData> AppliedStats
		{
			get
			{
				return this.readOnlyStatsList;
			}
		}
		
		
		/// <summary>
		/// 	A list of all abilities applied onto this Character
		/// </summary>
		public ReadOnlyCollection<AbilityData> AppliedAbilities
		{
			get
			{
				return this.readOnlyAbilitiesList;
			}
		}
		
		
		
		/// <summary>
		/// 	Recreates the read-only list of applied stats. Calling this too often may invoke the GarbageCollector
		/// </summary>
		private void UpdateReadOnlyStatsList()
		{
			this.readOnlyStatsList = this.appliedStats.AsReadOnly();
		}
		
		
		/// <summary>
		/// 	Recreates the read-only list of applied abilities. Calling this too often may invoke the GarbageCollector
		/// </summary>
		private void UpdateReadOnlyAbilitiesList()
		{
			this.readOnlyAbilitiesList = this.appliedAbilities.AsReadOnly();
		}
		 
		
	}
}
