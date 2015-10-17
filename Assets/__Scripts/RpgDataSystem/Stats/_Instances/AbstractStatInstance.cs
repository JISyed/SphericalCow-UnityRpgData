﻿//using UnityEngine;

namespace SphericalCow
{
	[System.Serializable]
	public abstract class AbstractStatInstance
	{
		//
		// Data
		//
		
		private string statName;
		private string statId;
		private int localXpPool;		// Total accumulated XP for Use-Assigned progression
		private int nextLevelXp;		// The total amount of local XP needed to level up
		[System.NonSerialized] protected RpgCharacterData character;

		
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

		public string StatId
		{
			get
			{
				return this.statId;
			}

			protected set
			{
				this.statId = value;
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
	