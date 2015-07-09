using UnityEngine;

namespace SphericalCow
{
	/// <summary>
	/// 	This is a saveable data container holding the base stats of the RPG system
	/// </summary>
	[System.Serializable]
	public sealed class GlobalGameStats
	{
		//
		// Data
		//

		private static GlobalGameStats instance;	// Singleton

		private int strength;
		private int agility;
		private int willpower;
		private int perception;
		private int luck;
		private int health;


		//
		// Methods
		//

		/// <summary>
		/// 	Initializes a new instance of the <see cref="GlobalGameStats"/> class.
		/// 	It's private because its a singleton
		/// </summary>
		private GlobalGameStats()
		{
			// Intentionally blank, for now
		}

		/// <summary>
		/// 	This methods gets called when <c>strength</c>, 
		/// 	<c>agility</c>, or <c>willpower</c> change. 
		/// 	Takes the integer average of the three, for now.
		/// </summary>
		private void RecalculateHealth()
		{
			this.health = (this.strength + this.agility + this.willpower) / 3;
		}


		//
		// Setters
		//

		public void SetStrength(int newStrengthValue)
		{
			this.strength = newStrengthValue;
			this.RecalculateHealth();
		}

		public void SetAgility(int newAgilityValue)
		{
			this.agility = newAgilityValue;
			this.RecalculateHealth();
		}

		public void SetWillpower(int newWillpowerValue)
		{
			this.willpower = newWillpowerValue;
			this.RecalculateHealth();
		}

		public void SetPerception(int newPerceptionValue)
		{
			this.perception = newPerceptionValue;
		}

		public void SetLuck(int newLuckValue)
		{
			this.luck = newLuckValue;
		}

		/// <summary>
		/// 	If incrementAmount is negative, it will decrement from the stat.
		/// </summary>
		public void IncrementStrength(int incrementAmount)
		{
			this.strength += incrementAmount;
			this.RecalculateHealth();
		}

		/// <summary>
		/// 	If incrementAmount is negative, it will decrement from the stat.
		/// </summary>
		public void IncrementAgility(int incrementAmount)
		{
			this.agility += incrementAmount;
			this.RecalculateHealth();
		}

		/// <summary>
		/// 	If incrementAmount is negative, it will decrement from the stat.
		/// </summary>
		public void IncrementWillpower(int incrementAmount)
		{
			this.willpower += incrementAmount;
			this.RecalculateHealth();
		}

		/// <summary>
		/// 	If incrementAmount is negative, it will decrement from the stat.
		/// </summary>
		public void IncrementPerception(int incrementAmount)
		{
			this.perception += incrementAmount;
		}

		/// <summary>
		/// 	If incrementAmount is negative, it will decrement from the stat.
		/// </summary>
		public void IncrementLuck(int incrementAmonut)
		{
			this.luck += incrementAmonut;
		}


		//
		// Properties
		//

		/// <summary>
		/// 	Get the singleton instance
		/// </summary>
		public GlobalGameStats Instance
		{
			get
			{
				if(GlobalGameStats.instance == null)
				{
					GlobalGameStats.instance = new GlobalGameStats();
				}

				return GlobalGameStats.instance;
			}
		}

		/// <summary>
		/// 	Dictates the amount the player can lift, 
		/// 	the damage they can do, and general virality.
		/// </summary>
		public int Strength
		{
			get
			{
				return this.strength;
			}
		}

		/// <summary>
		/// 	How quickly the player can move, stealth, 
		/// 	and chance of hit with melee weapons, and general vitality.
		/// </summary>
		public int Agility
		{
			get
			{
				return this.agility;
			}
		}

		/// <summary>
		/// 	Determines how well they deflect manipulation, 
		/// 	how well they manipulate, as well as how powerful their spells are.
		/// </summary>
		public int Willpower
		{
			get
			{
				return this.willpower;
			}
		}

		/// <summary>
		/// 	How aware they are of the world around them, 
		/// 	as well as accuracy with ranged weapons.
		/// </summary>
		public int Perception
		{
			get
			{
				return this.perception;
			}
		}

		/// <summary>
		/// 	The higher, the better
		/// </summary>
		public int Luck
		{
			get
			{
				return this.luck;
			}
		}

		/// <summary>
		/// 	This is based upon strength, agility, and willpower. Not set-able.
		/// </summary>]
		public int Health
		{
			get
			{
				return this.health;
			}
		}
	}	// end GlobalGameStats
}	// end namespace SphericalCow
