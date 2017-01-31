using UnityEngine;
using System.Collections.Generic;

namespace SphericalCow.OldCode
{
	public class Ability : ScriptableObject 
	{
		//
		// Data
		//
		
		[SerializeField] private string abilityName;
		[TextArea(3,12)] [SerializeField] private string description;
		[SerializeField] private Texture icon;
		[SerializeField] private List<AbilityModifier> abilityModifiers;
		
		
		//
		// Methods
		//



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

		public string Description
		{
			get
			{
				return this.description;
			}
		}

		public Texture Icon
		{
			get
			{
				return this.icon;
			}
		}

		public List<AbilityModifier> AbilityModifiers
		{
			get
			{
				return this.abilityModifiers;
			}
		}
		
	}
}
	