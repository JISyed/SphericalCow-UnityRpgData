using UnityEngine;
using UnityEditor;

namespace SphericalCow
{
	/// <summary>
	/// 	Acts as a custom inspector to RpgDataRegistryEditor
	/// </summary>
	[CustomEditor(typeof(RpgDataRegistry))]
	public class RpgDataRegistryEditor : Editor 
	{
		private RpgDataRegistry registry;
		
		void OnEnable()
		{
			this.registry = target as RpgDataRegistry;
		}
		
		public override void OnInspectorGUI()
		{
			if(GUILayout.Button("Remove Null Entries"))
			{
				this.RemoveNullReferencesFromRegistry();
			}
			
			this.DrawDefaultInspector();
		}
		
		private void RemoveNullReferencesFromRegistry()
		{
			this.registry.CleanMissingReferences();
		}
	}
}
