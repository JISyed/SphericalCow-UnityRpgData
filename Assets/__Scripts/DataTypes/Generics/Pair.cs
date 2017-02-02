// Attributed to:
// http://stackoverflow.com/a/166094

namespace SphericalCow.Generics
{
	/// <summary>
	/// 	Immutable pair
	/// </summary>
	public sealed class Pair<T1, T2> 
	{
		private Pair() 
		{
		}
		
		public Pair(T1 first, T2 second) 
		{
			this.First = first;
			this.Second = second;
		}

		// Auto-Properties
		public T1 First { get; private set; }
		public T2 Second { get; private set; }
	}

	/// <summary>
	/// 	Mutable pair
	/// </summary>
	public sealed class MutablePair<T1, T2>
	{
		public MutablePair() 
		{
		}
		
		public MutablePair(T1 first, T2 second) 
		{
			this.First = first;
			this.Second = second;
		}
		
		// Auto-Properties
		public T1 First { get; set; }
		public T2 Second { get; set; }
	}
}
