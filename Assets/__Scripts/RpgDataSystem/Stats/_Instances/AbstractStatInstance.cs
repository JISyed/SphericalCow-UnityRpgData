//using UnityEngine;
using Guid = System.Guid;

namespace SphericalCow
{
	[System.Serializable]
	public abstract class AbstractStatInstance
	{
		//
		// Data
		//
		
		private string statName;
		private Guid statGuid;
		private int localXpPool;		// Total accumulated XP for Use-Assigned progression
		private int nextLevelXp;		// The total amount of local XP needed to level up
		[System.NonSerialized] protected RpgCharacterData character;

		
		//
		// Methods
		//
		
		public abstract StatType GetStatType();
		
		protected abstract void SetupStatReference();

		protected Guid GenerateGuid()
		{
			return Guid.NewGuid();
		}

		
		//
		// Properties
		//
		
		public void SetStatName(string newName)
		{
			this.statName = newName;
		}
		
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

		public Guid StatGuid
		{
			get
			{
				return this.statGuid;
			}

			protected set
			{
				this.statGuid = value;
			}
		}
		
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

		public RpgCharacterData Character
		{
			get
			{
				return this.character;
			}
		}
		
	}
}
	