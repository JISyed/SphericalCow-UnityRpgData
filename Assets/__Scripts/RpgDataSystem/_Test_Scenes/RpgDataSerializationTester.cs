using UnityEngine;
using UI = UnityEngine.UI;
using Guid = System.Guid;
using System.Text;	// For StringBuilder
using System.Collections.Generic;

namespace SphericalCow.Testing
{
	/// <summary>
	/// 	Test script for serliazation of RpgCharacterData and its subsystems
	/// </summary>
	public class RpgDataSerializationTester : MonoBehaviour 
	{
		//
		// Data
		//
		
		[SerializeField] private bool loadSlot1OnStart;
		[SerializeField] private string givenCharacterName;
		[SerializeField] private string givenGuid;
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
		public UI.Text abilitiesLabel;
		
		public GameObject testXpPanel;
		public GameObject testHpPanel;
		public GameObject testStatPanel;
		public GameObject testPointAllocSpPanel;
		public GameObject testUseAllocSpPanel;
		public GameObject testAbilityPanel;
		public GameObject testSerializationPanel;
		
		
		public string[] randomNames;
		public string[] randomGuids;
		
		
		
		//
		// Unity Events
		//
		
		
		// Use this for initialization
		void Start () 
		{
			this.stringBuilder = new StringBuilder();
			
			Debug.Assert(this.xpProgressor != null, "Could not assign a null XpProgressor");
			
			if(this.loadSlot1OnStart)
			{
				bool slot1Loaded = this.LoadCharacterFromFileWithCheck(1);
				
				if(!slot1Loaded)
				{
					Debug.Log("Since Slot 1 was empty, a new character will be made instead");
					this.character = new RpgCharacterData(new Guid(this.givenGuid),
					                                      this.xpProgressor, 
					                                      this.startingHealthPoints, 
					                                      this.startingHealthPoints,
					                                      RpgDataRegistry.Instance.DefaultStatPointAssignment, 
					                                      this.givenCharacterName);
					this.AddXp(this.startingXp);
				}
			}
			else
			{
				this.character = new RpgCharacterData(new Guid(this.givenGuid),
				                                      this.xpProgressor, 
				                                      this.startingHealthPoints, 
				                                      this.startingHealthPoints,
				                                      RpgDataRegistry.Instance.DefaultStatPointAssignment, 
				                                      this.givenCharacterName);
				this.AddXp(this.startingXp);
			}
			
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
			
			
			
			// Print abilities
			
			foreach(AbilityData abilityData in this.character.AppliedAbilities)
			{
				this.stringBuilder.Append(abilityData.AbilityReference.Name).Append("\n");
				if(this.showStatAndAbilityIds)
				{
					this.stringBuilder.Append("      ID: ").Append(abilityData.Id.ToString()).Append("\n");
				}
				
				this.stringBuilder.Append("      Modifiers: ").Append("\n");
				
				var abilityModiferList = abilityData.AbilityReference.StatModifiers;
				foreach(AbilityModifier modifier in abilityModiferList)
				{
					this.stringBuilder.Append("            Modified Stat:      ").Append(modifier.ModifiedStat.Name).Append("\n");
					this.stringBuilder.Append("            Modification Type:  ").Append(modifier.Type.ToString()).Append("\n");
					this.stringBuilder.Append("            Modificaiton Value: ").Append(modifier.ModifierValue.ToString()).Append("\n\n");
				}
			}
			this.abilitiesLabel.text = this.stringBuilder.ToString();
			this.stringBuilder.Length = 0;
			
			
			if(this.character.AppliedAbilities.Count == 0)
			{
				this.abilitiesLabel.text = "No abilities applied...";
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
		/// 	Add a new ability to the RpgCharacter. Needs the name of the ability from the data assets (not filename!)
		/// </summary>
		public void AddAbility(string abilityName)
		{
			Ability newAbility = RpgDataRegistry.Instance.SearchAbility(abilityName);
			
			Debug.Assert(newAbility != null, "Trying to add an ability, but couldn't find the ability \"" + abilityName + "\"");
			
			this.character.AddAbility(newAbility);
			
			this.RefreshUI();
		}
		
		
		/// <summary>
		/// 	Add a new ability to the RpgCharacter. Needs the name of the ability from the data assets (not filename!)
		/// </summary>
		public void RemoveAbility(string abilityName)
		{
			this.character.RemoveAbility(abilityName);
			
			this.RefreshUI();
		}
		
		
		
		/// <summary>
		/// 	Records the player's name and id to the chosen slot then saves the character to file
		/// </summary>
		public void SaveCharacterToFile(int slotNumber)
		{
			SaveSlot slot = SaveSlotRegistry.Instance.GetSlotAt(slotNumber);
			Debug.Assert(slot != null, "Invalid SaveSlot");
			
			slot.isSlotOccupied = true;
			slot.playerName = this.character.Name;
			slot.playerId = this.character.Id.ToString();
			SaveSlotRegistry.Instance.SaveTheSlots();
			
			RpgCharacterSerializer.SaveCharacter(this.character);
			
			Debug.Log("The character \"" + this.character.Name + "\" was saved to Slot " + slotNumber.ToString());
			
			this.RefreshUI();
		}
		
		
		/// <summary>
		/// 	Loads a player from file using the name and id of the player saved in the given slot.
		/// 	Returns true if the slot is not empty and the character was loaded
		/// </summary>
		/// <param name="slotNumber">Slot number.</param>
		public void LoadCharacterFromFile(int slotNumber)
		{
			SaveSlot slot = SaveSlotRegistry.Instance.GetSlotAt(slotNumber);
			Debug.Assert(slot != null, "Invalid SaveSlot");
			
			if(slot.isSlotOccupied == false)
			{
				Debug.LogWarning("Save Slot " + slotNumber.ToString() + " is empty. No character was loaded.");
				return;
			}
			
			this.character = RpgCharacterSerializer.LoadCharacter(slot.playerName, slot.playerId);
			
			Debug.Assert(this.character != null, "Unknown Error when loading character from file! " + slot.playerName + " " + slot.playerId);
			
			Debug.Log("The character \"" + this.character.Name + "\" was loaded from Slot " + slotNumber.ToString());
			
			this.RefreshUI();
			
		}
		
		private bool LoadCharacterFromFileWithCheck(int slotNumber)
		{
			SaveSlot slot = SaveSlotRegistry.Instance.GetSlotAt(slotNumber);
			Debug.Assert(slot != null, "Invalid SaveSlot");
			
			if(slot.isSlotOccupied == false)
			{
				Debug.LogWarning("Save Slot " + slotNumber.ToString() + " is empty. No character was loaded.");
				return false;
			}
			
			this.character = RpgCharacterSerializer.LoadCharacter(slot.playerName, slot.playerId);
			
			Debug.Assert(this.character != null, "Unknown Error when loading character from file! " + slot.playerName + " " + slot.playerId);
			
			Debug.Log("The character \"" + this.character.Name + "\" was loaded from Slot " + slotNumber.ToString());
			
			this.RefreshUI();
			
			return true;
		}
		
		
		
		/// <summary>
		/// 	Randomizes the data of the current character. Does not save changes to file
		/// </summary>
		public void RandomizeCharacter()
		{
			RpgCharacterPacket newPacket = new RpgCharacterPacket();
			
			int randomIndex = Random.Range(0, this.randomNames.Length);
			newPacket.name = this.randomNames[randomIndex];
			newPacket.id = this.randomGuids[randomIndex];
			newPacket.hp = Random.Range(5, 150);
			newPacket.maxHp = 150;
			newPacket.additionalMaxHp = 0;
			newPacket.unallocatedSpPool = Random.Range(0,33);
			newPacket.assignmentType = RpgDataRegistry.Instance.DefaultStatPointAssignment;
			
			XpPacket xpPacket = new XpPacket();
			xpPacket.xpProgessorId = this.xpProgressor.Id.ToString();
			xpPacket.level = Random.Range(1, 10);
			xpPacket.xp = Random.Range(0, 301);
			xpPacket.xpToNextLevel = 300;
			xpPacket.currentLevelMultiplier = this.xpProgressor.LevelMultiplier;
			xpPacket.currentOldValueMultiplier = this.xpProgressor.OldXtnlMultiplier;
			
			newPacket.xpDataPacket = xpPacket;
			
			
			this.character = new RpgCharacterData(newPacket);
			
			Debug.Log("Current character was randomized. Note that nothing was saved to disk.");
			
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
		
		
		/// <summary>
		/// 	Toggles the Ability test panel.
		/// </summary>
		public void ToggleAbilityButtonPanel()
		{
			this.testAbilityPanel.SetActive(!this.testAbilityPanel.activeSelf);
		}
		
		
		/// <summary>
		/// 	Toggles the Serialization test panel.
		/// </summary>
		public void ToggleSerializationButtonPanel()
		{
			this.testSerializationPanel.SetActive(!this.testSerializationPanel.activeSelf);
		}
		
	}
}
