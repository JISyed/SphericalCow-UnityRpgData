using UnityEngine;
using Guid = System.Guid;
using System.Collections.Generic;

namespace SphericalCow
{
	/// <summary>
	/// 	A Stat Point Deriver for SkillStats
	/// </summary>
	public class SkillSpDeriver : AbstractSpDeriver 
	{
		private SkillStat stat;
		
		
		/// <summary>
		/// 	Constructor needs a SkillStat
		/// </summary>
		public SkillSpDeriver(SkillStat statReference) : base(statReference.Id)
		{
			this.stat = statReference;
		}
		
		
		/// <summary>
		/// 	Acquire SP that derive from linked stats and their percentage of SP contribution.
		/// 	This method varies on the type of stat (Base, Secondary, Skill)
		/// </summary>
		public override void DeriveSp(List<StatData> appliedStats)
		{
			// Clear the derived pool before calculating
			this.DerivedSpPool = 0;
			
			// For every linked stat...
			foreach(AbstractStatPercentagePair stat in this.stat.LinkedStats)
			{
				// Find if the character has the linked stat
				foreach(StatData linkedStat in appliedStats)
				{
					// Does the stat exist in this character?
					if(linkedStat.Id.Equals(stat.Stat.Id))
					{
						int linkedStatSp = linkedStat.StatPoints;
						
						// Calculate contributions of this linked stat to to this deriver's pool
						int contributions = (int) Mathf.Round(stat.Percentage/100.0f * (float) linkedStatSp);
						//Debug.Log("Contribution: " + contributions.ToString());
						
						// Add contributions to this deriver's pool
						this.DerivedSpPool += contributions;
						
						// This stat was found, so go back to the outer loop
						break;
					}
				}
			}
			
			//Debug.Log("FinalDerivedPool: " + this.DerivedSpPool.ToString());
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
