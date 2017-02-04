using UnityEngine;

namespace SphericalCow
{
	[System.Serializable]
	public class BaseStatPercentagePair 
	{
		[SerializeField] private BaseStat stat;
		[Range(0,100)] [SerializeField] private float percentage;
		
		public BaseStat Stat
		{
			get
			{
				return this.stat;
			}
		}
		
		public float Percentage
		{
			get
			{
				return this.percentage;
			}
		}
		
	}
}
