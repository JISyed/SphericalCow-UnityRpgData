using UnityEngine;

namespace SphericalCow
{
	/// <summary>
	/// 	Utility class that is responsible for saving and loading RpgCharacterData instances from file
	/// </summary>
	public static class RpgCharacterSerializer
	{
		/// <summary>
		/// 	Save the RpgCharacter to file. The file will be named based off the character's name and GUID
		/// </summary>
		public static void SaveCharacter(RpgCharacterData character)
		{
			if(character == null)
			{
				Debug.LogError("Cannot save a null RpgCharacter!");
				return;
			}
			
			
		}
		
		public static RpgCharacterData LoadCharacter(string characterName, string characterId)
		{
			if(string.IsNullOrEmpty(characterName))
			{
				Debug.LogError("Cannot load an RpgCharacter with a blank name!");
				return null;
			}
			if(string.IsNullOrEmpty(characterId))
			{
				Debug.LogError("Cannot load an RpgCharacter with a blank GUID!");
				return null;
			}
			
			
			
			
			
			return null;
		}
	}
}