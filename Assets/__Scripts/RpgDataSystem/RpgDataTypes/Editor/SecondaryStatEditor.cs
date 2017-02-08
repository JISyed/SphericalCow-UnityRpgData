using UnityEngine;
using UnityEditor;

namespace SphericalCow
{
	/// <summary>
	/// 	Acts as a custom inspector to SecondaryStat
	/// </summary>
	[CustomEditor(typeof(SecondaryStat))]
	public class SecondaryStatEditor : Editor 
	{
		private SecondaryStat dataObject;
		
		void OnEnable()
		{
			this.dataObject = target as SecondaryStat;
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
