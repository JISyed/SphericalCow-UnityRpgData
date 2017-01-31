using UnityEngine;
using System.Collections.Generic;

namespace SphericalCow.OldCode
{
	/// <summary>
	/// 	The Stats and Attributes Registry is responsible for holding queriable instances 
	///     of AbstractStat and Attributes (not instance).
	/// 	There can only be one of these ever (as in Singleton).
	/// </summary>
	public class StatsAndAttributesRegistry : MonoBehaviour 
	{
		[SerializeField] private GameObject prefab;
		
		[SerializeField] private List<BasicStat> everyBasicStat;
		[SerializeField] private List<SecondaryStat> everySecondaryStat;
		[SerializeField] private List<SkillStat> everySkillStat;
		[SerializeField] private List<Ability> everyAbility;
		
		private static StatsAndAttributesRegistry registryInstance;
		
		// Use this for early initialization
		void Awake () 
		{
			if(registryInstance != null)
			{
				Debug.LogError("There are multiple instances of StatsAndAttirbutesRegistry(s)!");
			}
			else
			{
				registryInstance = this;
			}
		}
		
		void OnDestroy()
		{
			registryInstance = null;
		}
		
		/// <summary>
		/// 	Only to be called by StatAssetUtility!
		/// </summary>
		public void AddBasicStat(BasicStat newStat)
		{
			this.everyBasicStat.Add(newStat);
		}
		
		/// <summary>
		/// 	Only to be called by StatAssetUtility!
		/// </summary>
		public void AddSecondaryStat(SecondaryStat newStat)
		{
			this.everySecondaryStat.Add(newStat);
		}
		
		/// <summary>
		/// 	Only to be called by StatAssetUtility!
		/// </summary>
		public void AddSkillStat(SkillStat newStat)
		{
			this.everySkillStat.Add(newStat);
		}
		
		/// <summary>
		/// 	Only to be called by StatAssetUtility!
		/// </summary>
		public void AddAbility(Ability newAbility)
		{
			this.everyAbility.Add(newAbility);
		}
		
		//
		// Properties
		//
		
		public List<BasicStat> EveryBasicStat
		{
			get
			{
				return this.everyBasicStat;
			}
		}
		
		public List<SecondaryStat> EverySecondaryStat
		{
			get
			{
				return this.everySecondaryStat;
			}
		}
		
		public List<SkillStat> EverySkillStat
		{
			get
			{
				return this.everySkillStat;
			}
		}
		
		public List<Ability> EveryAbility
		{
			get
			{
				return this.everyAbility;
			}
		}
	}
}
	