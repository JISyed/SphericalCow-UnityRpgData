﻿using UnityEngine;

namespace SphericalCow
{
	[System.Serializable]
	public class AbilityModifierInstance 
	{
		private string abilityName;
		private AbilityModifierType type;
		private string statName;
		private string statInstanceId;	// Using GUID for serialize a reference to another serialized object
		[System.NonSerialized] private AbstractStatInstance statRef;
		[System.NonSerialized] private RpgCharacterData character;
		private int targetValue;
		private int originalValue;

		//
		// Constructor (not called during deserialization)
		//

		public AbilityModifierInstance(AbilityModifier abilityModifierRef, RpgCharacterData characterData, AbilityInstance abilityInstance)
		{
			this.abilityName = abilityInstance.AbilityName;
			this.type = abilityModifierRef.Type;
			this.statName = abilityModifierRef.StatToModify.StatName;
			this.statRef = characterData.FindAnyStatInstance(this.statName);
			this.statInstanceId = this.statRef.StatId;
			this.character = characterData;
			this.targetValue = abilityModifierRef.TargetValue;
			this.originalValue = this.statRef.LocalXpPool;
		}


		//
		// Methods
		//



		//
		// Setters
		//

		public string AbilityName
		{
			get
			{
				return this.abilityName;
			}
		}

		public AbilityModifierType Type
		{
			get
			{
				return this.type;
			}
		}

		public string StatName
		{
			get
			{
				return this.statName;
			}
		}

		public string StatInstanceId
		{
			get
			{
				return this.statInstanceId;
			}
		}

		public AbstractStatInstance StatReference
		{
			get
			{
				return this.statRef;
			}
		}

		public RpgCharacterData Character
		{
			get
			{
				return this.character;
			}
		}

		public int TargetValue
		{
			get
			{
				return this.targetValue;
			}
		}

		public int OriginalValue
		{
			get
			{
				return this.originalValue;
			}
		}
	}
}
