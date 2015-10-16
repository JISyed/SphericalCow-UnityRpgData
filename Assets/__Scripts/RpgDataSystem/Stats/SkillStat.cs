using UnityEngine;
using System.Collections.Generic;

namespace SphericalCow
{
	public class SkillStat : AbstractStat 
	{
		//
		// Data
		//

		[SerializeField] private List<AbstractStatPercentagePair> statDerivations;


		//
		// Methods
		//

		public override StatType GetStatType ()
		{
			return StatType.Skill;
		}



		//
		// Getter
		//

		public List<AbstractStatPercentagePair> StatDerivations
		{
			get
			{
				return this.statDerivations;
			}
		}
	}
}
