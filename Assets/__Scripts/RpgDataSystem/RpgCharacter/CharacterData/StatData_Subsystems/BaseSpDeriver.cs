using UnityEngine;
using Guid = System.Guid;
using System.Collections.Generic;

namespace SphericalCow
{
	/// <summary>
	/// 	A Stat Point Deriver for BaseStats. Doesn't do anything because BaseStats don't derive SP from other stats
	/// </summary>
	[System.Serializable]
	public class BaseSpDeriver : AbstractSpDeriver 
	{
		/// <summary>
		/// 	Constructor needs a BaseStat
		/// </summary>
		public BaseSpDeriver(BaseStat statReference) : base(statReference.Id)
		{
			// Intentionally empty
		}
		
		
		/// <summary>
		/// 	Aquire SP that derive from linked stats and their percentage of SP contribution.
		/// 	For the BaseSpDeriver, this does nothing
		/// </summary>
		public override void DeriveSp(List<StatData> appliedStats)
		{
			// Intentionally does nothing, because BaseStats don't derive SP from other stats
		}
	}
}
