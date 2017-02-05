using UnityEngine;
using Guid = System.Guid;

namespace SphericalCow
{
	/// <summary>
	/// 	Represents any read-only RPG data type that has a name, decription, and icon
	/// </summary>
	public abstract class AbstractRpgDataType : ScriptableObject 
	{
		[SerializeField] private string entityName;
		[TextArea(3,12)] [SerializeField] private string description;
		[SerializeField] private Texture icon;
		[ReadOnly, SerializeField] private SaveableGuid id;		// Do not rename; renaming will cause problems
		[ReadOnly, SerializeField] private bool isGuidInitialized = false;
		
		
		/// <summary>
		/// 	Initialized the RPG Data Type. Only runs once.
		/// </summary>
		public void Init()
		{
			if(!this.isGuidInitialized)
			{
				this.id = new SaveableGuid(true);
				this.isGuidInitialized = true;
			}
		}
		
		
		/// <summary>
		/// 	Used for deserialization of this object's ID (Guids cannot be serialized natively)
		/// </summary>
		public void RestoreGuidData()
		{
			this.id.LoadInternalData();
		}
		
		
		
		/// <summary>
		/// 	The name of this RPG Data Instance
		/// </summary>
		public string Name
		{
			get
			{
				return this.entityName;
			}
		}
		
		/// <summary>
		/// 	The description of this RPG Data Instance
		/// </summary>
		public string Description
		{
			get
			{
				return this.description;
			}
		}
		
		/// <summary>
		/// 	The icon texture of this RPG Data Instance
		/// </summary>
		public Texture Icon
		{
			get
			{
				return this.icon;
			}
		}
		
		/// <summary>
		/// 	The unique GUID of this RPG Data Instance. Used for comparisons.
		/// </summary>
		public Guid Id
		{
			get
			{
				return this.id.GuidData;
			}
		}
		
		
		/// <summary>
		/// 	Is the GUID initialized?
		/// </summary>
		protected bool IsGuidInitialized
		{
			get
			{
				return this.isGuidInitialized;
			}
		}
		
	}
}
