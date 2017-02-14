using UnityEngine;
using Guid = System.Guid;
using System.Collections.Generic;

namespace SphericalCow
{
	/// <summary>
	/// 	A Stat Point Deriver for SecondaryStats
	/// </summary>
	[System.Serializable]
	public class SecondarySpDeriver : AbstractSpDeriver 
	{
		[System.NonSerialized] private SecondaryStat stat;
		
		
		
		/// <summary>
		/// 	Constructor needs a SecondaryStat
		/// </summary>
		public SecondarySpDeriver(SecondaryStat statReference) : base(statReference.Id)
		{
			this.stat = statReference;
		}
		
		
		/// <summary>
		/// 	Aquire SP that derive from linked stats and their percentage of SP contribution.
		/// 	This method varies on the type of stat (Base, Secondary, Skill)
		/// </summary>
		public override void DeriveSp(List<StatData> appliedStats)
		{
			// Clear the derived pool before calculating
			this.DerivedSpPool = 0;
			
			// For every linked stat...
			foreach(BaseStatPercentagePair baseStat in this.stat.LinkedStats)
			{
				// Find if the character has the linked stat
				foreach(StatData linkedStat in appliedStats)
				{
					// Does the base stat exist in this character?
					if(linkedStat.Id.Equals(baseStat.Stat.Id))
					{
						int linkedStatSp = linkedStat.ModifiedUnlinkedStatPoints;
						
						// Calculate contributions of this linked stat to to this deriver's pool
						int contributions = (int) Mathf.Round(baseStat.Percentage/100.0f * (float) linkedStatSp);
						Debug.Log("Contribution: " + contributions.ToString());
						
						// Add contributions to this deriver's pool
						this.DerivedSpPool += contributions;
						
						// This base stat was found, so go back to the outer loop
						break;
					}
				}
			}
			
			Debug.Log("FinalDerivedPool: " + this.DerivedSpPool.ToString());
		}
		
		
		
		/// <summary>
		/// 	The secondary stat being referenced by this deriver
		/// </summary>
		public SecondaryStat Stat
		{
			get
			{
				return this.stat;
			}
		}
		
	}
}
