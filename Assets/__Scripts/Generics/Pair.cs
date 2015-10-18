// Attributed to:
// http://stackoverflow.com/a/166094

namespace SphericalCow.Generics
{
	public class Pair<T1, T2> 
	{
		public Pair() 
		{
		}
		
		public Pair(T1 first, T2 second) 
		{
			this.First = first;
			this.Second = second;
		}

		// Auto-Properties
		public T1 First { get; set; }
		public T2 Second { get; set; }
	}
}
