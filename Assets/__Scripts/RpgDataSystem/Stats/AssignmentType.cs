namespace SphericalCow
{
	public enum AssignmentType 
	{
		PointAssigned,		// Uses a global pool that is user-allocated for all stats
		UseAssigned,		// Uses a stat-local pool per stat that is computer-allocated
		DefaultAssigned		// Uses whatever the global assignment scheme is (hint: the global scheme cannot be default!)
	}
}
	