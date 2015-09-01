using UnityEngine;

namespace SphericalCow
{
	[System.Serializable]
	public class AbilityModifier 
	{
		[SerializeField] private AbstractStat statToModify;
		[SerializeField] private AbilityModifierType type;
		[SerializeField] private int targetValue;
		
		AbilityModifier()
		{
		}
		
	}
}
