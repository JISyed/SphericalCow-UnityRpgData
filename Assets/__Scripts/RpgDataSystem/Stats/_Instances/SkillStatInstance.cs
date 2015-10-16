using UnityEngine;
using System.Collections.Generic;
using SphericalCow.Generics;

namespace SphericalCow
{
	[System.Serializable]
	public class SkillStatInstance : AbstractStatInstance 
	{
		//
		// Data
		//
		
		[System.NonSerialized] private SkillStat statReference;
		[System.NonSerialized] private List<Pair<AbstractStatInstance, int>> derivativeStats;



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
			this.derivativeStats = new List<Pair<AbstractStatInstance, int>>();

			{
				// Setup derivate list
				AbstractStatInstance currentStat = null;
				foreach(var statPercentPair in this.statReference.StatDerivations)
				{
					// Find the stat from the character depending on the stat type
					switch (statPercentPair.Stat.GetStatType()) 
					{
					case StatType.Basic:
						currentStat = this.character.FindBasicStatInstance(statPercentPair.Stat.StatName);
						break;
					case StatType.Secondary:
						currentStat = this.character.FindSecondaryStatInstance(statPercentPair.Stat.StatName);
						break;
					case StatType.Skill:
						currentStat = this.character.FindSkillStatInstance(statPercentPair.Stat.StatName);
						break;
					default:
						Debug.LogError("SkillStateInstance " + statData.StatName + "entered unexpected switch case!");
						break;
					}

					
					// Skip if not found
					if(currentStat == null)
					{
						continue;
					}
					
					// Add to the derivation list
					var newPair = new Pair<AbstractStatInstance, int>(currentStat, statPercentPair.Percentage);
					this.derivativeStats.Add(newPair);
				}
			}
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


		//
		// Getter
		//

		public List<Pair<AbstractStatInstance,int>> DerivativeStats
		{
			get
			{
				return this.derivativeStats;
			}
		}
	}
}
	