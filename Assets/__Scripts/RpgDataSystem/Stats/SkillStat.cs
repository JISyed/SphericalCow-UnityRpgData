using UnityEngine;
using System.Collections.Generic;

namespace SphericalCow
{
	public class SkillStat : AbstractStat 
	{
		[SerializeField] private List<AbstractStatPercentagePair> statDerivations;
		
		public List<AbstractStatPercentagePair> StatDerivations
		{
			get
			{
				return this.statDerivations;
			}
		}
	}
}
