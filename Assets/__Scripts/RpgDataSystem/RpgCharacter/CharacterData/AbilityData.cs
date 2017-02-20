using UnityEngine;
using Guid = System.Guid;

namespace SphericalCow
{
	/// <summary>
	/// 	Stores a reference to a single Ability for RpgCharacterData
	/// </summary>
	public class AbilityData 
	{
		private SaveableGuid id;
		private Ability ability;
		
		
		/// <summary>
		/// 	Constructor needs an Ability
		/// </summary>
		public AbilityData(Ability newAbility)
		{
			this.ability = newAbility;
			this.id = new SaveableGuid(newAbility.Id);
		}
		
		
		/// <summary>
		/// 	Deserialization Constructor
		/// </summary>
		public AbilityData(AbilityPacket abilityPacket)
		{
			this.id = new SaveableGuid(abilityPacket.abilityId);
			this.ability = RpgDataRegistry.Instance.SearchAbility(this.id.GuidData);
			
			Debug.Assert(this.ability != null, "An AbilityData instance could not be serialized because the definition file was not found! ID: " + abilityPacket.abilityId);
		}
		
		
		
		
		/// <summary>
		/// 	Only to be called by the RpgCharacterSerializer
		/// </summary>
		public AbilityPacket ExportSerializationPacket()
		{
			AbilityPacket newPacket = new AbilityPacket();
			newPacket.abilityId = this.id.GuidString;
			
			return newPacket;
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
