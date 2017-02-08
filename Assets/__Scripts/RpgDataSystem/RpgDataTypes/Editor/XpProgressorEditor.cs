using UnityEngine;
using UnityEditor;

namespace SphericalCow
{
	/// <summary>
	/// 	Acts as a custom inspector to XpProgressor
	/// </summary>
	[CustomEditor(typeof(XpProgressor))]
	public class XpProgressorEditor : Editor 
	{
		private XpProgressor dataObject;
		
		void OnEnable()
		{
			this.dataObject = target as XpProgressor;
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
