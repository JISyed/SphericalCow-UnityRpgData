using UnityEngine;

namespace SphericalCow.Old
{
	/// <summary>
	/// 	Represents an individual skill the the player might have
	/// </summary>
	public class Skill 
	{
		[System.NonSerialized] private SkillMetadata data;
		private string nameOfSkill;
		private int level;
		
		public Skill()
		{
			// Intentionally empty
		}
		
		//
		// Methods
		//
		
		public Skill(SkillMetadata metadata)
		{
			this.data = metadata;
			this.nameOfSkill = metadata.NameOfSkill;
			this.level = metadata.DefaultLevel;
		}
		
		public Skill(SkillMetadata metadata, int lastKnownLevel)
		{
			this.data = metadata;
			this.nameOfSkill = metadata.NameOfSkill;
			this.level = lastKnownLevel;
		}
		
		
		//
		// Accessors
		//
		
		public SkillMetadata Data
		{
			get
			{
				return this.data;
			}
		}
		
		public string NameOfSkill
		{
			get
			{
				return this.nameOfSkill;
			}
		}
		
		public int Level
		{
			get
			{
				return this.level;
			}
		}
		
		
		
	}
}
	