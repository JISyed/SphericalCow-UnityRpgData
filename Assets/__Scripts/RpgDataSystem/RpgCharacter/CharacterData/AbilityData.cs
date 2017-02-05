using UnityEngine;
using Guid = System.Guid;

namespace SphericalCow
{
	/// <summary>
	/// 	Stores a reference to a single Ability for RpgCharacterData
	/// </summary>
	[System.Serializable]
	public class AbilityData 
	{
		[SerializeField] private SaveableGuid id;
		[System.NonSerialized] private Ability ability;
		
		
		/// <summary>
		/// 	Constructor needs an Ability
		/// </summary>
		public AbilityData(Ability newAbility)
		{
			this.ability = newAbility;
			this.id = new SaveableGuid(newAbility.Id);
		}
		
		
		/// <summary>
		/// 	Find the correct Ability instance given its GUID. Used for deserialzation.
		/// </summary>
		public void LoadAbility()
		{
			// TODO: Implement by putting Abilities in a registry and use id
		}
		
		
		
		
		
		/// <summary>
		/// 	The ID of the referenced Ability
		/// </summary>
		public Guid Id
		{
			get
			{
				return this.id.GuidData;
			}
		}
		
		
		/// <summary>
		///		The referenced Ability
		/// </summary>
		public Ability AbilityReference
		{
			get
			{
				return this.ability;
			}
		}
		
		
	}
}
