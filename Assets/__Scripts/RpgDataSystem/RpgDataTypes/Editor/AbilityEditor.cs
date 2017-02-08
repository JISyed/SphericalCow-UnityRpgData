using UnityEngine;
using UnityEditor;

namespace SphericalCow
{
	/// <summary>
	/// 	Acts as a custom inspector to Ability
	/// </summary>
	[CustomEditor(typeof(Ability))]
	public class AbilityEditor : Editor 
	{
		private Ability dataObject;
		
		void OnEnable()
		{
			this.dataObject = target as Ability;
		}
		
		public override void OnInspectorGUI()
		{
			if(GUILayout.Button("Generate New ID (if missing)"))
			{
				this.GenerateNewId();
				EditorUtility.SetDirty(this.dataObject);
			}
			
			this.DrawDefaultInspector();
		}
		
		private void GenerateNewId()
		{
			this.dataObject.RegenerateNewId();
		}
	}
}
