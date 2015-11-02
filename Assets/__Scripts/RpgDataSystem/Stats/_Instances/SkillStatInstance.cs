using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;	// for ReadOnlyCollection<>
using Guid = System.Guid;
using SphericalCow.Generics;

namespace SphericalCow
{
	[System.Serializable]
	public class SkillStatInstance : AbstractStatInstance 
	{
		//
		// Data
		//
		
		[System.NonSerialized] private SkillStat statReference = null;
		[System.NonSerialized] private List<Pair<AbstractStatInstance, int>> derivativeStats = null;



		//
		// Constructor (Does not run on Deserialization)
		//

		public SkillStatInstance(SkillStat statData, RpgCharacterData characterData)
		{
			this.statReference = statData;
			this.character = characterData;
			this.StatGuid = this.GenerateGuid();
			this.StatName = this.statReference.StatName;
			this.LocalXpPool = 0;		// Should the default XP be 0?
			this.NextLevelXp = 60;	// TODO: Find a way to intelligently calculate this!

			this.derivativeStats = new List<Pair<AbstractStatInstance, int>>();
			this.SetupStatInstanceAssociations();
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

		/// <summary>
		///  Do NOT call this all the time! Should only be called when a new stat is added to the RPG Character!
		/// </summary>
		public void SetupStatInstanceAssociations()
		{
			this.derivativeStats.Clear();
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
						Debug.LogError("SphericalCow.StatType enum has a member not handled in SkillStatInstance!");
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
		// Getter
		//

		/// <summary>
		///		Returns a read-only list of derivative AbstractStatInstances paired with their percentage,
		/// 	indicting how much they affect the final value of this SkillStatInstance
		/// </summary>
		public ReadOnlyCollection<Pair<AbstractStatInstance,int>> DerivativeStats
		{
			get
			{
				return this.derivativeStats.AsReadOnly();
			}
		}
	}
}
	