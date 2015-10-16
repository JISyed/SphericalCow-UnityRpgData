using UnityEngine;
using System.Collections.Generic;

namespace SphericalCow
{
	public class SecondaryStat : AbstractStat 
	{
		//
		// Data
		//

		[SerializeField] private List<BasicStatPercentagePair> baseStatDerivations;

		//
		// Methods
		//

		public override StatType GetStatType ()
		{
			return StatType.Secondary;
		}


		//
		// Getters
		//

		public List<BasicStatPercentagePair> BaseStatDerivations
		{
			get
			{
				return this.baseStatDerivations;
			}
		}
	}
}