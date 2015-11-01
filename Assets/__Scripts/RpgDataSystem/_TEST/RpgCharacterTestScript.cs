using UnityEngine;
using System.Collections.Generic;
using UI = UnityEngine.UI;
using System.Text;	// For StringBuilder
using Guid = System.Guid;
using SphericalCow.Generics;

namespace SphericalCow.Testing
{
	public class RpgCharacterTestScript : MonoBehaviour 
	{
		public string playerName;
		public ProgressionType progressionVariable;
		public List<BasicStat> dataForBasicStats;
		public List<SecondaryStat> dataForSecondaryStats;
		public List<SkillStat> dataForSkillStats;
		public List<Ability> dataForAbilities;

		public bool createCharacterNow = true;	// If true, the player will be made at runtime and not loaded from file
		public bool showGuids = false;

		public UI.Text playerNameLabel;
		public UI.Text progressionVariableLabel;
		public UI.Text basicStatsLabel;
		public UI.Text secondaryStatsLabel;
		public UI.Text skillStatsLabel;
		public UI.Text abilitiesLabel;

		private RpgCharacterData player;	// The thing we are serializing

		private StringBuilder strBuild;

		// Use this for initialization
		void Start () 
		{
			// Only run if you are CREATING a character
			if(this.createCharacterNow)
			{
				this.player = new RpgCharacterData();
				this.player.SetCharacterName(this.playerName);
				this.player.SetProgressionVariable(this.progressionVariable);

				foreach(var basicStat in this.dataForBasicStats)
				{
					this.player.ListOfBasicStats.Add(new BasicStatInstance(basicStat, this.player));
				}

				foreach(var secondaryStat in this.dataForSecondaryStats)
				{
					this.player.ListOfSecondaryStats.Add(new SecondaryStatInstance(secondaryStat, this.player));
				}

				foreach(var skillStat in this.dataForSkillStats)
				{
					this.player.ListOfSkillStats.Add(new SkillStatInstance(skillStat, this.player));
				}

				foreach(var ability in this.dataForAbilities)
				{
					this.player.ListOfAbilties.Add(new AbilityInstance(ability, this.player));
				}
			}
			// Only run if you are LOADING a character from file
			else
			{
				Debug.LogWarning("Branch not implemened!");
			}

			// Make a StringBuilder because a ton of strings will be written
			this.strBuild = new StringBuilder();

			// Setup labels
			this.RefreshUI();

		}
		
		// Update is called once per frame
		void Update () 
		{
			
		}

		/// <summary>
		///  	Used to refresh the Unity UI displaying data about the character.
		/// 	Doens't have to run every frame
		/// </summary>
		private void RefreshUI()
		{
			//////////
			///  Top Header
			//////////

			this.playerNameLabel.text = this.player.CharacterName;
			this.progressionVariableLabel.text = "Lvl: " + this.player.ProgressionVariable.ToString();

			// Clear String Builder
			this.strBuild.Length = 0;

			//////////
			///  Basic Stats
			//////////

			// "Print" Basic stats
			this.strBuild.Append("Basic Stats:\n\n");
			foreach(var basicStatInst in this.player.ListOfBasicStats)
			{
				this.strBuild.Append("Name: ").Append(basicStatInst.StatName).Append("\n");
				if(this.showGuids)
				{
					this.strBuild.Append("GUID: ").Append(basicStatInst.StatGuid.ToString()).Append("\n");
				}
				this.strBuild.Append("Current Level: ").Append(basicStatInst.LocalXpPool).Append("\n");
				this.strBuild.Append("Next Level At: ").Append(basicStatInst.NextLevelXp).Append("\n\n");
			}
			this.basicStatsLabel.text = this.strBuild.ToString();

			// Clear String Builder
			this.strBuild.Length = 0;

			//////////
			///  Secondary Stats
			//////////

			// "Print" Secondary stats
			this.strBuild.Append("Secondary Stats:\n\n");
			foreach(var secStatInst in this.player.ListOfSecondaryStats)
			{
				this.strBuild.Append("Name: ").Append(secStatInst.StatName).Append("\n");
				if(this.showGuids)
				{
					this.strBuild.Append("GUID: ").Append(secStatInst.StatGuid.ToString()).Append("\n");
				}
				this.strBuild.Append("Current Level: ").Append(secStatInst.LocalXpPool).Append("\n");
				this.strBuild.Append("Next Level At: ").Append(secStatInst.NextLevelXp).Append("\n");

				this.strBuild.Append("Derived From:\n");
				foreach(var statPercentPair in secStatInst.DerivativeBasicStats)
				{
					this.strBuild.Append("      ").Append(statPercentPair.First.StatName).Append("  ");
					this.strBuild.Append(statPercentPair.Second).Append("%\n");
				}
				this.strBuild.Append("\n");
			}
			this.secondaryStatsLabel.text = this.strBuild.ToString();

			// Clear String Builder
			this.strBuild.Length = 0;

			//////////
			///  Skill Stats
			//////////
			 
			// "Print" Skill stats
			this.strBuild.Append("Skill Stats:\n\n");
			foreach(var skillStatInst in this.player.ListOfSkillStats)
			{
				this.strBuild.Append("Name: ").Append(skillStatInst.StatName).Append("\n");
				if(this.showGuids)
				{
					this.strBuild.Append("GUID: ").Append(skillStatInst.StatGuid.ToString()).Append("\n");
				}
				this.strBuild.Append("Current Level: ").Append(skillStatInst.LocalXpPool).Append("\n");
				this.strBuild.Append("Next Level At: ").Append(skillStatInst.NextLevelXp).Append("\n");

				this.strBuild.Append("Derived From:\n");
				foreach(var statPercentPair in skillStatInst.DerivativeStats)
				{
					this.strBuild.Append("      ").Append(statPercentPair.First.GetStatType().ToString()).Append(": ");
					this.strBuild.Append(statPercentPair.First.StatName).Append("  ");
					this.strBuild.Append(statPercentPair.Second).Append("%\n");
				}
				this.strBuild.Append("\n");
			}
			this.skillStatsLabel.text = this.strBuild.ToString();

			// Clear Stirng Builder
			this.strBuild.Length = 0;



			////////////
			/// Abilities
			////////////

			// Print Abilities
			this.strBuild.Append("Abilities:\n\n");
			foreach(var ability in this.player.ListOfAbilties)
			{
				this.strBuild.Append("Name: ").Append(ability.AbilityName).Append("\n");
				this.strBuild.Append("Modifiers: ").Append("\n");

				foreach(var abilityModifier in ability.AbilityModifierInstances)
				{
					this.strBuild.Append("      ");
					this.strBuild.Append("Stat To Mod : ").Append(abilityModifier.StatName).Append("\n");

					if(this.showGuids)
					{
						this.strBuild.Append("      ").Append("      ");
						this.strBuild.Append("Stat Inst GUID: ").Append(abilityModifier.StatInstanceGuid.ToString()).Append("\n");
					}

					this.strBuild.Append("      ").Append("      ");
					this.strBuild.Append(abilityModifier.Type.ToString()).Append("\n");

					this.strBuild.Append("      ").Append("      ");
					this.strBuild.Append("Target: ").Append(abilityModifier.TargetValue).Append("\n");

					this.strBuild.Append("      ").Append("      ");
					this.strBuild.Append("Original: ").Append(abilityModifier.OriginalValue).Append("\n");
				}
				this.strBuild.Append("\n");
			}
			this.abilitiesLabel.text = this.strBuild.ToString();
			this.strBuild.Length = 0; // Clear
		}

		/// <summary>
		/// Called when the Save button is pressed
		/// </summary>
		public void ButtonSave()
		{
			Debug.Log("Save not ready!");
		}

		/// <summary>
		/// Called when the Load button is pressed
		/// </summary>
		public void ButtonLoad()
		{
			Debug.Log("Load not ready!");
		}

		/// <summary>
		/// Called when the Random Values button is pressed
		/// </summary>
		public void ButtonRandomValues()
		{
			Debug.Log("Random not ready!");
		}
	}
}