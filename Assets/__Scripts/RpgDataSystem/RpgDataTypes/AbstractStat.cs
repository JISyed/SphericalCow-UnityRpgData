using UnityEngine;
using System.Collections;

namespace SphericalCow
{
	/// <summary>
	/// 	Base class for any RPG stat
	/// </summary>
	public abstract class AbstractStat : AbstractRpgDataType 
	{
		[SerializeField] private SpAssignmentType spAssignmentType = SpAssignmentType.Default;
		[SerializeField] private int absoluteMaximumSp = 0;
		
		
		/// <summary>
		/// 	Returns the what kind of stat this object is. [Base, Secondary, or Skill]
		/// </summary>
		abstract public StatType GetStatType();
		
		
		/// <summary>
		/// 	Returns the assignment method of SP for this stat
		/// </summary>
		public SpAssignmentType SpAssignment
		{
			get
			{
				return this.spAssignmentType;
			}
		}
		
		
		/// <summary>
		/// 	Returns the absolute number of SP that can accumulate for this stat.
		/// 	If set to 0, there is no max
		/// </summary>
		public int AbsoluteMaximumSp
		{
			get
			{
				return this.absoluteMaximumSp;
			}
		}
		
	}
}
