using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SphericalCow
{
	/// <summary>
	/// 	A class of specific Stats based off skills that can partially rely on the value of any other Stat
	/// </summary>
	public class SkillStat : AbstractStat
	{
		[SerializeField] private List<AbstractStatPercentagePair> linkedStats;
		private ReadOnlyCollection<AbstractStatPercentagePair> readonlyList = null;
		
		
		/// <summary>
		/// 	Returns the what kind of stat this object is. [Skill]
		/// </summary>
		public override StatType GetStatType()
		{
			return StatType.Skill;
		}
		
		
		/// <summary>
		/// 	Get the list of Stats linked to this Skill Stat
		/// </summary>
		public ReadOnlyCollection<AbstractStatPercentagePair> LinkedStats
		{
			get
			{
				if(this.readonlyList == null)
				{
					this.readonlyList = this.linkedStats.AsReadOnly();
				}
				
				return this.readonlyList;
			}
		}
	}
}
