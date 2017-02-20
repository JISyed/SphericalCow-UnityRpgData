using UnityEngine;
using System.Collections;

namespace SphericalCow
{
	/// <summary>
	/// 	A special object that holds data to help calculate 
	/// 	the next XP thresholds (xpToNextLevel, aka XTNL).
	/// 	Actual level formula is found in XpData
	/// </summary>
	public class XpProgressor : AbstractRpgDataType 
	{
		[SerializeField] private int initialOldXtnl;
		[SerializeField] private int levelMultiplier;
		[SerializeField] private int oldXtnlMultiplier;
		[SerializeField] private bool doesLevelMultiplierIncrement;
		[SerializeField] private bool doesOldXtnlMultiplierIncrement;
		
		/// <summary>
		/// 	The initial value for XpToNextLevel (XTNL).
		/// 	Infrequently needed for progression calculations.
		/// 	Usually 1 or 0 depending on XpProressor file.
		/// </summary>
		public int InitialOldXtnl
		{
			get
			{
				return this.initialOldXtnl;
			}
		}
		
		/// <summary>
		/// 	The multiplier for the level value. Used in progression calculations.
		/// </summary>
		public int LevelMultiplier
		{
			get
			{
				return this.levelMultiplier;
			}
		}
		
		/// <summary>
		/// 	The multiplier for the old XpToNextLevel (XTNL) value. Used in progression calculations.
		/// </summary>
		public int OldXtnlMultiplier
		{
			get
			{
				return this.oldXtnlMultiplier;
			}
		}
		
		/// <summary>
		/// 	Does the multiplier for the level value increment every time the player levels up?
		/// </summary>
		public bool DoesLevelMultiplerIncrement
		{
			get
			{
				return this.doesLevelMultiplierIncrement;
			}
		}
		
		/// <summary>
		/// 	Does the multiplier for the old XpToNextLevel (XTNL) value increment every time the player levels up?
		/// </summary>
		public bool DoesOldXntlMultiplierIncrement
		{
			get
			{
				return this.doesOldXtnlMultiplierIncrement;
			}
		}
		
	}
}
	