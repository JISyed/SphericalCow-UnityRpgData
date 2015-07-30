using UnityEngine;

namespace SphericalCow
{
	/// <summary>
	/// 	Metadata of a stat that can only be set by the developer and not by the game itself
	/// </summary>
	public class StatMetadata : ScriptableObject 
	{
		[SerializeField] private string nameOfStat;
		[SerializeField] [TextArea] private string description;
		[SerializeField] private Texture identifyingImage;
		[SerializeField] private int progressionRate;
		
		public string NameOfStat
		{
			get
			{
				return this.nameOfStat;
			}
		}
		
		public string Description
		{
			get
			{
				return this.description;
			}
		}
		
		public Texture IdentifyingImage
		{
			get
			{
				return this.identifyingImage;
			}
		}
		
		public int ProgressionRate
		{
			get
			{
				return this.progressionRate;
			}
		}
		
	}
}
