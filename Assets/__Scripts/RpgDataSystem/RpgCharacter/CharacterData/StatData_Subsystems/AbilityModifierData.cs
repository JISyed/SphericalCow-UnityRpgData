using UnityEngine;
using Guid = System.Guid;

namespace SphericalCow
{
	/// <summary>
	/// 	Represents individual modifications done on a stat
	/// </summary>
	public class AbilityModifierData 
	{
		private SaveableGuid abilityId;
		private int modifierIndex;
		private AbilityModifier abilityModifier;
		
		
		/// <summary>
		/// 	Constructor needs an Ability and an AbilityModifier that is a part of that Ability
		/// </summary>
		public AbilityModifierData(AbilityModifier newModifier, Ability newAbility)
		{
			this.abilityId = new SaveableGuid(newAbility.Id);
			
			this.modifierIndex = -1;
			int indexTally = 0;
			foreach(AbilityModifier m in newAbility.StatModifiers)
			{
				if(ReferenceEquals(m, newModifier))
				{
					this.abilityModifier = m;
					this.modifierIndex = indexTally;
					break;
				}
				
				indexTally++;
			}
			
			Debug.Assert(this.modifierIndex > -1, 
			             "AbilityModifierData for the Ability " + newAbility.Name + " could not find the proper AbilityModifier!" );
			
		}
		
		
		
		
		/// <summary>
		/// 	The ID of this modifier's Ability
		/// </summary>
		public Guid AbilityId
		{
			get
			{
				return this.abilityId.GuidData;
			}
		}
		
		
		/// <summary>
		/// 	The array index of the referenced AbilityModifer inside its Ability
		/// </summary>
		public int ModifierIndex
		{
			get
			{
				return this.modifierIndex;
			}
		}
		
		
		/// <summary>
		/// 	The referenced AbilityModifier
		/// </summary>
		public AbilityModifier AbilityModifierReference
		{
			get
			{
				return this.abilityModifier;
			}
		}
		
		
	}
}
