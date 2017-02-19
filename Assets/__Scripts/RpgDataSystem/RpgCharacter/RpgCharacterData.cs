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
		[SerializeField] private int unallocatedSpPool;		// Global SP pool used with Use-Assigned system
		[SerializeField] private XpData xpData;
		[SerializeField] private List<StatData> appliedStats;
		[SerializeField] private List<AbilityData> appliedAbilities;
		[SerializeField] private int numOfStats;
		[SerializeField] private int numOfAbilities;
		[SerializeField] private GlobalSpAssignmentType assignmentType;
		
		[System.NonSerialized] private ReadOnlyCollection<StatData> readOnlyStatsList;
		[System.NonSerialized] private ReadOnlyCollection<AbilityData> readOnlyAbilitiesList;
		
		
		/// <summary>
		/// 	Constructor requires an XpProgressor, starting HP, maximum HP, and optionally, a name
		/// </summary>
		public RpgCharacterData(XpProgressor newXpProgressor, 
		                        int newHP, 
		                        int newMaxHp, 
		                        GlobalSpAssignmentType typeOfSpAssignment, 
		                        string newName = RpgCharacterData.DefaultName)
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
			this.unallocatedSpPool = 0;
			this.assignmentType = typeOfSpAssignment;
			
			this.xpData = new XpData(newXpProgressor);
			this.appliedAbilities = new List<AbilityData>();
			this.appliedStats = new List<StatData>();
			
			this.numOfStats = 0;
			this.numOfAbilities = 0;
			
			
			this.UpdateReadOnlyStatsList();
			this.UpdateReadOnlyAbilitiesList();
		}
		
		
		
		/// <summary>
		/// 	Deserialization Constructor
		/// </summary>
		public RpgCharacterData(RpgCharacterPacket rpgCharacterPacket)
		{
			Debug.Assert(rpgCharacterPacket != null, "RpgCharacterData could not be deserialized because the packet was null!");
			
			this.id = new SaveableGuid(rpgCharacterPacket.id);
			this.name = rpgCharacterPacket.name;
			this.hp = rpgCharacterPacket.hp;
			this.maxHp = rpgCharacterPacket.maxHp;
			this.additionalMaxHp = rpgCharacterPacket.additionalMaxHp;
			this.unallocatedSpPool = rpgCharacterPacket.unallocatedSpPool;
			this.assignmentType = rpgCharacterPacket.assignmentType;
			
			this.xpData = new XpData(rpgCharacterPacket.xpDataPacket);
			this.appliedStats = new List<StatData>();
			this.appliedAbilities = new List<AbilityData>();
			
			this.numOfStats = rpgCharacterPacket.appliedStats.Count;
			this.numOfAbilities = rpgCharacterPacket.appliedAbilities.Count;
			
			
			// Deserialize abilties first, so we can just loop through stats later to apply abilities
			foreach(AbilityPacket abilityPacket in rpgCharacterPacket.appliedAbilities)
			{
				AbilityData newAbility = new AbilityData(abilityPacket);
				this.appliedAbilities.Add(newAbility);
			}
			
			foreach(StatPacket statPacket in rpgCharacterPacket.appliedStats)
			{
				StatData newStat = new StatData(statPacket, this.appliedStats);
				this.appliedStats.Add(newStat);
				
				// Apply all abilities that this new stat takes
				foreach(AbilityData ability in this.appliedAbilities)
				{
					newStat.ApplyOneAbility(ability);
				}
				
			}
			
			this.RecalculateAllLinkedStatsPools();
			
			this.UpdateReadOnlyStatsList();
			this.UpdateReadOnlyAbilitiesList();
		}
		
		
		
		
		
		//
		// Experience Points (XP)
		// 
		
		
		
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
			
			bool didLevelUp = this.xpData.AddXp(xpAmount);
			
			if(didLevelUp)
			{				
				if(this.StatPointAssignmentType == GlobalSpAssignmentType.PointAssigned)
				{
					this.UpgradeStatPointsByPointAllocation();
				}
				else if(this.StatPointAssignmentType == GlobalSpAssignmentType.UseAssigned)
				{
					this.UpgradeStatPointsByUseAllocation();
				}
				else
				{
					Debug.LogError("Someone added a new type of SP assignment method. See GlobalSpAssignmentType");
				}
			}
			
			return didLevelUp;
		}
		
		
		
		
		
		
		//
		// Health Points (HP)
		//
		
		
		
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
		
		
		
		
		
		
		
		
		//
		// Stats
		//
		
		
		
		/// <summary>
		/// 	Adds a new Stat to this Character. Also creates an internal record for the stat.
		/// 	Will not add a particular stat more than once.
		/// </summary>
		public void AddStat(AbstractStat newStat)
		{
			// Check if this stat was already added
			StatData statData = this.SearchStat(newStat);
			
			// Add the stat
			if(statData == null)
			{
				statData = new StatData(newStat, this.appliedStats);
				this.appliedStats.Add(statData);
				
				// Apply all abilities that this new stat takes
				foreach(AbilityData ability in this.appliedAbilities)
				{
					statData.ApplyOneAbility(ability);
				}
				
				
				this.RecalculateAllLinkedStatsPools();
				
				
				
				this.UpdateReadOnlyStatsList();
			}
			else
			{
				if(Debug.isDebugBuild)
				{
					Debug.LogWarning("The stat " + newStat.Name + " was already added to " + this.Name);
				}
			}
		}
		
		
		/// <summary>
		/// 	Remove a stat from this Character by name
		/// </summary>
		public void RemoveStat(string oldStatName)
		{
			// Check if this stat exists in the Character
			StatData statData = this.SearchStat(oldStatName);
			
			if(statData != null)
			{
				this.UnlinkAndRemoveStat(statData);
			}
			else
			{
				if(Debug.isDebugBuild)
				{
					Debug.LogWarning("The stat \"" + oldStatName + "\" was already removed from " + this.Name);
				}
			}
			
		}
		
		
		/// <summary>
		/// 	Remove a stat from this Character by ID
		/// </summary>
		public void RemoveStat(Guid oldStatId)
		{
			// Check if this stat exists in the Character
			StatData statData = this.SearchStat(oldStatId);
			
			if(statData != null)
			{
				this.UnlinkAndRemoveStat(statData);
			}
			else
			{
				if(Debug.isDebugBuild)
				{
					Debug.LogWarning("The stat of ID \"" + oldStatId.ToString() + "\" was already removed from " + this.Name);
				}
			}
			
		}
		
		
		
		/// <summary>
		/// 	Search for a stat in this Character by ID. Will return null if no such stat is found
		/// </summary>
		public StatData SearchStat(Guid statId)
		{
			if(statId.Equals(Guid.Empty))
			{
				Debug.LogWarning("RpgCharacterData.SearchStat(Guid) was given an empty GUID!");
				return null;
			}
			
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
			if(string.IsNullOrEmpty(statName))
			{
				Debug.LogWarning("RpgCharacterData.SearchStat(string) was given a null or empty string!");
				return null;
			}
			
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
			if(statDefinition == null)
			{
				Debug.LogWarning("RpgCharacterData.SearchStat(AbstractStat) was given a null AbstractStat!");
				return null;
			}
			
			StatData foundStat = null;
			
			foreach(StatData s in this.appliedStats)
			{
				if(s.StatReference.Id.Equals(statDefinition.Id))
				{
					foundStat = s;
					break;
				}
			}
			
			return foundStat;
		}
		
		
		
		
		/// <summary>
		/// 	Takes a certain amount of SP out of the Character's unallocated pool 
		/// 	and puts it into the pool of the given stat.
		/// </summary>
		/// <returns><c>true</c>, if the given stat exists in this Character <c>false</c> otherwise.</returns>
		/// <param name="spToAdd">The amount of SP to add to the given stat and remove from the unallocated pool</param>
		/// <param name="statName">The name of the stat</param>
		public bool AddStatPointsFromUnallocatedPool(int spToAdd, string statName)
		{
			StatData foundStat = this.SearchStat(statName);
			
			if(foundStat != null)
			{
				this.PrivAddSpFromUnallocPool(spToAdd, foundStat);
				return true;
			}
			
			return false;
		}
		
		
		/// <summary>
		/// 	Takes a certain amount of SP out of the Character's unallocated pool 
		/// 	and puts it into the pool of the given stat.
		/// </summary>
		/// <returns><c>true</c>, if the given stat exists in this Character <c>false</c> otherwise.</returns>
		/// <param name="spToAdd">The amount of SP to add to the given stat and remove from the unallocated pool</param>
		/// <param name="statId">The ID of the stat</param>
		public bool AddStatPointsFromUnallocatedPool(int spToAdd, Guid statId)
		{
			StatData foundStat = this.SearchStat(statId);
			
			if(foundStat != null)
			{
				this.PrivAddSpFromUnallocPool(spToAdd, foundStat);
				return true;
			}
			
			return false;
		}
		
		
		/// <summary>
		/// 	Takes a certain amount of SP out of the Character's unallocated pool 
		/// 	and puts it into the pool of the given stat.
		/// </summary>
		/// <returns><c>true</c>, if the given stat exists in this Character <c>false</c> otherwise.</returns>
		/// <param name="spToAdd">The amount of SP to add to the given stat and remove from the unallocated pool</param>
		/// <param name="statDefinition">The definition file for the stat</param>
		public bool AddStatPointsFromUnallocatedPool(int spToAdd, AbstractStat statDefinition)
		{
			StatData foundStat = this.SearchStat(statDefinition);
			
			if(foundStat != null)
			{
				this.PrivAddSpFromUnallocPool(spToAdd, foundStat);
				return true;
			}
			
			return false;
		}
		
		
		
		/// <summary>
		/// 	Tells the system that the given stat was used. This is needed for UseAssigned SP calculations.
		/// 	Call this whenever that given stat was used in your game logic. Call as many times as needed.
		/// 	Will return false if the given stat was not found in this Character.
		/// </summary>
		/// <returns><c>true</c>, if the given stat exists in this Character <c>false</c> otherwise.</returns>
		/// <param name="statName">The name of the stat</param>
		public bool MarkStatAsUsed(string statName)
		{
			StatData foundStat = this.SearchStat(statName);
			
			if(foundStat != null)
			{
				foundStat.MarkStatAsUsed();
				return true;
			}
			
			return false;
		}
		
		
		/// <summary>
		/// 	Tells the system that the given stat was used. This is needed for UseAssigned SP calculations.
		/// 	Call this whenever that given stat was used in your game logic. Call as many times as needed.
		/// 	Will return false if the given stat was not found in this Character.
		/// </summary>
		/// <returns><c>true</c>, if the given stat exists in this Character <c>false</c> otherwise.</returns>
		/// <param name="statId">The ID of the stat</param>
		public bool MarkStatAsUsed(Guid statId)
		{
			StatData foundStat = this.SearchStat(statId);
			
			if(foundStat != null)
			{
				foundStat.MarkStatAsUsed();
				return true;
			}
			
			return false;
		}
		
		
		/// <summary>
		/// 	Tells the system that the given stat was used. This is needed for UseAssigned SP calculations.
		/// 	Call this whenever that given stat was used in your game logic. Call as many times as needed.
		/// 	Will return false if the given stat was not found in this Character.
		/// </summary>
		/// <returns><c>true</c>, if the given stat exists in this Character <c>false</c> otherwise.</returns>
		/// <param name="statDefinition">The definition file of the stat</param>
		public bool MarkStatAsUsed(AbstractStat statDefinition)
		{
			StatData foundStat = this.SearchStat(statDefinition);
			
			if(foundStat != null)
			{
				foundStat.MarkStatAsUsed();
				return true;
			}
			
			return false;
		}
		
		
		/// <summary>
		/// 	Loops through all applied stats of this Character and recalculates derived SP
		/// 	for each stat's linked stats
		/// </summary>
		public void RecalculateAllLinkedStatsPools()
		{
			foreach(StatData stat in this.appliedStats)
			{
				stat.RecalculateLinkedStatPoints();
			}
		}
		
		
		
		
		
		
		
		
		//
		// Abilities
		//
		
		

		/// <summary>
		/// 	Adds a new Aiblity to this Character. Also creates an internal record for the ability.
		/// 	Will not add a particular ability more than once.
		/// </summary>
		public void AddAbility(Ability newAbility)
		{
			// Check if this stat was already added
			AbilityData abilityData = this.SearchAbility(newAbility);
			
			// Add the stat
			if(abilityData == null)
			{
				abilityData = new AbilityData(newAbility);
				this.appliedAbilities.Add(abilityData);
				
				// Apply this ability on all stats that take this ability
				foreach(StatData stat in this.appliedStats)
				{
					stat.ApplyOneAbility(abilityData);
					stat.RecalculateLinkedStatPoints();
				}
				
				this.UpdateReadOnlyAbilitiesList();
			}
			else
			{
				if(Debug.isDebugBuild)
				{
					Debug.LogWarning("The ability " + newAbility.Name + " was already added to " + this.Name);
				}
			}
		}
		
		
		/// <summary>
		/// 	Remove an ability from this Character by name
		/// </summary>
		public void RemoveAbility(string oldAbilityName)
		{
			// Check if this ability exists in the Character
			AbilityData abilityData = this.SearchAbility(oldAbilityName);
			
			if(abilityData != null)
			{
				this.UnlinkAndRemoveAbility(abilityData);
			}
			else
			{
				if(Debug.isDebugBuild)
				{
					Debug.LogWarning("The ability \"" + oldAbilityName + "\" was already removed from " + this.Name);
				}
			}
		}
		
		
		/// <summary>
		/// 	Remove an ability from this Character by ID
		/// </summary>
		public void RemoveAbility(Guid oldAbilityId)
		{
			// Check if this ability exists in the Character
			AbilityData abilityData = this.SearchAbility(oldAbilityId);
			
			if(abilityData != null)
			{
				this.UnlinkAndRemoveAbility(abilityData);
			}
			else
			{
				if(Debug.isDebugBuild)
				{
					Debug.LogWarning("The ability of ID \"" + oldAbilityId.ToString() + "\" was already removed from " + this.Name);
				}
			}
		}
		
		
		/// <summary>
		/// 	Search for an ability in this Character by an Ability data asset. 
		/// 	Will return null if no such ability is found
		/// </summary>
		public AbilityData SearchAbility(Ability abilityDefinition)
		{
			if(abilityDefinition == null)
			{
				Debug.LogWarning("RpgCharacterData.Ability(Ability) was given a null Ability!");
				return null;
			}
			
			AbilityData foundAbility = null;
			
			foreach(AbilityData ability in this.appliedAbilities)
			{
				if(ability.AbilityReference.Id.Equals(abilityDefinition.Id))
				{
					foundAbility = ability;
					break;
				}
			}
			
			return foundAbility;
		}
		
		
		/// <summary>
		/// 	Search for an ability in this Character by the Ability's name. 
		/// 	Will return null if no such ability is found
		/// </summary>
		public AbilityData SearchAbility(string abilityName)
		{
			if(string.IsNullOrEmpty(abilityName))
			{
				Debug.LogWarning("RpgCharacterData.Ability(string) was given a null or empty string!");
				return null;
			}
			
			AbilityData foundAbility = null;
			
			foreach(AbilityData ability in this.appliedAbilities)
			{
				if(ability.AbilityReference.Name.Equals(abilityName))
				{
					foundAbility = ability;
					break;
				}
			}
			
			return foundAbility;
		}
		
		
		/// <summary>
		/// 	Search for an ability in this Character by the Ability's ID. 
		/// 	Will return null if no such ability is found
		/// </summary>
		public AbilityData SearchAbility(Guid abilityId)
		{
			if(abilityId.Equals(Guid.Empty))
			{
				Debug.LogWarning("RpgCharacterData.Ability(Guid) was given an empty GUID!");
				return null;
			}
			
			AbilityData foundAbility = null;
			
			foreach(AbilityData ability in this.appliedAbilities)
			{
				if(ability.Id.Equals(abilityId))
				{
					foundAbility = ability;
					break;
				}
			}
			
			return foundAbility;
		}
		
		
		
		
		
		
		//
		// Serilaization
		//
		
		
		/// <summary>
		/// 	Only to be called by the RpgCharacterSerializer
		/// </summary>
		public RpgCharacterPacket ExportSerializationPacket()
		{
			RpgCharacterPacket newPacket = new RpgCharacterPacket();
			
			newPacket.id = this.id.GuidString;
			newPacket.name = this.name;
			newPacket.hp = this.hp;
			newPacket.maxHp = this.maxHp;
			newPacket.additionalMaxHp = this.additionalMaxHp;
			newPacket.unallocatedSpPool = this.unallocatedSpPool;
			newPacket.assignmentType = this.assignmentType;
			
			newPacket.xpDataPacket = this.xpData.ExportSerializationPacket();
			
			foreach(StatData stat in this.appliedStats)
			{
				StatPacket newStatPacket = stat.ExportSerializationPacket();
				newPacket.appliedStats.Add(newStatPacket);
			}
			
			foreach(AbilityData ability in this.appliedAbilities)
			{
				AbilityPacket newAbilityPacket = ability.ExportSerializationPacket();
				newPacket.appliedAbilities.Add(newAbilityPacket);
			}
			
			return newPacket;
		}
		
		
		
		
		
		
		//
		// Properties
		//
		
		
		
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
		/// 	The current number of applied stats on this Character
		/// </summary>
		public int NumberOfAppliedStats
		{
			get
			{
				return this.numOfStats;
			}
		}
		
		
		
		/// <summary>
		/// 	The current number of applied abilities on this Character
		/// </summary>
		public int NumberOfAppliedAbilities
		{
			get
			{
				return this.numOfAbilities;
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
		/// 	The method in which the Character allocates SP when leveling up (either UseAssigned or PointAssigned)
		/// </summary>
		public GlobalSpAssignmentType StatPointAssignmentType
		{
			get
			{
				return this.assignmentType;
			}
		}
		
		
		
		
		
		
		
		
		//
		// Private Methods
		//
		
		
		
		/// <summary>
		/// 	Permanently adds SP to the given StatData instance and removes the same amount from the global pool
		///	 	Warning: Will not check for null parameters!
		/// </summary>
		/// <param name="spToAdd">Sp to add.</param>
		/// <param name="stat">Stat.</param>
		private void PrivAddSpFromUnallocPool(int spToAdd, StatData stat)
		{
			if(spToAdd < 0)
			{
				spToAdd = -spToAdd;
			}
			
			int remainingUnallocatedSp = this.UnallocatedSp - spToAdd;
			
			if(remainingUnallocatedSp < 0)
			{
				spToAdd = spToAdd + remainingUnallocatedSp;
				
				// You can uncomment out this log if you want
				//Debug.Log("Unallocated Pool has ran out");
			}
			
			
			this.PrivAddSpToStatData(spToAdd, stat);
			this.unallocatedSpPool -= spToAdd;
		}
		
		
		
		
		/// <summary>
		/// 	Permanently adds SP to the given StatData instance.
		/// 	Warning: Will not check for null parameters!
		/// </summary>
		/// <param name="spToAdd">SP to add to the given stat instance</param>
		/// <param name="stat">The stat instance to increment</param>
		private void PrivAddSpToStatData(int spToAdd, StatData stat)
		{
			stat.AddStatPointsToRawPool(spToAdd);
			this.RecalculateAllLinkedStatsPools();
		}
		
		
		
		
		
		
		/// <summary>
		/// 	Increases SP in a global pool that the player can later use to allocate into certain stats
		/// </summary>
		private void UpgradeStatPointsByPointAllocation()
		{
			// Constant used to modulate the relationship between number of stats and amount of new SP on levelup
			float k = RpgDataRegistry.Instance.PointToStatRatio;
			int m = RpgDataRegistry.Instance.PointAssignedMultiplier;
			
			
			// In point assigned, the number of stats you currently have becomes the basis for how many
			// SP you gain when you level up. This is multiplied by a constant k.
			// For example, if k==1 and you have 4 applied stats, you should get 4 SP added into the global pool on levelup.
			// Players can them manually allocate those 4 SP into each stat evenly, or all into one stat if they wanted to.
			
			this.unallocatedSpPool += ((int) Mathf.Round(k * (float) this.NumberOfAppliedStats)) * m;
		}
		
		
		/// <summary>
		/// 	Increases SP in all currently applied stats based off frequency of use (useFactor)
		/// </summary>
		private void UpgradeStatPointsByUseAllocation()
		{	
			float useFactorRatio = RpgDataRegistry.Instance.UseFactorRatio;
			int useAllocMultiplier = RpgDataRegistry.Instance.UseAssignedMultiplier;
			
			foreach(StatData stat in this.appliedStats)
			{
				// Calculate a multiplier to impose onto a UseFactor based off a square root function
				// Where: UseFactor = f, useFactorRatio = r, and factorMultiplier = m :
				//        m = sqrt(f*r)
				// Where r is a constant and f is always different
				// Test this function on Demos online calculator
				float factorMultiplier = Mathf.Sqrt(useFactorRatio * (float) stat.UseFactor);
				
				// Use the factorMultiplier and a developer-defined multiplier to calculate the newly added SP
				int spIncrease = useAllocMultiplier * (int) Mathf.Round(factorMultiplier);
				
				// Increment the stat
				this.PrivAddSpToStatData(spIncrease, stat);
			}
			
			
		}
		
		
		
		
		/// <summary>
		/// 	Recreates the read-only list of applied stats. Calling this too often may invoke the GarbageCollector
		/// </summary>
		private void UpdateReadOnlyStatsList()
		{
			this.readOnlyStatsList = this.appliedStats.AsReadOnly();
			this.numOfStats = this.appliedStats.Count;
		}
		
		
		/// <summary>
		/// 	Recreates the read-only list of applied abilities. Calling this too often may invoke the GarbageCollector
		/// </summary>
		private void UpdateReadOnlyAbilitiesList()
		{
			this.readOnlyAbilitiesList = this.appliedAbilities.AsReadOnly();
			this.numOfAbilities = this.appliedAbilities.Count;
		}
		 
		
		
		
		/// <summary>
		/// 	Unlinks ability modifiers in the stats and removes the relevant ability
		/// </summary>
		private void UnlinkAndRemoveAbility(AbilityData oldAbility)
		{
			// Unlink this ability from all stats that take this ability
			foreach(StatData stat in this.appliedStats)
			{
				stat.RemoveOneAbility(oldAbility);
				stat.RecalculateLinkedStatPoints();
			}
			
			this.appliedAbilities.Remove(oldAbility);
			this.UpdateReadOnlyAbilitiesList();
			
			this.RecalculateAllLinkedStatsPools();
		}
		
		
		
		/// <summary>
		/// 	Unlinks stats in the SP derivers and removes the relevant stat
		/// </summary>
		private void UnlinkAndRemoveStat(StatData oldStat)
		{
			// Unapply all abilities that this old stat took
			foreach(AbilityData ability in this.appliedAbilities)
			{
				oldStat.RemoveOneAbility(ability);
			}
			
			this.appliedStats.Remove(oldStat);
			this.UpdateReadOnlyStatsList();
			
			this.RecalculateAllLinkedStatsPools();
		}
		
		
	}
}
