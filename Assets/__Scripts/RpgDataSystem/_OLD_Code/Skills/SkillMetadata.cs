using UnityEngine;

namespace SphericalCow.Old
{
	/// <summary>
	/// 	Metadata of a skill that can only be set by the developer and not by the game itself
	/// </summary>
	public class SkillMetadata : ScriptableObject 
	{
		[SerializeField] private string nameOfSkill;
		[SerializeField] private int defaultLevel;
		[SerializeField] private SkillAttribute attribute;
		
		public string NameOfSkill
		{
			get
			{
				return this.nameOfSkill;
			}
		}
		
		public int DefaultLevel
		{
			get
			{
				return this.defaultLevel;
			}
		}
		
		public SkillAttribute Attribute
		{
			get
			{
				return this.attribute;
			}
		}
	}
}
