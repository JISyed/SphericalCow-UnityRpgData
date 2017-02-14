using UnityEngine;
using Guid = System.Guid;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SphericalCow
{
	/// <summary>
	/// 	The "database" of all RPG Data Types (XpProgressors, Stats, and Abilities).
	/// 	This does NOT contain data concerning RpgCharacters, only stores read-only data.
	/// 	Meant to be a singleton script attached to a persistent GameObject 
	/// </summary>
	public class RpgDataRegistry : MonoBehaviour 
	{
		[Range(1,100)][SerializeField] private int pointAssignedMultiplier = 1;	// Used in PointAssigned algorithm
		[Range(0.5f, 2.0f)][SerializeField] private float pointToStatRatio = 1.0f;	// Used in PointAssigned algorithm
		[Range(1,100)][SerializeField] private int useAssignedMultiplier = 1;	// Used for UseAssigned algorithm
		[Range(0.1f, 2.0f)][SerializeField] private float useFactorRatio = 0.5f;	// Used for UseAssinged algorithm
		[SerializeField] private GlobalSpAssignmentType defaultSpAssignment;
		[SerializeField] private XpProgressor[] xpProgressors;
		[SerializeField] private BaseStat[] baseStats;
		[SerializeField] private SecondaryStat[] secondaryStats;
		[SerializeField] private SkillStat[] skillStats;
		[SerializeField] private Ability[] abilties;
		
		private ReadOnlyCollection<XpProgressor> readOnlyXpProgressors = null;
		private ReadOnlyCollection<BaseStat> readOnlyBaseStats = null;
		private ReadOnlyCollection<SecondaryStat> readOnlySecondaryStats = null;
		private ReadOnlyCollection<SkillStat> readOnlySkillStats = null;
		private ReadOnlyCollection<Ability> readOnlyAbilties = null;
		
		
		
		/// <summary>
		/// 	The name of the prefab holding the RpgDataRegistry script
		/// </summary>
		public const string RpgRegistryPrefabName = "RpgDataRegistryObject";
		
		private static RpgDataRegistry instance = null;
		
		
		
		
		/// <summary>
		/// 	Singeton access to RpgDataRegistry
		/// </summary>
		public static RpgDataRegistry Instance
		{
			get
			{
				if(RpgDataRegistry.instance == null)
				{
					GameObject registryObject = GameObject.Instantiate(Resources.Load<GameObject>(RpgDataRegistry.RpgRegistryPrefabName));
					GameObject.DontDestroyOnLoad(registryObject);
					RpgDataRegistry newRegistry = registryObject.GetComponent<RpgDataRegistry>();
					if(newRegistry == null)
					{
						Debug.LogError("RpgDataRegistry script is missing from the prefab " + RpgDataRegistry.RpgRegistryPrefabName);
						return null;
					}
					RpgDataRegistry.instance = newRegistry;
					newRegistry.Init();
				}
				
				return RpgDataRegistry.instance;
			}
		}
		
		
		
		// Called before Start()
		void Awake()
		{
			if(RpgDataRegistry.instance != null)
			{
				Debug.LogError("There are multiple instances of RpgDataRegistry running!", this);
			}
		}
		
		
		
		/// <summary>
		/// 	Intialization code for the registry
		/// </summary>
		private void Init()
		{
			// Deserialize the Guids within every entry of the registry (serialized as strings, we want Guids)
			foreach(XpProgressor entry in this.xpProgressors)
			{
				entry.RestoreGuidData();
			}
			foreach(BaseStat entry in this.baseStats)
			{
				entry.RestoreGuidData();
			}
			foreach(SecondaryStat entry in this.secondaryStats)
			{
				entry.RestoreGuidData();
			}
			foreach(SkillStat entry in this.skillStats)
			{
				entry.RestoreGuidData();
			}
			foreach(Ability entry in this.abilties)
			{
				entry.RestoreGuidData();
			}
			
			// Initialize the read-only list of every entry in the registry
			this.readOnlyXpProgressors = new ReadOnlyCollection<XpProgressor>(new List<XpProgressor>(this.xpProgressors));
			this.readOnlyBaseStats = new ReadOnlyCollection<BaseStat>(new List<BaseStat>(this.baseStats));
			this.readOnlySecondaryStats = new ReadOnlyCollection<SecondaryStat>(new List<SecondaryStat>(this.secondaryStats));
			this.readOnlySkillStats = new ReadOnlyCollection<SkillStat>(new List<SkillStat>(this.skillStats));
			this.readOnlyAbilties = new ReadOnlyCollection<Ability>(new List<Ability>(this.abilties));
		}
		
		
		
		/// <summary>
		/// 	Search for an XpProgressor by ID. Will return null if not found
		/// </summary>
		public XpProgressor SearchXpProgressor(Guid byId)
		{
			XpProgressor foundObject = null;
			
			foreach(XpProgressor entry in this.xpProgressors)
			{
				if(entry.Id.Equals(byId))
				{
					foundObject = entry;
					break;
				}
			}
			
			return foundObject;
		}
		
		
		/// <summary>
		/// 	Search for an XpProgressor by name (not filename). Will return null if not found
		/// </summary>
		public XpProgressor SearchXpProgressor(string byName)
		{
			XpProgressor foundObject = null;
			
			foreach(XpProgressor entry in this.xpProgressors)
			{
				if(entry.Name.Equals(byName))
				{
					foundObject = entry;
					break;
				}
			}
			
			return foundObject;
		}
		
		
		
		
		
		/// <summary>
		/// 	Search for any Stat by ID.
		/// 	Will search among BaseStats, SecondaryStats, and SkillStats.
		/// 	Will return null if not found
		/// </summary>
		public AbstractStat SearchAnyStat(Guid byId)
		{
			AbstractStat foundObject = null;
			
			foreach(BaseStat entry in this.baseStats)
			{
				if(entry.Id.Equals(byId))
				{
					foundObject = entry;
					break;
				}
			}
			
			if(foundObject == null)
			{
				foreach(SecondaryStat entry in this.secondaryStats)
				{
					if(entry.Id.Equals(byId))
					{
						foundObject = entry;
						break;
					}
				}
				
				if(foundObject == null)
				{
					foreach(SkillStat entry in this.skillStats)
					{
						if(entry.Id.Equals(byId))
						{
							foundObject = entry;
							break;
						}
					}
				}
			}
			
			return foundObject;
		}
		
		/// <summary>
		/// 	Search for any Stat by name (not filename). 
		/// 	Will search among BaseStats, SecondaryStats, and SkillStats. 
		/// 	Will return null if not found
		/// </summary>
		public AbstractStat SearchAnyStat(string byName)
		{
			AbstractStat foundObject = null;
			
			foreach(BaseStat entry in this.baseStats)
			{
				if(entry.Name.Equals(byName))
				{
					foundObject = entry;
					break;
				}
			}
			
			if(foundObject == null)
			{
				foreach(SecondaryStat entry in this.secondaryStats)
				{
					if(entry.Name.Equals(byName))
					{
						foundObject = entry;
						break;
					}
				}
				
				if(foundObject == null)
				{
					foreach(SkillStat entry in this.skillStats)
					{
						if(entry.Name.Equals(byName))
						{
							foundObject = entry;
							break;
						}
					}
				}
			}
			
			
			return foundObject;
		}
		
		
		
		
		
		
		/// <summary>
		/// 	Search for a BaseStat by ID. Will return null if not found
		/// </summary>
		public BaseStat SearchBaseStat(Guid byId)
		{
			BaseStat foundObject = null;
			
			foreach(BaseStat entry in this.baseStats)
			{
				if(entry.Id.Equals(byId))
				{
					foundObject = entry;
					break;
				}
			}
			
			return foundObject;
		}
		
		/// <summary>
		/// 	Search for a BaseStat by name (not filename). Will return null if not found
		/// </summary>
		public BaseStat SearchBaseStat(string byName)
		{
			BaseStat foundObject = null;
			
			foreach(BaseStat entry in this.baseStats)
			{
				if(entry.Name.Equals(byName))
				{
					foundObject = entry;
					break;
				}
			}
			
			return foundObject;
		}
		
		
		
		
		
		
		
		
		
		
		
		
		/// <summary>
		/// 	Search for a SecondaryStat by ID. Will return null if not found
		/// </summary>
		public SecondaryStat SearchSecondaryStat(Guid byId)
		{
			SecondaryStat foundObject = null;
			
			foreach(SecondaryStat entry in this.secondaryStats)
			{
				if(entry.Id.Equals(byId))
				{
					foundObject = entry;
					break;
				}
			}
			
			return foundObject;
		}
		
		
		/// <summary>
		/// 	Search for a SecondaryStat by name (not filename). Will return null if not found
		/// </summary>
		public SecondaryStat SearchSecondaryStat(string byName)
		{
			SecondaryStat foundObject = null;
			
			foreach(SecondaryStat entry in this.secondaryStats)
			{
				if(entry.Name.Equals(byName))
				{
					foundObject = entry;
					break;
				}
			}
			
			return foundObject;
		}
		
		
		
		
		
		/// <summary>
		/// 	Search for a SkillStat by ID. Will return null if not found
		/// </summary>
		public SkillStat SearchSkillStat(Guid byId)
		{
			SkillStat foundObject = null;
			
			foreach(SkillStat entry in this.skillStats)
			{
				if(entry.Id.Equals(byId))
				{
					foundObject = entry;
					break;
				}
			}
			
			return foundObject;
		}
		
		
		/// <summary>
		/// 	Search for a SkillStat by name (not filename). Will return null if not found
		/// </summary>
		public SkillStat SearchSkillStat(string byName)
		{
			SkillStat foundObject = null;
			
			foreach(SkillStat entry in this.skillStats)
			{
				if(entry.Name.Equals(byName))
				{
					foundObject = entry;
					break;
				}
			}
			
			return foundObject;
		}
		
		
		
		
		/// <summary>
		/// 	Search for an Ability by ID. Will return null if not found
		/// </summary>
		public Ability SearchAbility(Guid byId)
		{
			Ability foundObject = null;
			
			foreach(Ability entry in this.abilties)
			{
				if(entry.Id.Equals(byId))
				{
					foundObject = entry;
					break;
				}
			}
			
			return foundObject;
		}
		
		
		/// <summary>
		/// 	Search for an Ability by name (not filename). Will return null if not found
		/// </summary>
		public Ability SearchAbility(string byName)
		{
			Ability foundObject = null;
			
			foreach(Ability entry in this.abilties)
			{
				if(entry.Name.Equals(byName))
				{
					foundObject = entry;
					break;
				}
			}
			
			return foundObject;
		}
		
		
		
		// The following block will only run in the Unity Editor. Not meant for game code
		
		#if UNITY_EDITOR
			
			/// <summary>
			/// 	Adds an XpProgressor into the registry.
			/// 	Only to be called by RpgDataAssetUtility in the editor!
			/// </summary>
			public void AddRpgDataObject(RpgRegistryUtility.AdderOfXpProgressor newXpProgressor)
			{
				// Resize array
				int oldArraySize = this.NumberOfXpProgressors;
				System.Array.Resize<XpProgressor>(ref this.xpProgressors, oldArraySize + 1);
				
				Debug.Assert(this.NumberOfXpProgressors == oldArraySize + 1, "XpProgressor array resize failed!");
				Debug.Assert(newXpProgressor.xpProgressor != null, "Cannot add null XpProgressor to registry!");
				
				// Add new item
				this.xpProgressors[oldArraySize] = newXpProgressor.xpProgressor;
			}
			
			
			/// <summary>
			/// 	Adds a BaseStat into the registry.
			/// 	Only to be called by RpgDataAssetUtility in the editor!
			/// </summary>
			public void AddRpgDataObject(RpgRegistryUtility.AdderOfBaseStat newBaseStat)
			{
				// Resize array
				int oldArraySize = this.NumberOfBaseStats;
				System.Array.Resize<BaseStat>(ref this.baseStats, oldArraySize + 1);
				
				Debug.Assert(this.NumberOfBaseStats == oldArraySize + 1, "BaseStat array resize failed!");
				Debug.Assert(newBaseStat.baseStat != null, "Cannot add null BaseStat to registry!");
				
				// Add new item
				this.baseStats[oldArraySize] = newBaseStat.baseStat;
			}
			
			
			/// <summary>
			/// 	Adds a SecondaryStat into the registry.
			/// 	Only to be called by RpgDataAssetUtility in the editor!
			/// </summary>
			public void AddRpgDataObject(RpgRegistryUtility.AdderOfSecondaryStat newSecondaryStat)
			{
				// Resize array
				int oldArraySize = this.NumberOfSecondaryStats;
				System.Array.Resize<SecondaryStat>(ref this.secondaryStats, oldArraySize + 1);
				
				Debug.Assert(this.NumberOfSecondaryStats == oldArraySize + 1, "SecondaryStat array resize failed!");
				Debug.Assert(newSecondaryStat.secondaryStat != null, "Cannot add null SecondaryStat to registry!");
				
				// Add new item
				this.secondaryStats[oldArraySize] = newSecondaryStat.secondaryStat;
			}
			
			
			/// <summary>
			/// 	Adds a SkillStat into the registry.
			/// 	Only to be called by RpgDataAssetUtility in the editor!
			/// </summary>
			public void AddRpgDataObject(RpgRegistryUtility.AdderOfSkillStat newSkillStat)
			{
				// Resize array
				int oldArraySize = this.NumberOfSkillStats;
				System.Array.Resize<SkillStat>(ref this.skillStats, oldArraySize + 1);
				
				Debug.Assert(this.NumberOfSkillStats == oldArraySize + 1, "SkillStat array resize failed!");
				Debug.Assert(newSkillStat.skillStat != null, "Cannot add null SkillStat to registry!");
				
				// Add new item
				this.skillStats[oldArraySize] = newSkillStat.skillStat;
			}
			
			
			/// <summary>
			/// 	Adds an Ability into the registry.
			/// 	Only to be called by RpgDataAssetUtility in the editor!
			/// </summary>
			public void AddRpgDataObject(RpgRegistryUtility.AdderOfAbility newAbility)
			{
				// Resize array
				int oldArraySize = this.NumberOfAbilities;
				System.Array.Resize<Ability>(ref this.abilties, oldArraySize + 1);
				
				Debug.Assert(this.NumberOfAbilities == oldArraySize + 1, "Ability array resize failed!");
				Debug.Assert(newAbility.ability != null, "Cannot add null Ability to registry!");
				
				// Add new item
				this.abilties[oldArraySize] = newAbility.ability;
			}
			
			
			
			/// <summary>
			/// 	Looks through each list in the registry and removes null references, shrinking the arrays.
			/// 	Editor only. Should only be called by RpgDataFileWatcher or RpgDataRegistryEditor
			/// </summary>
			public void CleanMissingReferences()
			{
				bool wereAnyListsCleaned = false;
			
				wereAnyListsCleaned = this.CleanXpProgressorArray();
				wereAnyListsCleaned = this.CleanBaseStatArray();
				wereAnyListsCleaned = this.CleanSecondaryStatArray();
				wereAnyListsCleaned = this.CleanSkillStatArray();
				wereAnyListsCleaned = this.CleanAbilityArray();
				
				if(!wereAnyListsCleaned)
				{
					Debug.Log("The RPG Data Registry wasn't in need of cleaning null entires");
				}
			}
			
			
			private bool CleanXpProgressorArray()
			{
				Debug.Assert(this.xpProgressors != null, "Cannot clean XpProgressor array when it's null!");
				
				// Check if there are any null items in the array. If there isn't, there's no need to create a new array (wasting memory)
				bool arrayNeedsReconstruction = false;
				foreach(XpProgressor item in this.xpProgressors)
				{
					if(item == null)
					{
						arrayNeedsReconstruction = true;
						break;
					}
				}
				
				// Create a new array with no null items
				if(arrayNeedsReconstruction)
				{
					List<XpProgressor> arrayReconstructor = new List<XpProgressor>();
					foreach(XpProgressor item in this.xpProgressors)
					{
						if(item != null)
						{
							arrayReconstructor.Add(item);
						}
					}
					this.xpProgressors = arrayReconstructor.ToArray();
					Debug.Log("XpProgressor List in the RpgRegistry had null entries. Those entires were cleaned.");
				}
				
				return arrayNeedsReconstruction;
			}
			
			private bool CleanBaseStatArray()
			{
				Debug.Assert(this.baseStats != null, "Cannot clean BaseStat array when it's null!");
				
				// Check if there are any null items in the array. If there isn't, there's no need to create a new array (wasting memory)
				bool arrayNeedsReconstruction = false;
				foreach(BaseStat item in this.baseStats)
				{
					if(item == null)
					{
						arrayNeedsReconstruction = true;
						break;
					}
				}
				
				// Create a new array with no null items
				if(arrayNeedsReconstruction)
				{
					List<BaseStat> arrayReconstructor = new List<BaseStat>();
					foreach(BaseStat item in this.baseStats)
					{
						if(item != null)
						{
							arrayReconstructor.Add(item);
						}
					}
					this.baseStats = arrayReconstructor.ToArray();
					Debug.Log("BaseStat List in the RpgRegistry had null entries. Those entires were cleaned.");
				}
				
				return arrayNeedsReconstruction;
			}
			
			private bool CleanSecondaryStatArray()
			{
				Debug.Assert(this.secondaryStats != null, "Cannot clean SecondaryStat array when it's null!");
				
				// Check if there are any null items in the array. If there isn't, there's no need to create a new array (wasting memory)
				bool arrayNeedsReconstruction = false;
				foreach(SecondaryStat item in this.secondaryStats)
				{
					if(item == null)
					{
						arrayNeedsReconstruction = true;
						break;
					}
				}
				
				// Create a new array with no null items
				if(arrayNeedsReconstruction)
				{
					List<SecondaryStat> arrayReconstructor = new List<SecondaryStat>();
					foreach(SecondaryStat item in this.secondaryStats)
					{
						if(item != null)
						{
							arrayReconstructor.Add(item);
						}
					}
					this.secondaryStats = arrayReconstructor.ToArray();
					Debug.Log("SecondaryStat List in the RpgRegistry had null entries. Those entires were cleaned.");
				}
				
				return arrayNeedsReconstruction;
			}
			
			private bool CleanSkillStatArray()
			{
				Debug.Assert(this.skillStats != null, "Cannot clean SkillStat array when it's null!");
				
				// Check if there are any null items in the array. If there isn't, there's no need to create a new array (wasting memory)
				bool arrayNeedsReconstruction = false;
				foreach(SkillStat item in this.skillStats)
				{
					if(item == null)
					{
						arrayNeedsReconstruction = true;
						break;
					}
				}
				
				// Create a new array with no null items
				if(arrayNeedsReconstruction)
				{
					List<SkillStat> arrayReconstructor = new List<SkillStat>();
					foreach(SkillStat item in this.skillStats)
					{
						if(item != null)
						{
							arrayReconstructor.Add(item);
						}
					}
					this.skillStats = arrayReconstructor.ToArray();
					Debug.Log("SkillStat List in the RpgRegistry had null entries. Those entires were cleaned.");
				}
				
				return arrayNeedsReconstruction;
			}
			
			private bool CleanAbilityArray()
			{
				Debug.Assert(this.abilties != null, "Cannot clean Ability array when it's null!");
				
				// Check if there are any null items in the array. If there isn't, there's no need to create a new array (wasting memory)
				bool arrayNeedsReconstruction = false;
				foreach(Ability item in this.abilties)
				{
					if(item == null)
					{
						arrayNeedsReconstruction = true;
						break;
					}
				}
				
				// Create a new array with no null items
				if(arrayNeedsReconstruction)
				{
					List<Ability> arrayReconstructor = new List<Ability>();
					foreach(Ability item in this.abilties)
					{
						if(item != null)
						{
							arrayReconstructor.Add(item);
						}
					}
					this.abilties = arrayReconstructor.ToArray();
					Debug.Log("Ability List in the RpgRegistry had null entries. Those entires were cleaned.");
				}
				
				return arrayNeedsReconstruction;
			}
			
		#endif
		
		// End UNITY_EDITOR
		
		
		
		
		
		/// <summary>
		/// 	List of all XpProgressors in the registry
		/// </summary>
		public ReadOnlyCollection<XpProgressor> XpProgressors
		{
			get
			{
				return this.readOnlyXpProgressors;
			}
		}
		
		
		/// <summary>
		/// 	List of all BaseStats in the registry
		/// </summary>
		public ReadOnlyCollection<BaseStat> BaseStats
		{
			get
			{
				return this.readOnlyBaseStats;
			}
		}
		
		
		/// <summary>
		/// 	List of all SecondaryStats in the registry
		/// </summary>
		public ReadOnlyCollection<SecondaryStat> SecondaryStats
		{
			get
			{
				return this.readOnlySecondaryStats;
			}
		}
		
		
		/// <summary>
		/// 	List of all SkillStats in the registry
		/// </summary>
		public ReadOnlyCollection<SkillStat> SkillStats
		{
			get
			{
				return this.readOnlySkillStats;
			}
		}
		
		
		/// <summary>
		/// 	List of all Abilties in the registry
		/// </summary>
		public ReadOnlyCollection<Ability> Abilities
		{
			get
			{
				return this.readOnlyAbilties;
			}
		}
		
		
		/// <summary>
		/// 	Total number of XpProgressors on record in the registry
		/// </summary>
		public int NumberOfXpProgressors
		{
			get
			{
				return this.xpProgressors.Length;
			}
		}
		
		
		/// <summary>
		/// 	Total number of BaseStats on record in the registry
		/// </summary>
		public int NumberOfBaseStats
		{
			get
			{
				return this.baseStats.Length;
			}
		}
		
		
		/// <summary>
		/// 	Total number of SecondaryStats on record in the registry
		/// </summary>
		public int NumberOfSecondaryStats
		{
			get
			{
				return this.secondaryStats.Length;
			}
		}
		
		
		/// <summary>
		/// 	Total number of SkillStats on record in the registry
		/// </summary>
		public int NumberOfSkillStats
		{
			get
			{
				return this.skillStats.Length;
			}
		}
		
		
		/// <summary>
		/// 	Total number of Abilities on record in the registry
		/// </summary>
		public int NumberOfAbilities
		{
			get
			{
				return this.abilties.Length;
			}
		}
		
		
		/// <summary>
		/// 	The default SP assignment method to apply onto RpgCharacters (either UseAssigned or PointAssigned)
		/// </summary>
		public GlobalSpAssignmentType DefaultStatPointAssignment
		{
			get
			{
				return this.defaultSpAssignment;
			}
		}
		
		
		/// <summary>
		/// 	Used in PointAssigned algorithm. This multiplies the amount of SP rewarded on levelup
		/// 	after doing the point-assignment calculations. Default value is 1.
		/// </summary>
		public int PointAssignedMultiplier
		{
			get
			{
				return this.pointAssignedMultiplier;
			}
		}
		
		
		/// <summary>
		/// 	Used in PointAssigned algorithm. It's the ratio between how many stats a character has 
		/// 	and how many SP should be rewarded when leveling up. For example, if a player has 4 stats
		///		and the point-to-stat ratio is 0.5, then players will get 2 SP on levelup. If the ratio 
		///		is 1.0, then they will get 4 SP. If 2.0, they will get 8 SP. 
		/// </summary>
		public float PointToStatRatio
		{
			get
			{
				return this.pointToStatRatio;
			}
		}
		
		
		/// <summary>
		/// 	Used in UseAssigned algorithm. This multiplies the amount of SP rewarded on levelup
		/// 	after doing the use-assignment calculations. Default value is 1.
		/// </summary>
		public int UseAssignedMultiplier
		{
			get
			{
				return this.useAssignedMultiplier;
			}
		}
		
		
		/// <summary>
		/// 	Used in UseAssigned algorithm. It's the ratio of the stat's UseFactor to be calculated
		/// 	into a square root function in order to get the amount of SP rewarded on level up.
		/// 	The function is:
		/// 						rewarded_SP = sqrt(UseFactor * Ratio) * Multiplier
		/// 	This function can be studied on an online graphing calculator like Demos
		/// </summary>
		public float UseFactorRatio
		{
			get
			{
				return this.useFactorRatio;
			}
		}
		
		
	}
}
