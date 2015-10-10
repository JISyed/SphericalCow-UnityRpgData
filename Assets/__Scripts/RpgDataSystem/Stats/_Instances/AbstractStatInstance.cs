//using UnityEngine;

namespace SphericalCow
{
	[System.Serializable]
	public abstract class AbstractStatInstance
	{
		//
		// Data
		//
		
		private string statName;
		//private ProgressionType progressionVariable;
		private int localXpPool;		// Total accumulated XP for Use-Assigned progression
		private int nextLevelXp;		// The total amount of local XP needed to level up
		
		
		//
		// Methods
		//
		
		public abstract StatType GetStatType();
		
		protected abstract void SetupStatReference();


		
		//
		// Properties
		//
		
		public void SetStatName(string newName)
		{
			this.statName = newName;
		}
		
		//public void SetProgressionVariable(ProgressionType newValue)
		//{
		//	this.progressionVariable = newValue;
		//}
		
		public void SetLocalXpPool(int newXpAmount)
		{
			this.localXpPool = newXpAmount;
		}
		
		public void SetNextLevelXp(int newValue)
		{
			this.nextLevelXp = newValue;
		}
		
		public string StatName
		{
			get
			{
				return this.statName;
			}
		}
		
		//public ProgressionType ProgressionVariable
		//{
		//	get
		//	{
		//		return this.progressionVariable;
		//	}
		//}
		
		public int LocalXpPool
		{
			get
			{
				return this.localXpPool;
			}
		}
		
		public int NextLevelXp
		{
			get
			{
				return this.nextLevelXp;
			}
		}
		
	}
}
	