using UnityEngine;
using System;

/// <summary>
/// 	A wrapper to C#'s Guid that can be serialized by Unity
/// </summary>
[Serializable]
public class SaveableGuid
{
	[NonSerialized] private Guid guid;
	[SerializeField] private string guidString = "";
	
	
	
	
	
	/// <summary>
	/// 	Creates an incomplete instance of SavableGuid (nesessary for loading)
	/// </summary>
	public SaveableGuid()
	{
	}
	
	/// <summary>
	/// 	Specify if you want to make a complete instance of SavableGuid
	/// </summary>
	public SaveableGuid(bool doRandomlyChooseGuid)
	{
		if (doRandomlyChooseGuid)
		{
			this.RandomizeGuid();
		}
	}
	
	
	/// <summary>
	/// 	Check if the Guid data is not empty
	/// </summary>
	public bool IsGuidDataValid()
	{
		return this.guid.Equals(Guid.Empty) == false;
	}
	
	
	/// <summary>
	/// 	Check if the Guid string representation is neither null nor empty
	/// </summary>
	public bool IsGuidStringValid()
	{
		return string.IsNullOrEmpty(this.guidString) == false;
	}
	
	
	/// <summary>
	/// 	Use with caution! Will give a new random GUID. May mess up existing serialized data!
	/// </summary>
	public void RepairGuid()
	{
		if(string.IsNullOrEmpty(this.guidString))
		{
			this.RandomizeGuid();
		}
	}
	
	
	/// <summary>
	/// 	Initialize the Guid data from the string representation. The string representation must exist!
	/// </summary>
	public void LoadInternalData()
	{
		Debug.Assert(this.IsGuidStringValid(), "An RPG data object has a GUID with an invalid string representation!");
		
		this.guid = new Guid(this.guidString);
		this.UpdateStringRepresentation();
	}
	
	
	
	/// <summary>
	///  The GUID as a String
	/// </summary>
	public string GuidString
	{
		get
		{
			return this.guidString;
		}
	}
	
	/// <summary>
	/// 	The GUID as a Guid object
	/// </summary>
	public Guid GuidData
	{
		get
		{
			return this.guid;
		}
	}
	
	
	
	
	private void RandomizeGuid()
	{
		this.guid = Guid.NewGuid();
		this.UpdateStringRepresentation();
	}
	
	
	
	private void UpdateStringRepresentation()
	{
		Debug.Assert(this.IsGuidDataValid(), "An RPG data object has an invalid (empty) GUID!");
		
		this.guidString = this.guid.ToString();
	}
	
}
