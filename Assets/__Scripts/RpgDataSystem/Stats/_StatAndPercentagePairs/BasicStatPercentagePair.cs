using UnityEngine;

namespace SphericalCow
{
	[System.Serializable]
	public class BasicStatPercentagePair 
	{
		[SerializeField] private BasicStat stat;
		[SerializeField] private int percentage;
		
		public BasicStat Stat
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
