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
		
		public void SetLocalXpPoolManually(int newXpAmount)
		{
			this.LocalXpPool = newXpAmount;
		}

		public void SetNextLevelXpManually(int newValue)
		{
			this.NextLevelXp = newValue;
		}
		
		public string StatName
		{
			get
			{
				return this.statName;
			}

			protected set
			{
				this.statName = value;
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

			protected set
			{
				this.localXpPool = value;
			}
		}
		
		public int NextLevelXp
		{
			get
			{
				return this.nextLevelXp;
			}

			protected set
			{
				this.nextLevelXp = value;
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
	