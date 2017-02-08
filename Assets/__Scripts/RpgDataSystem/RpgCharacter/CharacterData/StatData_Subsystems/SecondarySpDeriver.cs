using UnityEngine;
using Guid = System.Guid;

namespace SphericalCow
{
	/// <summary>
	/// 	A Stat Point Deriver for SecondaryStats
	/// </summary>
	[System.Serializable]
	public class SecondarySpDeriver : AbstractSpDeriver 
	{
		[System.NonSerialized] private SecondaryStat stat;
		
		
		
		/// <summary>
		/// 	Constructor needs a SecondaryStat
		/// </summary>
		public SecondarySpDeriver(SecondaryStat statReference) : base(statReference.Id)
		{
			this.stat = statReference;
		}
		
		
		/// <summary>
		/// 	Aquire SP that derive from linked stats and their percentage of SP contribution.
		/// 	This method varies on the type of stat (Base, Secondary, Skill)
		/// </summary>
		public override void DeriveSp()
		{
			// TODO: Implement by refering to SecondaryStat's linked stats
		}
		
		
		
		/// <summary>
		/// 	The secondary stat being referenced by this deriver
		/// </summary>
		public SecondaryStat Stat
		{
			get
			{
				return this.stat;
			}
		}
		
	}
}
