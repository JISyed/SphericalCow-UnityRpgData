using UnityEngine;

namespace SphericalCow
{
	abstract public class AbstractStat : ScriptableObject 
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

		abstract public StatType GetStatType();

		
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
