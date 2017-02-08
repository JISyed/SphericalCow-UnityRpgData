using UnityEngine;
using System.Collections;

namespace SphericalCow.Testing
{
	/// <summary>
	/// 	Test script for XpProgressor and XpData
	/// </summary>
	public class RpgDataXpTester : MonoBehaviour 
	{
		[SerializeField] private string characterName;
		[SerializeField] private string xpProgressorName;
		[SerializeField] private int startingHealthPoints;
		
		private RpgCharacterData character;
		private XpProgressor xpProgressor;
		
		
		
		// Use this for initialization
		void Start () 
		{
			Debug.Assert(string.IsNullOrEmpty(this.xpProgressorName) == false, "Please provide an proper XpProgressor in the Inspector");
			this.xpProgressor = RpgDataRegistry.Instance.SearchXpProgressor(this.xpProgressorName);
			Debug.Assert(this.xpProgressor != null, "Could not find an XpProgressor by the name " + this.xpProgressorName);
			
			this.character = new RpgCharacterData(this.xpProgressor, 
			                                      this.startingHealthPoints, 
			                                      this.startingHealthPoints, 
			                                      this.characterName);
			
			Debug.Log("Name: " + this.character.Name);
			Debug.Log("ID: " + this.character.Id.ToString());
			Debug.Log("HP: " + this.character.Hp.ToString());
			Debug.Log("MaxHP: " + this.character.MaximumHp.ToString());
			Debug.Log("XP: " + this.character.Xp.ToString());
			Debug.Log("XpToNextLevel: " + this.character.XpToNextLevel.ToString());
			Debug.Log("Level: " + this.character.Level.ToString());
			Debug.Log("Difficulty: " + this.character.XpProgressor.Name);
			string progressionEquationStr = string.Format("newXTNL = {0} * currLevel + {1} * oldXTNL", 
			                                              this.character.XpProgressor.LevelMultiplier,
			                                              this.character.XpProgressor.OldXtnlMultiplier);
			Debug.Log("Difficulty Equation: " + progressionEquationStr);
		}
		
		// Update is called once per frame
		void Update () 
		{
			
		}
	}
}
