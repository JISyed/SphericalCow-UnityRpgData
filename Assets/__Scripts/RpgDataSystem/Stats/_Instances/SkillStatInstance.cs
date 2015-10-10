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
		// Constructor (Does not run on Deserialization)
		//

		public SkillStatInstance(SkillStat statData, RpgCharacterData characterData)
		{
			this.statReference = statData;
			this.character = characterData;
			this.SetStatName(this.statReference.StatName);
			this.SetLocalXpPool(0);		// Should the default XP be 0?
			this.SetNextLevelXp(60);	// TODO: Find a way to intelligently calculate this!
		}


		//
		// Methods
		//
		
		public override StatType GetStatType ()
		{
			return StatType.Skill;
		}
		
		protected override void SetupStatReference ()
		{
			// TODO: Setup of stat reference in SkillStatInstance is not implemented!
			throw new System.NotImplementedException ();
		}
	}
}
	