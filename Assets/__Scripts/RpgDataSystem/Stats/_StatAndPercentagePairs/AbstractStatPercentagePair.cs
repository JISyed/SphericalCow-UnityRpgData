using UnityEngine;

namespace SphericalCow
{
	[System.Serializable]
	public class AbstractStatPercentagePair
	{
		[SerializeField] private AbstractStat stat;
		[SerializeField] private int percentage;
		
		public AbstractStat Stat
		{
			get
			{
				return this.stat;
			}
		}
		
		public int Percentage
		{
			get
			{
				return this.percentage;
			}
		}
		
	}
}
