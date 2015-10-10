using UnityEngine;
using System.Collections.Generic;
using UI = UnityEngine.UI;
using System.Text;	// For StringBuilder

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
					this.player.ListOfBasicStats.Add(new BasicStatInstance(basicStat));
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
			this.playerNameLabel.text = this.player.CharacterName;
			this.progressionVariableLabel.text = "Lvl: " + this.player.ProgressionVariable.ToString();

			// Clear String Builder
			this.strBuild.Length = 0;

			// "Print" Basic stats
			this.strBuild.Append("Basic Stats:\n\n");
			foreach(var basicStatInst in this.player.ListOfBasicStats)
			{
				this.strBuild.Append("Name: ").Append(basicStatInst.StatName).Append("\n");
				this.strBuild.Append("Current Level: ").Append(basicStatInst.LocalXpPool).Append("\n");
				this.strBuild.Append("Next Level At: ").Append(basicStatInst.NextLevelXp).Append("\n\n");
			}
			this.basicStatsLabel.text = this.strBuild.ToString();

			// Clear String Builder
			this.strBuild.Length = 0;

		}
	}
}