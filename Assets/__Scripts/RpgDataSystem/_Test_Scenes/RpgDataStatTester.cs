using UnityEngine;
using UI = UnityEngine.UI;
using Guid = System.Guid;
using System.Text;	// For StringBuilder

namespace SphericalCow.Testing
{
	/// <summary>
	/// 	Test script for AbstractStat, BaseStat, SecondaryStat, SkillStat, and StatData (with subsystems)
	/// </summary>
	public class RpgDataStatTester : MonoBehaviour 
	{
		//
		// Data
		//
		
		[SerializeField] private string givenCharacterName;
		[SerializeField] private XpProgressor xpProgressor;
		[SerializeField] private int startingHealthPoints;
		[SerializeField] private int startingXp;
		[SerializeField] private bool showStatAndAbilityIds;
		
		private RpgCharacterData character;
		
		private StringBuilder stringBuilder;
		
		public UI.Text nameLabel;
		public UI.Text idLabel;
		public UI.Text hpLabel;
		public UI.Text maxHpLabel;
		public UI.Text xpLabel;
		public UI.Text xtnlLabel;
		public UI.Text levelLabel;
		public UI.Text difficultyLabel;
		public UI.Text equationLabel;
		public UI.Text statsLabel;
		public UI.Text globalSpLabel;
		public UI.Text spAllocLabel;
		
		public GameObject testXpPanel;
		public GameObject testHpPanel;
		public GameObject testStatPanel;
		public GameObject testPointAllocSpPanel;
		public GameObject testUseAllocSpPanel;
		
		
		
		
		//
		// Unity Events
		//
		
		
		// Use this for initialization
		void Start () 
		{
			this.stringBuilder = new StringBuilder();
			
			Debug.Assert(this.xpProgressor != null, "Could not assign a null XpProgressor");
			
			this.character = new RpgCharacterData(Guid.NewGuid(),
			                                      this.xpProgressor, 
			                                      this.startingHealthPoints, 
			                                      this.startingHealthPoints,
			                                      RpgDataRegistry.Instance.DefaultStatPointAssignment, 
			                                      this.givenCharacterName);
			
			
			this.AddXp(this.startingXp);
			
			this.RefreshUI();
		}
		
		
		
		
		
		
		//
		// Routines
		//
		
		
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
			
			this.globalSpLabel.text = this.character.UnallocatedSp.ToString();
			this.spAllocLabel.text = this.character.StatPointAssignmentType.ToString();
			
			
			// Print stats
			
			foreach(StatData statData in this.character.AppliedStats)
			{
				this.stringBuilder.Append(statData.Name).Append("\n");
				if(this.showStatAndAbilityIds)
				{
					this.stringBuilder.Append("      ID: ").Append(statData.Id.ToString()).Append("\n");
				}
				this.stringBuilder.Append("      Type: ").Append(statData.Type.ToString()).Append("\n");
				this.stringBuilder.Append("      Raw SP: ").Append(statData.RawStatPoints).Append("\n");
				this.stringBuilder.Append("      Final SP: ").Append(statData.StatPoints);
				if(statData.StatReference.AbsoluteMaximumSp != 0)
				{
					this.stringBuilder.Append(" / ").Append(statData.StatReference.AbsoluteMaximumSp).Append("\n");
				}
				else
				{
					this.stringBuilder.Append("\n");
				}
				this.stringBuilder.Append("      Use Factor: ").Append(statData.UseFactor).Append("\n");
				this.stringBuilder.Append("\n\n");
			}
			this.statsLabel.text = this.stringBuilder.ToString();
			this.stringBuilder.Length = 0;
			
			if(this.character.AppliedStats.Count == 0)
			{
				this.statsLabel.text = "No stats applied...";
			}
			
			
			
		}
		
		
		
		
		
		
		//
		// Buttons
		//
		
		
		
		
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
		/// 	Add a new stat to the RpgCharacter. Needs the name of the stat from the data assets (not filename!)
		/// </summary>
		public void AddStat(string statName)
		{
			AbstractStat newStat = RpgDataRegistry.Instance.SearchAnyStat(statName);
			
			Debug.Assert(newStat != null, "Trying to add a stat, but couldn't find the stat \"" + statName + "\"");
			
			this.character.AddStat(newStat);
			
			this.RefreshUI();
		}
		
		
		
		/// <summary>
		/// 	Remove a stat to the RpgCharacter. Needs the name of the stat from the data assets (not filename!)
		/// </summary>
		public void RemoveStat(string statName)
		{
			this.character.RemoveStat(statName);
			
			this.RefreshUI();
		}
		
		
		
		
		/// <summary>
		/// 	Adds SP from the Character's global SP pool into a particular stat, given its name
		/// </summary>
		/// <param name="spToAdd">Amount of SP to add to the given stat</param>
		/// <param name="statName">The name of the stat to add SP into, if it exists (not filename)</param>
		public void AddSpFromGlobalPool(int spToAdd, string statName)
		{
			bool statExists = this.character.AddStatPointsFromUnallocatedPool(spToAdd, statName);
			
			if(!statExists)
			{
				Debug.LogError("Could not add SP from global pool because stat " + statName + " does not exist!");
			}
			
			this.RefreshUI();
		}
		
		
		public void AddSpFromGlobalPoolToStatAgility()
		{
			this.AddSpFromGlobalPool(1, "Agility");
		}
		
		public void AddSpFromGlobalPoolToStatStrength()
		{
			this.AddSpFromGlobalPool(1, "Strength");
		}
		
		public void AddSpFromGlobalPoolToStatHealth()
		{
			this.AddSpFromGlobalPool(1, "Health");
		}
		
		public void AddSpFromGlobalPoolToStatKatana()
		{
			this.AddSpFromGlobalPool(1, "Katana");
		}
		
		
		
		/// <summary>
		/// 	Tells the RPG system that the given stat was used. Used for UseAssigned SP calculations.
		/// 	In game logic, marking a stat as used is highly recommended whenever a stat is "used" somehow.
		/// </summary>
		/// <param name="statName">Stat name.</param>
		public void MarkSomeStatAsUsed(string statName)
		{
			bool statExists = this.character.MarkStatAsUsed(statName);
			if(!statExists)
			{
				Debug.LogError("Could not mark stat " + statName + " as used because it does not exist!");
			}
			
			this.RefreshUI();
		}
		
		
		
		
		
		
		/// <summary>
		/// 	Toggles the XP test panel.
		/// </summary>
		public void ToggleXpButtonPanel()
		{
			this.testXpPanel.SetActive(!this.testXpPanel.activeSelf);
		}
		
		
		/// <summary>
		/// 	Toggles the HP test panel.
		/// </summary>
		public void ToggleHpButtonPanel()
		{
			this.testHpPanel.SetActive(!this.testHpPanel.activeSelf);
		}
		
		
		/// <summary>
		/// 	Toggles the Stat test panel.
		/// </summary>
		public void ToggleStatButtonPanel()
		{
			this.testStatPanel.SetActive(!this.testStatPanel.activeSelf);
		}
		
		
		/// <summary>
		/// 	Toggles the Point Allocation test panel.
		/// </summary>
		public void TogglePointAllocationButtonPanel()
		{
			this.testPointAllocSpPanel.SetActive(!this.testPointAllocSpPanel.activeSelf);
		}
		
		
		/// <summary>
		/// 	Toggles the Use Allocation test panel.
		/// </summary>
		public void ToggleUseAllocationButtonPanel()
		{
			this.testUseAllocSpPanel.SetActive(!this.testUseAllocSpPanel.activeSelf);
		}
		
	}
}
