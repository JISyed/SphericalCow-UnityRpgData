using UnityEngine;
using System.Collections.Generic;

namespace SphericalCow.OldCode
{
	public class SkillStat : OldAbstractStat 
	{
		//
		// Data
		//

		[SerializeField] private List<AbstractStatPercentagePair> statDerivations;


		//
		// Methods
		//

		public override SphericalCow.OldCode.StatType GetStatType ()
		{
			return SphericalCow.OldCode.StatType.Skill;
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
