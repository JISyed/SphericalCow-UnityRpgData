using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SphericalCow
{
	/// <summary>
	/// 	A class of Stats that partially rely on the value of Base Stats
	/// </summary>
	public class SecondaryStat : AbstractStat 
	{
		[SerializeField] private List<BaseStatPercentagePair> linkedStats;
		private ReadOnlyCollection<BaseStatPercentagePair> readonlyList = null;
		
		
		/// <summary>
		/// 	Returns the what kind of stat this object is. [Secondary]
		/// </summary>
		public override StatType GetStatType()
		{
			return StatType.Secondary;
		}
		
		
		/// <summary>
		/// 	The list of Base Stats linked to this Secondary Stat
		/// </summary>
		public ReadOnlyCollection<BaseStatPercentagePair> LinkedStats
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
