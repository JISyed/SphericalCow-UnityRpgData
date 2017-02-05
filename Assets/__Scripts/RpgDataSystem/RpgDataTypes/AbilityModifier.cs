using UnityEngine;
using Guid = System.Guid;

namespace SphericalCow
{
	/// <summary>
	/// 	A unit of modification in an Ability that alters one Stat
	/// </summary>
	[System.Serializable]
	public class AbilityModifier
	{
		[SerializeField] private AbstractStat modifiedStat;
		[SerializeField] private AbilityModifierType type;
		[SerializeField] private int assignedModifierValue;
		
		
		
		/// <summary>
		/// 	The Stat that is to be modified by this AbilityModifier
		/// </summary>
		public AbstractStat ModifiedStat
		{
			get
			{
				return this.modifiedStat;
			}
		}
		
		
		/// <summary>
		/// 	The type of AbilityModifier. Can be IncreaseTo, IncreaseBy, DecreaseTo, or DecreaseBy
		/// </summary>
		public AbilityModifierType Type
		{
			get
			{
				return this.type;
			}
		}
		
		
		/// <summary>
		/// 	The amount of SP to be modified in a Stat. Depending on the type, this can also be an SP extrema (min/max)
		/// </summary>
		public int ModifierValue
		{
			get
			{
				return this.assignedModifierValue;
			}
		}
		
	}
}
