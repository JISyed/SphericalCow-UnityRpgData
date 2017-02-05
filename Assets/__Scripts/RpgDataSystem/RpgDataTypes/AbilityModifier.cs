﻿using UnityEngine;
using Guid = System.Guid;

namespace SphericalCow
{
	/// <summary>
	/// 	A unit of modification in an Ability that alters one Stat
	/// </summary>
	[System.Serializable]
	public class AbilityModifier
	{
		[SerializeField] private AbstractStat modifiedStat;
		[SerializeField] private AbilityModifierType type;
		[SerializeField] private int assignedModifierValue;
		//[ReadOnly, SerializeField] private SaveableGuid id;		// Do not rename; renaming will cause problems
		//[ReadOnly, SerializeField] private bool isIdInitialized = false;
		
		
		
		/// <summary>
		/// 	AbilityModifier's constructor
		/// </summary>
		public AbilityModifier()
		{
			//this.id = new SaveableGuid(true);
		}
		
		
		/// <summary>
		/// 	Used to initialize the GUID. Only runs once
		/// </summary>
//		public void Init()
//		{
//			if(!this.isIdInitialized)
//			{
//				this.id = new SaveableGuid(true);
//				this.isIdInitialized = true;
//			}
//		}
		
		
		
		/// <summary>
		/// 	Used for deserialization of this object's ID (Guids cannot be serialized natively)
		/// </summary>
//		public void RestoreGuidData()
//		{
//			if(this.id.IsGuidDataValid())
//			{
//				this.id.LoadInternalData();
//			}
//		}
		
		
		
		/// <summary>
		/// 	The Stat that is to be modified by this AbilityModifier
		/// </summary>
		public AbstractStat ModifiedStat
		{
			get
			{
				return this.modifiedStat;
			}
		}
		
		
		/// <summary>
		/// 	The type of AbilityModifier. Can be IncreaseTo, IncreaseBy, DecreaseTo, or DecreaseBy
		/// </summary>
		public AbilityModifierType Type
		{
			get
			{
				return this.type;
			}
		}
		
		
		/// <summary>
		/// 	The amount of SP to be modified in a Stat. Depending on the type, this can also be an SP extrema (min/max)
		/// </summary>
		public int ModifierValue
		{
			get
			{
				return this.assignedModifierValue;
			}
		}
		
		
		/// <summary>
		/// 	The ID of this AbilityModifier
		/// </summary>
//		public Guid Id
//		{
//			get
//			{
//				return this.id.GuidData;
//			}
//		}
		
	}
}
