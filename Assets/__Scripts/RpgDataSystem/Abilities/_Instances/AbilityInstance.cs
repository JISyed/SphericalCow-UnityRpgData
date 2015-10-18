using UnityEngine;
using System.Collections.Generic;

namespace SphericalCow
{
	[System.Serializable]
	public class AbilityInstance 
	{
		private List<AbilityModifierInstance> abilityModifierInstances;
		private string abilityName = "";
		[System.NonSerialized] private Ability abilityRef;
		[System.NonSerialized] private RpgCharacterData character;

		//
		// Constructor (Never runs in deserialization)
		//

		public AbilityInstance(Ability abilityReference, RpgCharacterData characterData)
		{
			this.abilityRef = abilityReference;
			this.character = characterData;
			this.abilityName = abilityRef.AbilityName;

			this.abilityModifierInstances = new List<AbilityModifierInstance>();

			// TODO: Create ability modifier instances!
		}


		//
		// Getters
		//

		public string AbilityName
		{
			get
			{
				return this.abilityName;
			}
		}

		public List<AbilityModifierInstance> AbilityModifierInstances
		{
			get
			{
				return this.abilityModifierInstances;
			}
		}

		public RpgCharacterData Character
		{
			get
			{
				return this.character;
			}
		}
	}
}