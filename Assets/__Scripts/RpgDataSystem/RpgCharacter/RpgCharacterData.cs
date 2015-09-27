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
	}
}
	