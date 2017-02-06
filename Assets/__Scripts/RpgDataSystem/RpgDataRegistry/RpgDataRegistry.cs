using UnityEngine;
using Guid = System.Guid;
using System.Collections.Generic;

namespace SphericalCow
{
	/// <summary>
	/// 	The "database" of all RPG Data Types (XpProgressors, Stats, and Abilities).
	/// 	This does NOT contain data concerning RpgCharacters, only stores read-only data.
	/// 	Meant to be a singleton script attached to a persistent GameObject 
	/// </summary>
	public class RpgDataRegistry : MonoBehaviour 
	{
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
		
		
		
		[SerializeField] private XpProgressor[] xpProgressors;
		[SerializeField] private BaseStat[] baseStats;
		[SerializeField] private SecondaryStat[] secondaryStats;
		[SerializeField] private SkillStat[] skillStats;
		[SerializeField] private Ability[] abilties;
		
		
		
		
		void Awake()
		{
			if(RpgDataRegistry.instance != null)
			{
				Debug.LogError("There are multiple instances of RpgDataRegistry running!", this);
			}
		}
		
		
		
		
		private void Init()
		{
			
		}
		
		
		
		
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
		
		
		
		
		
		public XpProgressor[] XpProgressors
		{
			get
			{
				return this.xpProgressors;
			}
		}
		
		
		public BaseStat[] BaseStats
		{
			get
			{
				return this.baseStats;
			}
		}
		
		
		public SecondaryStat[] SecondaryStats
		{
			get
			{
				return this.secondaryStats;
			}
		}
		
		
		public SkillStat[] SkillStats
		{
			get
			{
				return this.skillStats;
			}
		}
		
		
		public Ability[] Abilities
		{
			get
			{
				return this.abilties;
			}
		}
		
		
		public int NumberOfXpProgressors
		{
			get
			{
				return this.xpProgressors.Length;
			}
		}
		
		
		public int NumberOfBaseStats
		{
			get
			{
				return this.baseStats.Length;
			}
		}
		
		
		public int NumberOfSecondaryStats
		{
			get
			{
				return this.secondaryStats.Length;
			}
		}
		
		
		public int NumberOfSkillStats
		{
			get
			{
				return this.skillStats.Length;
			}
		}
		
		
		public int NumberOfAbilities
		{
			get
			{
				return this.abilties.Length;
			}
		}
		
		
	}
}
