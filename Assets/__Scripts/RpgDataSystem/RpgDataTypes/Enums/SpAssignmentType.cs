namespace SphericalCow
{
	/// <summary>
	/// 	Determines how a stat will allocate or assign SP when new SP is gained
	/// </summary>
	public enum SpAssignmentType 
	{
		/// <summary>
		/// 	Use the default allocation method determined by global definition
		/// </summary>
		Default,
		
		/// <summary>
		/// 	SP is allocated based on whether a stat is used or not
		/// </summary>
		UseAssigned,
		
		/// <summary>
		/// 	SP is manually allocated by the player from a global point pool
		/// </summary>
		PointAssigned
	}	
}