using UnityEngine;
using Guid = System.Guid;

namespace SphericalCow
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
	}
	
}
