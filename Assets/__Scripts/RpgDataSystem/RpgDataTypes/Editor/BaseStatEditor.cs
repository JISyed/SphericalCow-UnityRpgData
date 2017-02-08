using UnityEngine;
using UnityEditor;

namespace SphericalCow
{
	/// <summary>
	/// 	Acts as a custom inspector to BaseStat
	/// </summary>
	[CustomEditor(typeof(BaseStat))]
	public class BaseStatEditor : Editor 
	{
		private BaseStat dataObject;
		
		void OnEnable()
		{
			this.dataObject = target as BaseStat;
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
