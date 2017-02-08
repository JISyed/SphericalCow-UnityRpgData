namespace SphericalCow
{
	/// <summary>
	/// 	Used to indicate whether all Ability modifications impose an UpperLimit (because of DecreaseTo),
	/// 	a LowerLimit (because of IncreaseTo), or NoLimit (from other values of AbiltyModifierType)
	/// </summary>
	public enum ModifierLimitType
	{
		/// <summary>
		/// 	In total, Abilities impose no limit and stat changes are relative
		/// </summary>
		NoLimit,
		
		/// <summary>
		/// 	In total, Abilities impose an upper limit on the SP of a stat because of AbilityModifierType.DecreaseTo
		/// </summary>
		UpperLimit,
		
		/// <summary>
		/// 	In total, Abilities impose an lower limit on the SP of a stat because of AbilityModifierType.IncreaseTo
		/// </summary>
		LowerLimit
	}
}
