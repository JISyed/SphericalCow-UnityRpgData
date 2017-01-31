using UnityEngine;
using UnityEditor;

namespace SphericalCow.OldCode
{
	/// <summary>
	/// 	Acts as a custom inspector to StatsAndAttributesRegistry
	/// </summary>
	[CustomEditor(typeof(StatsAndAttributesRegistry))]
	public class StatsAndAttributesRegistryEditor : Editor 
	{
		private StatsAndAttributesRegistry registry;
		
		void OnEnable()
		{
			this.registry = (StatsAndAttributesRegistry) target;
		}
		
		public override void OnInspectorGUI()
		{
			if(GUILayout.Button("Remove Missing Stats & Abilities"))
			{
				this.RemoveMissingStats();
			}
			
			this.DrawDefaultInspector();
		}
		
		private void RemoveMissingStats()
		{
			this.registry.EveryBasicStat.RemoveAll(stat => stat == null);
			this.registry.EverySecondaryStat.RemoveAll(stat => stat == null);
			this.registry.EverySkillStat.RemoveAll(stat => stat == null);
			this.registry.EveryAbility.RemoveAll(abilty => abilty == null);
		}
		
	}
}
	