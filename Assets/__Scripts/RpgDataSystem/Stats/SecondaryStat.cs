using UnityEngine;
using System.Collections.Generic;

namespace SphericalCow
{
	public class SecondaryStat : AbstractStat 
	{
		[SerializeField] private List<BasicStatPercentagePair> baseStatDerivations;
		
		public List<BasicStatPercentagePair> BaseStatDerivations
		{
			get
			{
				return this.baseStatDerivations;
			}
		}
	}
}