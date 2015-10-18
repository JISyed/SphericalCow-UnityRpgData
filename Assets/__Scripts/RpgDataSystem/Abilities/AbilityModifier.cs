using UnityEngine;

namespace SphericalCow
{
	[System.Serializable]
	public class AbilityModifier 
	{
		[SerializeField] private AbstractStat statToModify;
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

		public AbstractStat StatToModify
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
