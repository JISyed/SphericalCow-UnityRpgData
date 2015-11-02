using UnityEngine;
using Guid = System.Guid;

namespace SphericalCow
{
	[System.Serializable]
	public class AbilityModifierInstance 
	{
		private string abilityName;
		private AbilityModifierType type;
		private string statName;
		private Guid statInstanceGuid;	// Using GUID for serialize a reference to another serialized object
		private int targetValue;
		private int originalValue;
		[System.NonSerialized] private AbstractStatInstance statRef;
		[System.NonSerialized] private RpgCharacterData character;

		//
		// Constructor (not called during deserialization)
		//

		public AbilityModifierInstance(AbilityModifier abilityModifierRef, RpgCharacterData characterData, AbilityInstance abilityInstance)
		{
			this.abilityName = abilityInstance.AbilityName;
			this.type = abilityModifierRef.Type;
			this.statName = abilityModifierRef.StatToModify.StatName;
			this.statRef = characterData.FindAnyStatInstance(this.statName);
			this.statInstanceGuid = this.statRef.StatGuid;
			this.character = characterData;
			this.targetValue = abilityModifierRef.TargetValue;
			this.originalValue = this.statRef.LocalXpPool;
		}


		//
		// Methods
		//

		/// <summary>
		/// 	Will only procede if the statRef is null
		/// </summary>
		public void RefreshStatReference()
		{
			if(this.statRef == null)
			{
				this.statRef = this.character.FindAnyStatInstance(this.statName);
			}
		}




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

		public Guid StatInstanceGuid
		{
			get
			{
				return this.statInstanceGuid;
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
