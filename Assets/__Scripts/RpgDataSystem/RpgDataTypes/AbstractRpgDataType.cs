using UnityEngine;
using Guid = System.Guid;

namespace SphericalCow
{
	/// <summary>
	/// 	Represents any read-only RPG data type that has a name, decription, and icon
	/// </summary>
	public class AbstractRpgDataType : ScriptableObject 
	{
		[SerializeField] private string entityName;
		[TextArea(3,12)] [SerializeField] private string description;
		[SerializeField] private Texture icon;
		[ReadOnly, SerializeField] private SaveableGuid id;		// Do not rename; renaming will cause problems
		[ReadOnly, SerializeField] private bool isGuidInitialized = false;
		
		
		void OnEnable()
		{
			if(string.IsNullOrEmpty(this.id.GuidString))
			{
				this.id.RepairGuid();
				Debug.LogWarning("The ID for " + this.entityName 
				                 + " went blank and had to be recreated! YAML data files may have to be fixed!");
			}
		}
		
		
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
		
	}
}
