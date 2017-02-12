namespace SphericalCow
{
	/// <summary>
	/// 	Determines how a stat will allocate or assign SP when new SP is gained.
	/// 	Designated per player, not per stat
	/// </summary>
	public enum GlobalSpAssignmentType 
	{
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