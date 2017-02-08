using UnityEngine;
using Guid = System.Guid;

namespace SphericalCow
{
	[System.Serializable]
	public class AbilityModifierData 
	{
		[SerializeField] private SaveableGuid abilityId;
		[SerializeField] private int modifierIndex;
		[System.NonSerialized] private AbilityModifier abilityModifier;
		
		
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
		/// 	Find the correct AbilityModifier instance given its Ability's GUID. Used for deserialzation.
		/// </summary>
		public void LoadAbilityModifier()
		{
			this.abilityId.LoadInternalData();
			
			Ability ability = RpgDataRegistry.Instance.SearchAbility(this.abilityId.GuidData);
			Debug.Assert(ability != null, "AbilityModifierData failed to find the Ability of ID " + this.abilityId.GuidString);
			
			int indexTalley = 0;
			foreach(AbilityModifier m in ability.StatModifiers)
			{
				if(indexTalley == this.modifierIndex)
				{
					this.abilityModifier = m;
					break;
				}
				
				indexTalley++;
			}
			
			Debug.Assert(this.abilityModifier != null, "AbilityModifierData failed to find the AbilityModifier of Ability "
			             + ability.Name + " for Modifier index " + this.modifierIndex.ToString());
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
