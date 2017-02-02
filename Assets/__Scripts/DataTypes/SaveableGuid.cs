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
	
	private void RandomizeGuid()
	{
		this.guid = Guid.NewGuid();
		this.UpdateStringRepresentation();
	}
	
	private void UpdateStringRepresentation()
	{
		this.guidString = this.guid.ToString();
	}
	
	/// <summary>
	/// 	Initialize the Guid data from the string representation. The string representation must exist!
	/// </summary>
	public void LoadInternalData()
	{
		this.guid = new Guid(this.guidString);
		this.UpdateStringRepresentation();
	}
	
	/// <summary>
	///  Get the GUID as a String
	/// </summary>
	public string GuidString
	{
		get
		{
			return this.guidString;
		}
	}
	
	/// <summary>
	/// 	Get the GUID as a Guid object
	/// </summary>
	public Guid GuidData
	{
		get
		{
			return this.guid;
		}
	}
	
}
