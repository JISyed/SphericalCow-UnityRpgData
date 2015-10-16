using UnityEngine;

namespace SphericalCow
{
	public class BasicStat : AbstractStat 
	{
		//
		// Methods
		//

		public override StatType GetStatType ()
		{
			return StatType.Basic;
		}
	}
}
