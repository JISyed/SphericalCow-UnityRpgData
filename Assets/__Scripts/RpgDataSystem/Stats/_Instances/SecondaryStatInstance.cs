using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;	// for ReadOnlyCollection<>
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
		
		[System.NonSerialized] private SecondaryStat statReference = null;
		[System.NonSerialized] private List<Pair<BasicStatInstance, int>> derivativeBasicStats = null;


		//
		// Constructor (Does not run on Deserialization)
		//

		public SecondaryStatInstance(SecondaryStat statData, RpgCharacterData characterData) : 
			base(statData.StatName, characterData)
		{
			this.statReference = statData;
			this.derivativeBasicStats = new List<Pair<BasicStatInstance, int>>();
			this.SetupBasicStatInstanceAssociations();
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

		/// <summary>
		///  Do NOT call this all the time! Should only be called when a new stat is added to the RPG Character!
		/// </summary>
		public void SetupBasicStatInstanceAssociations()
		{
			this.derivativeBasicStats.Clear();
			{
				// Setup derivate list
				BasicStatInstance currentBasicStat;
				foreach(var statPercentPair in this.statReference.BaseStatDerivations)
				{
					// Find the stat from the character
					currentBasicStat = this.Character.FindBasicStatInstance(statPercentPair.Stat.StatName);
					
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
		// Getters
		//

		/// <summary>
		///		Returns a read-only list of derivative BasicStatInstances paired with their percentage,
		/// 	indicting how much they affect the final value of this SecondaryStatInstance
		/// </summary>
		public ReadOnlyCollection<Pair<BasicStatInstance, int>> DerivativeBasicStats
		{
			get
			{
				return this.derivativeBasicStats.AsReadOnly();
			}
		}
		
		public SecondaryStat SecondaryStatRef
		{
			get
			{
				return this.statReference;
			}
		}
		
		/// <summary>
		/// 	Get the maximum amount of stat points possible for this particular stat. If 0 or less, then there is no max.
		/// </summary>
		override public int AbsoluteMaxStatPoints
		{
			get
			{
				return this.statReference.AbsoluteMaxStatPoint;
			}
		}
	}
}
