using UnityEngine;
using System.Collections.Generic;

namespace SphericalCow
{
	[System.Serializable]
	public class SkillStatInstance : AbstractStatInstance 
	{
		//
		// Data
		//
		
		[System.NonSerialized] private SkillStat statReference;
		[System.NonSerialized] private List<AbstractStatInstance> derivativeStats;
		
		
		//
		// Methods
		//
		
		public override StatType GetStatType ()
		{
			return StatType.Skill;
		}
		
		protected override void SetupStatReference ()
		{
			// TODO
			throw new System.NotImplementedException ();
		}
	}
}
	