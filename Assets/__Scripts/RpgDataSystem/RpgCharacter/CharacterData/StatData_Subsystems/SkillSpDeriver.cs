using UnityEngine;
using Guid = System.Guid;
using System.Collections.Generic;

namespace SphericalCow
{
	/// <summary>
	/// 	A Stat Point Deriver for SkillStats
	/// </summary>
	[System.Serializable]
	public class SkillSpDeriver : AbstractSpDeriver 
	{
		[System.NonSerialized] private SkillStat stat;
		
		
		/// <summary>
		/// 	Constructor needs a SkillStat
		/// </summary>
		public SkillSpDeriver(SkillStat statReference) : base(statReference.Id)
		{
			this.stat = statReference;
		}
		
		
		/// <summary>
		/// 	Aquire SP that derive from linked stats and their percentage of SP contribution.
		/// 	This method varies on the type of stat (Base, Secondary, Skill)
		/// </summary>
		public override void DeriveSp(List<StatData> appliedStats)
		{
			// TODO: Implement by refering to SkillStat's linked stats
		}
		
		
		
		/// <summary>
		/// 	The skill stat being referenced by this deriver
		/// </summary>
		public SkillStat Stat
		{
			get
			{
				return this.stat;
			}
		}
		
	}
}
