using UnityEngine;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SphericalCow
{
	/// <summary>
	/// 	Represents a collection of modifications applied onto select Stats, enhancing some and weakening others
	/// </summary>
	public class Ability : AbstractRpgDataType 
	{
		[SerializeField] private List<AbilityModifier> statModifiers = new List<AbilityModifier>();
		private ReadOnlyCollection<AbilityModifier> readonlyList = null;
		//[ReadOnly, SerializeField] private bool areModifierGuidsInitialized = false;
		
		
//		/// <summary>
//		/// 	Initialized the RPG Data Type. Only runs once. 
//		/// 	For Abilities, also intialized the GUIDs of the AbilityModifiers
//		/// </summary>
//		public override void Init ()
//		{
//			if(!this.IsGuidInitialized)
//			{
//				foreach(AbilityModifier m in this.statModifiers)
//				{
//					m.Init();
//				}
//			}
//			
//			// "base.Init()" will switch the IsGuidInitialzed flag once automatically
//			base.Init();
//		}
		
		
		
//		void OnEnable()
//		{
//			//if(!this.areModifierGuidsInitialized)
//			//{
//				foreach(AbilityModifier m in this.statModifiers)
//				{
//					m.Init();
//				}
//				
//				//this.areModifierGuidsInitialized = true;
//			//}
//		}
		
		
		
		
		/// <summary>
		/// 	The list of AbilityModifiers linked to this Ability
		/// </summary>
		public ReadOnlyCollection<AbilityModifier> StatModifiers
		{
			get
			{
				if(this.readonlyList == null)
				{
					this.readonlyList = this.statModifiers.AsReadOnly();
				}
				
				return this.readonlyList;
			}
		}
		
	}
}
