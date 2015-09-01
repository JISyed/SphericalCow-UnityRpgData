using UnityEngine;
using System.Collections.Generic;

namespace SphericalCow
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
		
		
	}
}
	