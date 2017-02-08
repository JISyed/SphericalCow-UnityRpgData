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
		private const int SizeOfProgressionChart = 20;
		
		private XpProgressor dataObject;
		private int currentLevelMultiplier;
		private int currentOldValueMultiplier;
		
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
			
			
			GUILayout.Space(20);
			
			
			GUILayout.Label("Progression Formula: (XTNL = XpToNextLevel)");
			GUILayout.Space(5);
			string formulaStr = string.Format("NewXTNL = {0} * CurrentLevel + {1} * OldXTNL", 
			                                  this.dataObject.LevelMultiplier,
			                                  this.dataObject.OldXtnlMultiplier);
			GUILayout.Label(formulaStr);
			string initXtnlStr = string.Format("     Where the intial OldXTNL = {0}", this.dataObject.InitialOldXtnl);
			GUILayout.Label(initXtnlStr);
			
			
			GUILayout.Space(20);
			
			
			GUILayout.Label("Progression Chart: ");
			GUILayout.Label("Level\t|  New XTNL");
			GUILayout.Label("==================");
			this.currentLevelMultiplier = this.dataObject.LevelMultiplier;
			this.currentOldValueMultiplier = this.dataObject.OldXtnlMultiplier;
			int xtnl = this.dataObject.InitialOldXtnl;
			for(int i = 1; i <= XpProgressorEditor.SizeOfProgressionChart; i++)
			{
				xtnl = this.CalculateXpProgression(i, xtnl);
				string rowStr = string.Format("{0}\t|    {1}", i, xtnl);
				GUILayout.Label(rowStr);
			}
		}
		
		private void GenerateNewId()
		{
			this.dataObject.RegenerateNewId();
		}
		
		
		
		
		/// <summary>
		/// 	Calculates the new XTNL (XP to the next level) given the old XTNL and new Level.
		/// 	If the associated XpProgressor increments internal multipliers, those will be incremented here.
		/// </summary>
		/// <param name="newLevel">The new level.</param>
		/// <param name="oldXtnl">Old value for XpToNextLevel.</param>
		private int CalculateXpProgression(int newLevel, int oldXtnl)
		{
			int newXtnl = this.currentLevelMultiplier * newLevel
				+ this.currentOldValueMultiplier * oldXtnl;
			
			if(this.dataObject.DoesLevelMultiplerIncrement)
			{
				this.currentLevelMultiplier++;
			}
			
			if(this.dataObject.DoesOldXntlMultiplierIncrement)
			{
				this.currentOldValueMultiplier++;
			}
			
			return newXtnl;
		}
	}
}
