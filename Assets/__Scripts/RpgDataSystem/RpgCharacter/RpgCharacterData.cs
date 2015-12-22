using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;	// for ReadOnlyCollection<>

namespace SphericalCow
{
	/// <summary>
	/// 	Contains all the relevant RPG data about one character
	/// </summary>
	[System.Serializable]
	public class RpgCharacterData 
	{
		//
		// Data
		//
		
		private int xp;										// Experience points of the player (not the same thing as stat points!)
		private int levelUpXpThreshold;						// The amount of XP needed to level up
		private int pointAssignedSpPool;					// "Global" pool of stat points that accumulates upon leveling up (for Point-Assigned only)
		private ProgressionType progressionVariable;
		private string characterName;
		private List<BasicStatInstance> basicStats;
		private List<SecondaryStatInstance> secondaryStats;
		private List<SkillStatInstance> skillStats;
		private List<AbilityInstance> abilities;

		
		//
		// Methods
		//


		/// <summary>
		/// 	Constructor. Will not be called upon deserialization
		/// </summary>
		public RpgCharacterData()
		{
			this.xp = 0;
			this.levelUpXpThreshold = 100; 			// TODO: Find an intelligent way to calculate this
			this.pointAssignedSpPool = 0;
			this.progressionVariable = ProgressionType.Easy;
			this.characterName = "Player";
			this.basicStats = new List<BasicStatInstance>();
			this.secondaryStats = new List<SecondaryStatInstance>();
			this.skillStats = new List<SkillStatInstance>();
			this.abilities = new List<AbilityInstance>();
		}


		//
		// Methods
		//

		public BasicStatInstance FindBasicStatInstance(string nameOfStat)
		{
			BasicStatInstance inquired = null;
			foreach(var stat in this.basicStats)
			{
				if(stat.StatName.Equals(nameOfStat))
				{
					// Found
					inquired = stat;
					break;
				}
			}

			return inquired;
		}

		public SecondaryStatInstance FindSecondaryStatInstance(string nameOfStat)
		{
			SecondaryStatInstance inquired = null;
			foreach(var stat in this.secondaryStats)
			{
				if(stat.StatName.Equals(nameOfStat))
				{
					// Found
					inquired = stat;
					break;
				}
			}
			
			return inquired;
		}

		public SkillStatInstance FindSkillStatInstance(string nameOfStat)
		{
			SkillStatInstance inquired = null;
			foreach(var stat in this.skillStats)
			{
				if(stat.StatName.Equals(nameOfStat))
				{
					// Found
					inquired = stat;
					break;
				}
			}
			
			return inquired;
		}

		public AbilityInstance FindAbilityInstance(string nameOfAbility)
		{
			AbilityInstance inquired = null;
			foreach(var ability in this.abilities)
			{
				if(ability.AbilityName.Equals(nameOfAbility))
				{
					// Found
					inquired = ability;
					break;
				}
			}
			
			return inquired;
		}

		public AbstractStatInstance FindAnyStatInstance(string nameOfStat)
		{
			AbstractStatInstance foundStat = null;

			// Search Base stats
			foundStat = this.FindBasicStatInstance(nameOfStat);
			if(foundStat == null)
			{
				// Search Secondary stats
				foundStat = this.FindSecondaryStatInstance(nameOfStat);
				if(foundStat == null)
				{
					// Search Skill stats
					foundStat = this.FindSkillStatInstance(nameOfStat);
				}
			}

			return foundStat;
		}

		public void AddBasicStat(BasicStat statReference)
		{
			this.basicStats.Add(new BasicStatInstance(statReference, this));
			this.RefreshDerivativeStatAssociations();
		}

		public void AddSecondaryStat(SecondaryStat statReference)
		{
			this.secondaryStats.Add(new SecondaryStatInstance(statReference, this));
			this.RefreshDerivativeStatAssociations();
		}

		public void AddSkillStat(SkillStat statReference)
		{
			this.skillStats.Add(new SkillStatInstance(statReference, this));
			this.RefreshDerivativeStatAssociations();
		}

		public void AddAbility(Ability abilityReference)
		{
			AbilityInstance newAbilityInst = new AbilityInstance(abilityReference, this);
			this.abilities.Add(newAbilityInst);
			newAbilityInst.ApplyAbility();
		}



		private void RefreshDerivativeStatAssociations()
		{
			// For all secondary stats
			foreach(var secondaryStat in this.secondaryStats)
			{
				secondaryStat.SetupBasicStatInstanceAssociations();
			}

			// For all skill stats
			foreach(var skillStat in this.skillStats)
			{
				skillStat.SetupStatInstanceAssociations();
			}
		}



		//
		// Setters
		//

		public void SetCharacterName(string newCharacterName)
		{
			this.characterName = newCharacterName;
		}

		public void SetProgressionVariable(ProgressionType newProgressionVariable)
		{
			this.progressionVariable = newProgressionVariable;
		}
		
		public void SetXpManually(int newXp)
		{
			this.xp = newXp;
		}
		
		public void SetLevelUpXpThresholdManually(int newThreshold)
		{
			this.levelUpXpThreshold = newThreshold;
		}
		
		
		
		
		//
		// Getters
		//
		
		public int Xp
		{
			get
			{
				return this.xp;
			}
		}
		
		public int XpNeededToLevelUp
		{
			get
			{
				return this.levelUpXpThreshold;
			}
		}
		
		public int PointAssignedSpPool
		{
			get
			{
				return this.pointAssignedSpPool;
			}
		}

		public string CharacterName
		{
			get
			{
				return this.characterName;
			}
		}

		public ProgressionType ProgressionVariable
		{
			get
			{
				return this.progressionVariable;
			}
		}

		/// <summary>
		/// 	Returns a read-only list of basic stats for this character
		/// </summary>
		public ReadOnlyCollection<BasicStatInstance> ListOfBasicStats
		{
			get
			{
				return this.basicStats.AsReadOnly();
			}
		}

		/// <summary>
		/// 	Returns a read-only list of secondary stats for this character
		/// </summary>
		public ReadOnlyCollection<SecondaryStatInstance> ListOfSecondaryStats
		{
			get
			{
				return this.secondaryStats.AsReadOnly();
			}
		}

		/// <summary>
		/// 	Returns a read-only list of skill stats for this character
		/// </summary>
		public ReadOnlyCollection<SkillStatInstance> ListOfSkillStats
		{
			get
			{
				return this.skillStats.AsReadOnly();
			}
		}

		/// <summary>
		/// 	Returns a read-only list of abilities for this character
		/// </summary>
		public ReadOnlyCollection<AbilityInstance> ListOfAbilties
		{
			get
			{
				return this.abilities.AsReadOnly();
			}
		}




	}
}
	