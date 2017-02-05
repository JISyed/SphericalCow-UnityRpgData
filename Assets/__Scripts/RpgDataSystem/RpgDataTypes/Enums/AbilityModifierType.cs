namespace SphericalCow
{
	/// <summary>
	/// 	Describes the way abilities can modify stat points as long as the ability is applied.
	/// </summary>
	public enum AbilityModifierType 
	{
		/// <summary>
		/// 	Increase the stat point by a certain value
		/// </summary>
		IncreaseBy,
		
		/// <summary>
		/// 	Increase the stat point to a certain value if the current value is less than the target value
		/// </summary>
		IncreaseTo,
		
		/// <summary>
		/// 	Decrease the stat point by a certain value
		/// </summary>
		DecreaseBy,
		
		/// <summary>
		/// 	Decrease the stat point to a certain value if the current value is greater than the target value
		/// </summary>
		DecreaseTo
	}
}
