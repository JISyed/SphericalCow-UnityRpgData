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
		private const string DefaultName = "Unnamed One";
		private const int DefaultHealthPoints = 100;
		
		[SerializeField] private SaveableGuid id;	// This is the ID of the character itself
		[SerializeField] private string name;
		[SerializeField] private int hp;
		[SerializeField] private int maxHp;
		[SerializeField] private int additionalMaxHp;
		[SerializeField] private int unallocatedSpPool = 0;
		[SerializeField] private XpData xpData;
		[SerializeField] private List<StatData> appliedStats;
		[SerializeField] private List<AbilityData> appliedAbilities;
		
		[System.NonSerialized] private ReadOnlyCollection<StatData> readOnlyStatsList;
		[System.NonSerialized] private ReadOnlyCollection<AbilityData> readOnlyAbilitiesList;
		
		
		/// <summary>
		/// 	Constructor requires an XpProgressor, starting HP, maximum HP, and optionally, a name
		/// </summary>
		public RpgCharacterData(XpProgressor newXpProgressor, int newHP, int newMaxHp, string newName = RpgCharacterData.DefaultName)
		{
			Debug.Assert(newXpProgressor != null, "RpgCharacterData consturctor is being given a null XpProgressor!");
			if(string.IsNullOrEmpty(newName))
			{
				Debug.LogWarning("RpgCharacterData constructor was given a null or empty new name string");
				newName = RpgCharacterData.DefaultName;
			}
			if(newMaxHp <= 0)
			{
				if(newMaxHp != 0)
				{
					newMaxHp = -newMaxHp;
					Debug.LogWarning("A new RPG Character " + newName + " was given negative Max HP. Negating to positive.");
				}
				else
				{
					newMaxHp = RpgCharacterData.DefaultHealthPoints;
					Debug.LogWarning("A new RPG Character " + newName + " was given 0 Max HP. As this is not allowed, the new HP will be " 
					                 + RpgCharacterData.DefaultHealthPoints.ToString());
				}
			}
			if(newHP <= 0)
			{
				if(newHP != 0)
				{
					newHP = -newHP;
					Debug.LogWarning("A new RPG Character " + newName + " was given negative HP. Negating to positive.");
				}
				else
				{
					newHP = newMaxHp;
					Debug.LogWarning("A new RPG Character " + newName + " was given 0 HP. As this is not allowed at start, the new HP will be the same as the given Max HP");
				}
			}
			
			this.id = new SaveableGuid(true);
			this.name = newName;
			this.hp = newHP;
			this.maxHp = newMaxHp;
			this.additionalMaxHp = 0;
			this.xpData = new XpData(newXpProgressor);
			this.appliedAbilities = new List<AbilityData>();
			this.appliedStats = new List<StatData>();
			
			this.UpdateReadOnlyStatsList();
			this.UpdateReadOnlyAbilitiesList();
		}
		
		
		
		
		/// <summary>
		/// 	Adds a certain amount of XP to this Character. This system was designed only for adding XP, not removing.
		/// 	Any negative parameter will be turned positive.
		/// 	Returns true if the newly added XP causes the RPG Character to level up, false otherwise.
		/// 	NOTICE: This system doesn't consider if a character gets too much XP at once and skips a level.
		/// 	A loop that checks the XP and XpToNextLevel must also check for multiple levelups when calling this method.
		/// </summary>
		/// <returns><c>true</c>, if newly added XP causes the RPG Character to level up, <c>false</c> otherwise.</returns>
		/// <param name="xpAmount">The amount of XP to add. Will be turned negated if negative</param>
		public bool AddXp(int xpAmount)
		{
			Debug.Assert(this.xpData != null, "RpgCharacter of name " + this.name + " has no XpData record!");
			return this.xpData.AddXp(xpAmount);
		}
		
		
		
		/// <summary>
		/// 	Set a new increase onto the raw maximum HP that was originally given when this Character was initialized.
		/// 	This additional value is added to the raw MaxHP to get the Character's final MaxHP.
		/// 	This is ideally called by stats that alter health points in the Character.
		/// 	This is not a cumulative increase in MaxHP, but more directly set, for compatibility with stats.
		/// </summary>
		/// <param name="newAdditonalMaxHp">
		/// 	This value is added with the raw MaxHP to get your actual MaxHP,
		/// 	as in "MaxHP = RawMaxHP + AdditionalMaxHP".
		/// 	Negative values will be made positive.
		/// </param>
		public void SetAdditonalMaxHp(int newAdditonalMaxHp)
		{
			// This should remain "=", not "+="; we are not accumulating additionalMaxHp
			this.additionalMaxHp = newAdditonalMaxHp;
			
			if(this.hp > this.MaximumHp)
			{
				this.hp = this.MaximumHp;
			}
		}
		
		
		
		
		/// <summary>
		/// 	Add health points to this Character. Negative values will be made positive.
		/// 	Cannot exceed the maximum HP of this Character.
		/// 	Will return true if prior to calling this method, the Character had 0 HP,
		/// 	as to notify the develeoper that the Character "revived"
		/// </summary>
		/// <returns><c>true</c>, if HP was previously 0, indicating revival of the character, <c>false</c> otherwise.</returns>
		/// <param name="hpAmount">The amount of HP to add. Cannot be negative</param>
		public bool AddHp(int hpAmount)
		{
			if(hpAmount < 0)
			{
				hpAmount = -hpAmount;
			}
			
			bool wasRevived = this.hp == 0 && hpAmount != 0;
			
			this.hp += hpAmount;
			if(this.hp > this.MaximumHp)
			{
				this.hp = this.MaximumHp;
			}
			
			return wasRevived;
		}
		
		
		/// <summary>
		/// 	Removes health points from this Character. Negative values will be made positive.
		/// 	Cannot deplete past 0 HP.
		/// 	Will return true if the Character loses all of the HP once this method is called,
		/// 	as to notify the developer that the Character "died" or "was defeated"
		/// </summary>
		/// <returns><c>true</c>, if hp was removed, <c>false</c> otherwise.</returns>
		/// <param name="hpAmount">Hp amount.</param>
		public bool RemoveHp(int hpAmount)
		{
			if(hpAmount < 0)
			{
				hpAmount = -hpAmount;
			}
			
			this.hp -= hpAmount;
			if(this.hp < 0)
			{
				this.hp = 0;
			}
			
			return this.hp == 0;
		}
		
		
		
		/// <summary>
		/// 	Adds a new Stat to this Character. Also creates an internal record for the stat.
		/// 	Will not add a particular stat more than once.
		/// </summary>
		public void AddStat(AbstractStat newStat)
		{
			// Check if this stat was already added
			StatData statData = this.SearchStat(newStat.Id);
			
			// Add the stat
			if(statData == null)
			{
				
			}
		}
		
		
		
		/// <summary>
		/// 	Search for a stat in this Character by ID. Will return null if no such stat is found
		/// </summary>
		public StatData SearchStat(Guid statId)
		{
			StatData foundStat = null;
			
			foreach(StatData s in this.appliedStats)
			{
				if(s.Id.Equals(statId))
				{
					foundStat = s;
					break;
				}
			}
			
			return foundStat;
		}
		
		
		/// <summary>
		/// 	Search for a stat in this Character by name. Will return null if no such stat is found
		/// </summary>
		public StatData SearchStat(string statName)
		{
			StatData foundStat = null;
			
			foreach(StatData s in this.appliedStats)
			{
				if(s.Name.Equals(statName))
				{
					foundStat = s;
					break;
				}
			}
			
			return foundStat;
		}
		
		
		
		/// <summary>
		/// 	Search for a stat in this Character by a Stat data asset (BaseStat, SecondaryStat, SkillStat). 
		/// 	Will return null if no such stat is found
		/// </summary>
		public StatData SearchStat(AbstractStat statDefinition)
		{
			StatData foundStat = null;
			
			foreach(StatData s in this.appliedStats)
			{
				if(ReferenceEquals(s.StatReference, statDefinition))
				{
					foundStat = s;
					break;
				}
			}
			
			return foundStat;
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
		/// 	The health points (HP) of this Character
		/// </summary>
		public int Hp
		{
			get
			{
				return this.hp;
			}
		}
		
		/// <summary>
		/// 	The maximum HP possible for this Character.
		/// 	Includes additonal increases to the MaxHP caused by stats.
		/// </summary>
		public int MaximumHp
		{
			get
			{
				return this.maxHp + this.additionalMaxHp;
			}
		}
		
		
		/// <summary>
		/// 	The maximum HP originally given to this Character without additional increases given by stats
		/// </summary>
		public int RawMaximumHp
		{
			get
			{
				return this.maxHp;
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
		/// 	The XpProgressor being used by this RPG Character
		/// </summary>
		public XpProgressor XpProgressor
		{
			get
			{
				return this.xpData.XpProgressorReference;
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
