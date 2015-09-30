using UnityEngine;

namespace SphericalCow
{
	[System.Serializable]
	public class BasicStatInstance : AbstractStatInstance 
	{
		//
		// Data
		//
		
		[System.NonSerialized] private BasicStat statReference;
		
		
		//
		// Methods
		//
		
		public override StatType GetStatType ()
		{
			return StatType.Basic;
		}
		
		protected override void SetupStatReference ()
		{
			// TODO: Setup BasicStatInstance is not implemented!
			throw new System.NotImplementedException ();
		}
		
	}
	
}
