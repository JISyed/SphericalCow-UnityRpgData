using UnityEngine;

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
		private int targetValue;
		private int originalValue;
	}
}
