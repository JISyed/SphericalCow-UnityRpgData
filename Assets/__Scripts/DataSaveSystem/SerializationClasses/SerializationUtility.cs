using UnityEngine;
using System.Collections;

namespace SphericalCow
{
	/// <summary>
	/// 	A utility class used to help serialize data used in a game
	/// </summary>
	public static class SerializationUtility
	{
		//
		// Static Data
		//
		
		/// <summary>
		/// 	The file extension for anything saved with this serializer.
		/// 	Does NOT include the dot in the beginning.
		/// </summary>
		public const string EXTENSION = "scdata";
	}
}