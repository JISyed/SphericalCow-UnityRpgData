using UnityEngine;
using System.Collections.Generic;

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
		
		private ProgressionType progressionVariable;
		private string characterName ;//= "Player";
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
			this.progressionVariable = ProgressionType.Easy;
			this.characterName = "Default Player Name";
			this.basicStats = new List<BasicStatInstance>();
			this.secondaryStats = new List<SecondaryStatInstance>();
			this.skillStats = new List<SkillStatInstance>();
			this.abilities = new List<AbilityInstance>();
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


		//
		// Getters
		//

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

		public List<BasicStatInstance> ListOfBasicStats
		{
			get
			{
				return this.basicStats;
			}
		}

		public List<SecondaryStatInstance> ListOfSecondaryStats
		{
			get
			{
				return this.secondaryStats;
			}
		}

		public List<SkillStatInstance> ListOfSkillStats
		{
			get
			{
				return this.skillStats;
			}
		}

		public List<AbilityInstance> ListOfAbilties
		{
			get
			{
				return this.abilities;
			}
		}




	}
}
	