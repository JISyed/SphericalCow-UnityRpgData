using UnityEngine;

namespace SphericalCow
{
	public class AbstractStat : ScriptableObject 
	{	
		//
		// Data
		//
		
		[SerializeField] private string statName;
		[SerializeField] private string description;
		[SerializeField] private Texture icon;
		//[SerializeField] private ProgressionType progressionVariable;	// CONSIDER INSTANCING
		[SerializeField] private AssignmentType xpAssignmentType;
		//[SerializeField] private int localXpPool;	// XP rewarded based on use-assignement scheme	// CONSIDER INSTANCING
		[SerializeField] private int absoluteMaxXpLevel;	// Cannot change
		
		//
		// Methods
		//
		
		
		//
		// Properties
		//
		
		public string StatName
		{
			get
			{
				return this.statName;
			}
		}
		
		public string Description
		{
			get
			{
				return this.description;
			}
		}
		
		public Texture Icon
		{
			get
			{
				return this.icon;
			}
		}
		
		public AssignmentType XpAssignmentType
		{
			get
			{
				return this.xpAssignmentType;
			}
		}
		
		public int AbsoluteMaxXpLevel
		{
			get
			{
				return this.absoluteMaxXpLevel;
			}
		}
	}
}
