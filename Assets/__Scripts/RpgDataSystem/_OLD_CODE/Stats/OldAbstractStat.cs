using UnityEngine;

namespace SphericalCow.OldCode
{
	abstract public class OldAbstractStat : ScriptableObject 
	{	
		//
		// Data
		//
		
		[SerializeField] private string statName;
		[TextArea(3,12)] [SerializeField] private string description;
		[SerializeField] private Texture icon;
		[SerializeField] private AssignmentType statPointAssignmentType = AssignmentType.DefaultAssigned;
		[SerializeField] private int absoluteMaxStatPoint;	// Cannot change
		
		//
		// Methods
		//

		abstract public SphericalCow.OldCode.StatType GetStatType();

		
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
		
		public AssignmentType StatPointAssignmentType
		{
			get
			{
				return this.statPointAssignmentType;
			}
		}
		
		public int AbsoluteMaxStatPoint
		{
			get
			{
				return this.absoluteMaxStatPoint;
			}
		}
	}
}
