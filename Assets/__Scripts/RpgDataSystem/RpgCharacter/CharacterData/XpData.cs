using UnityEngine;
using Guid = System.Guid;

namespace SphericalCow
{
	/// <summary>
	/// 	Stores data related to experience points (XP) for RpgCharacterData
	/// </summary>
	[System.Serializable]
	public class XpData 
	{
		[System.NonSerialized] private XpProgressor xpProgressor = null;
		[SerializeField] private SaveableGuid xpProgressorId;
		[SerializeField] private int level;
		[SerializeField] private int xp;
		[SerializeField] private int xpToNextLevel;
		[SerializeField] private int currentLevelMultiplier;
		[SerializeField] private int currentOldValueMultiplier;
		
		
		/// <summary>
		/// 	Constructor needs an XpProgressor
		/// </summary>
		public XpData(XpProgressor newXpProgressor)
		{
			this.xpProgressor = newXpProgressor;
			this.xpProgressorId = new SaveableGuid(newXpProgressor.Id);
			this.level = 1;
			this.xp = 0;
			this.xpToNextLevel = newXpProgressor.InitialOldXtnl;
			this.currentLevelMultiplier = newXpProgressor.LevelMultiplier;
			this.currentOldValueMultiplier = newXpProgressor.OldXtnlMultiplier;
			
			this.xpToNextLevel = this.CalculateXpProgression(this.level, this.xpToNextLevel);
		}
		
		
		/// <summary>
		/// 	Find the correct XpProgressor instance given its GUID. Used for deserialzation.
		/// </summary>
		public void LoadXpProgressor()
		{
			this.xpProgressorId.LoadInternalData();
			this.xpProgressor = RpgDataRegistry.Instance.SearchXpProgressor(this.xpProgressorId.GuidData);
			Debug.Assert(this.xpProgressor != null, "XpData failed to find the XpProgressor of ID " + this.xpProgressorId.GuidString);
		}
		
		
		/// <summary>
		/// 	Calculates the new XTNL (XP to the next level) given the old XTNL and new Level
		/// </summary>
		/// <param name="newLevel">The new level.</param>
		/// <param name="oldXtnl">Old value for XpToNextLevel.</param>
		private int CalculateXpProgression(int newLevel, int oldXtnl)
		{
			int newXtnl = this.currentLevelMultiplier * newLevel
						+ this.currentOldValueMultiplier * oldXtnl;
			
			if(this.xpProgressor.DoesLevelMultiplerIncrement)
			{
				this.currentLevelMultiplier++;
			}
			
			if(this.xpProgressor.DoesOldXntlMultiplierIncrement)
			{
				this.currentOldValueMultiplier++;
			}
			
			return newXtnl;
		}
		
		
		/// <summary>
		/// 	The current Level of the character
		/// </summary>
		public int Level
		{
			get
			{
				return this.level;
			}
		}
		
		
		/// <summary>
		/// 	The amount of Experience Points (XP) of the character
		/// </summary>
		public int XP
		{
			get
			{
				return this.xp;
			}
		}
		
		
		/// <summary>
		/// 	The amount of total XP to be reached before advancing to the next Level
		/// </summary>
		public int XpToNextLevel
		{
			get
			{
				return this.xpToNextLevel;
			}
		}
		
		
		/// <summary>
		/// 	The ID of the referenced XpProgressor
		/// </summary>
		public Guid XpProgressorId
		{
			get
			{
				return this.xpProgressorId.GuidData;
			}
		}
		
		
		/// <summary>
		/// 	The referenced XpProgressor
		/// </summary>
		public XpProgressor XpProgressorReference
		{
			get
			{
				return this.xpProgressor;
			}
		}
		
		
	}
}
	