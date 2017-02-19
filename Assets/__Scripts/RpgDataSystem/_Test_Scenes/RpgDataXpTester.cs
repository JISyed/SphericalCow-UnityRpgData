using UnityEngine;
using UI = UnityEngine.UI;
using Guid = System.Guid;

namespace SphericalCow.Testing
{
	/// <summary>
	/// 	Test script for XpProgressor and XpData
	/// </summary>
	public class RpgDataXpTester : MonoBehaviour 
	{
		[SerializeField] private string givenCharacterName;
		[SerializeField] private string xpProgressorName;
		[SerializeField] private int startingHealthPoints;
		[SerializeField] private int startingXp;
		
		private RpgCharacterData character;
		private XpProgressor xpProgressor;
		
		
		public UI.Text nameLabel;
		public UI.Text idLabel;
		public UI.Text hpLabel;
		public UI.Text maxHpLabel;
		public UI.Text xpLabel;
		public UI.Text xtnlLabel;
		public UI.Text levelLabel;
		public UI.Text difficultyLabel;
		public UI.Text equationLabel;
		
		public GameObject testXpPanel;
		public GameObject testHpPanel;
		
		
		
		// Use this for initialization
		void Start () 
		{
			Debug.Assert(string.IsNullOrEmpty(this.xpProgressorName) == false, 
			             "Please provide an proper XpProgressor in the Inspector");
			
			this.xpProgressor = RpgDataRegistry.Instance.SearchXpProgressor(this.xpProgressorName);
			
			Debug.Assert(this.xpProgressor != null, 
			             "Could not find an XpProgressor by the name " + this.xpProgressorName);
			
			this.character = new RpgCharacterData(Guid.NewGuid(),
			                                      this.xpProgressor, 
			                                      this.startingHealthPoints, 
			                                      this.startingHealthPoints,
			                                      RpgDataRegistry.Instance.DefaultStatPointAssignment, 
			                                      this.givenCharacterName);
			this.AddXp(this.startingXp);
			
			this.RefreshUI();
		}
		
		
		
		
		/// <summary>
		/// 	Give any amount of XP to the character. 
		/// 	The RPG character wasn't designed to loop through the 
		/// 	instnace of multiple levelups, so that's implemented here
		/// </summary>
		public void AddXp(int newXpToAdd)
		{
			// No negative parameters
			if(newXpToAdd < 0)
			{
				newXpToAdd = -newXpToAdd;
			}
			
			// Add the XP
			bool didLevelUp = false;
			didLevelUp = this.character.AddXp(newXpToAdd);
			
			// Check if the player leveled up. 
			// Checking in a loop because multiple levelups may be possible if too much XP was given at once
			while(didLevelUp)
			{
				Debug.Log("LEVELUP!       " + 
				          this.character.Name + 
				          " has now upgraded to Level " + 
				          this.character.Level.ToString());
				
				didLevelUp = this.character.AddXp(0);	// Don't add any more XP, but check for leveling up
			}
			
			this.RefreshUI();
		}
		
		
		
		/// <summary>
		/// 	Add more HP to the character. Cannot exceed the maximum HP of the character
		/// </summary>
		public void AddHp(int newHpToAdd)
		{
			// No negative parameters
			if(newHpToAdd < 0)
			{
				newHpToAdd = -newHpToAdd;
			}
			
			// Being revived means the Character had 0 HP before and then was given HP
			bool wasRevived = this.character.AddHp(newHpToAdd);
			
			if(wasRevived)
			{
				Debug.Log(this.character.Name + " was revived!");
			}
			
			this.RefreshUI();
		}
		
		/// <summary>
		/// 	Add more HP to the character. Cannot go lower than 0 HP
		/// </summary>
		public void RemoveHp(int newHpToRemove)
		{
			// No negative parameters
			if(newHpToRemove < 0)
			{
				newHpToRemove = -newHpToRemove;
			}
			
			// Character will be defeated only if he/she reaches 0 HP and having had some HP before
			bool wasDefeated = this.character.Hp != 0 && this.character.RemoveHp(newHpToRemove);
			
			if(wasDefeated)
			{
				Debug.Log(this.character.Name + " was defeated!");
			}
			
			this.RefreshUI();
		}
		
		
		/// <summary>
		/// 	Change how much addition there is to the character's maximum HP. Will re-adjust the HP if needed
		/// </summary>
		public void ChangeAdditionalMaxHp(int newAdditonalMaxHp)
		{
			// No negative parameters
			if(newAdditonalMaxHp < 0)
			{
				newAdditonalMaxHp = -newAdditonalMaxHp;
			}
			
			this.character.SetAdditonalMaxHp(newAdditonalMaxHp);
			
			this.RefreshUI();
		}
		
		
		
		
		/// <summary>
		///  	Used to refresh the Unity UI displaying data about the character.
		/// 	Doesn't have to run every frame
		/// </summary>
		private void RefreshUI()
		{
			this.nameLabel.text = this.character.Name;
			this.idLabel.text = this.character.Id.ToString();
			this.hpLabel.text = this.character.Hp.ToString();
			this.maxHpLabel.text = this.character.MaximumHp.ToString();
			this.xpLabel.text = this.character.Xp.ToString();
			this.xtnlLabel.text = this.character.XpToNextLevel.ToString();
			this.levelLabel.text = this.character.Level.ToString();
			this.difficultyLabel.text = this.character.XpProgressor.Name;
			
			string progressionEquationStr = string.Format("newXtnl = {0} * Level + {1} * oldXtnl", 
			                                              this.character.XpProgressor.LevelMultiplier,
			                                              this.character.XpProgressor.OldXtnlMultiplier);
			this.equationLabel.text = progressionEquationStr;
			
		}
		
		
		
		public void ToggleXpButtonPanel()
		{
			this.testXpPanel.SetActive(!this.testXpPanel.activeSelf);
		}
		
		
		public void ToggleHpButtonPanel()
		{
			this.testHpPanel.SetActive(!this.testHpPanel.activeSelf);
		}
		
		
	}
}
