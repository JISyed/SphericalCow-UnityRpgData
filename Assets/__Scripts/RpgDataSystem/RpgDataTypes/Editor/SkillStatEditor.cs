using UnityEngine;
using UnityEditor;

namespace SphericalCow
{
	/// <summary>
	/// 	Acts as a custom inspector to SkillStat
	/// </summary>
	[CustomEditor(typeof(SkillStat))]
	public class SkillStatEditor : Editor 
	{
		private SkillStat dataObject;
		
		void OnEnable()
		{
			this.dataObject = target as SkillStat;
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
