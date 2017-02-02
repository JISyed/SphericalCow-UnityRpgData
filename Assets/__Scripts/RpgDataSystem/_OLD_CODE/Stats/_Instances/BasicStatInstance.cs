using UnityEngine;
using Guid = System.Guid;

namespace SphericalCow.OldCode
{
	[System.Serializable]
	public class BasicStatInstance : AbstractStatInstance 
	{
		//
		// Data
		//
		
		[System.NonSerialized] private BasicStat statReference = null;
		

		//
		// Constructor (Does not run on Deserialization)
		//

		public BasicStatInstance(BasicStat statData, RpgCharacterData characterData) :
			base(statData.StatName, characterData)
		{
			this.statReference = statData;
		}


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
		
		
		
		//
		// Properties
		//
		
		public BasicStat BasicStatRef
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
