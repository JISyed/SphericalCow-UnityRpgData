using UnityEngine;

namespace SphericalCow
{
	/// <summary>
	/// 	The foundation for all other stats
	/// </summary>
	public class BaseStat : AbstractStat 
	{
		/// <summary>
		/// 	Returns the what kind of stat this object is. [Base]
		/// </summary>
		public override StatType GetStatType()
		{
			return StatType.Base;
		}
	}
}
	