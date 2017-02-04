using UnityEngine;

namespace SphericalCow.OldCode
{
	[System.Serializable]
	public class AbilityModifier 
	{
		[SerializeField] private OldAbstractStat statToModify;
		[SerializeField] private AbilityModifierType type;
		[SerializeField] private int targetValue;

		//
		// Constructor
		//

		AbilityModifier()
		{
		}


		//
		// Methods
		//



		//
		// Getters
		//

		public OldAbstractStat StatToModify
		{
			get
			{
				return this.statToModify;
			}
		}

		public AbilityModifierType Type
		{
			get
			{
				return this.type;
			}
		}

		public int TargetValue
		{
			get
			{
				return this.targetValue;
			}
		}


	}
}
