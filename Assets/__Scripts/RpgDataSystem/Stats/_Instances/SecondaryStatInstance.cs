using UnityEngine;
using System.Collections.Generic;

namespace SphericalCow
{
	[System.Serializable]
	public class SecondaryStatInstance : AbstractStatInstance 
	{
		//
		// Data
		//
		
		[System.NonSerialized] private SecondaryStat statReference;
		[System.NonSerialized] private List<BasicStatInstance> derivativeBasicStats;
		
		
		//
		// Methods
		//
		
		public override StatType GetStatType ()
		{
			return StatType.Secondary;
		}
		
		protected override void SetupStatReference ()
		{
			// TODO: SetupStatReference for SecondaryStatInstance is not implemented!
			throw new System.NotImplementedException ();
		}
		
	}
}
