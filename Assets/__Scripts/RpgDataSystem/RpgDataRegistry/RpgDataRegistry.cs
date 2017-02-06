using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Guid = System.Guid;

namespace SphericalCow
{
	/// <summary>
	/// 	The "database" of all RPG Data Types (XpProgressors, Stats, and Abilities).
	/// 	This does NOT contain data concerning RpgCharacters, only stores read-only data.
	/// 	Meant to be a singleton script attached to a persistent GameObject 
	/// </summary>
	public class RpgDataRegistry : MonoBehaviour 
	{
		private static RpgDataRegistry instance = null;
		
		/// <summary>
		/// 	Singeton access to RpgDataRegistry
		/// </summary>
		/// <value>The instance.</value>
		public static RpgDataRegistry Instance
		{
			get
			{
				// TODO: Fix this singleton access by getting the registry prefab from a Resource folder
				
				if(RpgDataRegistry.instance == null)
				{
					GameObject registryObject = new GameObject("SGC RPG Data Registry Object");
					GameObject.DontDestroyOnLoad(registryObject);
					RpgDataRegistry newRegistry = registryObject.AddComponent<RpgDataRegistry>();
					RpgDataRegistry.instance = newRegistry;
					newRegistry.Init();
				}
				
				return RpgDataRegistry.instance;
			}
		}
		
		
		
		[SerializeField] private List<XpProgressor> xpProgressors;
		[SerializeField] private List<BaseStat> baseStats;
		[SerializeField] private List<SecondaryStat> secondaryStats;
		[SerializeField] private List<SkillStat> skillStats;
		[SerializeField] private List<Ability> abilties;
		
		
		
		
		
		private void Init()
		{
			
		}
		
		
	}
}
