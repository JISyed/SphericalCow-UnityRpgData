using UnityEngine;
using System.Collections.Generic;
using Guid = System.Guid;
using SphericalCow.Generics;


namespace SphericalCow
{
	[System.Serializable]
	public class SecondaryStatInstance : AbstractStatInstance 
	{
		//
		// Data
		//
		
		[System.NonSerialized] private SecondaryStat statReference;
		[System.NonSerialized] private List<Pair<BasicStatInstance, int>> derivativeBasicStats;


		//
		// Constructor (Does not run on Deserialization)
		//

		public SecondaryStatInstance(SecondaryStat statData, RpgCharacterData characterData)
		{
			this.statReference = statData;
			this.character = characterData;
			this.StatGuid = this.GenerateGuid();
			this.SetStatName(this.statReference.StatName);
			this.SetLocalXpPool(0);		// Should the default XP be 0?
			this.SetNextLevelXp(80);	// TODO: Find a way to intelligently calculate this!
			this.derivativeBasicStats = new List<Pair<BasicStatInstance, int>>();

			{
				// Setup derivate list
				BasicStatInstance currentBasicStat;
				foreach(var statPercentPair in this.statReference.BaseStatDerivations)
				{
					// Find the stat from the character
					currentBasicStat = this.character.FindBasicStatInstance(statPercentPair.Stat.StatName);

					// Skip if not found
					if(currentBasicStat == null)
					{
						continue;
					}

					// Add to the derivation list
					var newPair = new Pair<BasicStatInstance, int>(currentBasicStat, statPercentPair.Percentage);
					this.derivativeBasicStats.Add(newPair);
				}
			}
		}

		
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


		//
		// Getters
		//

		public List<Pair<BasicStatInstance, int>> DerivativeBasicStats
		{
			get
			{
				return this.derivativeBasicStats;
			}
		}
		
	}
}
