using UnityEngine;
using UI = UnityEngine.UI;
using Guid = System.Guid;
using System.Text;	// For StringBuilder

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
		
		
		
		// Use this for initialization
		void Start () 
		{
			Debug.Assert(string.IsNullOrEmpty(this.xpProgressorName) == false, "Please provide an proper XpProgressor in the Inspector");
			this.xpProgressor = RpgDataRegistry.Instance.SearchXpProgressor(this.xpProgressorName);
			Debug.Assert(this.xpProgressor != null, "Could not find an XpProgressor by the name " + this.xpProgressorName);
			
			this.character = new RpgCharacterData(this.xpProgressor, 
			                                      this.startingHealthPoints, 
			                                      this.startingHealthPoints, 
			                                      this.givenCharacterName);
			this.character.AddXp(this.startingXp);
			
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
	}
}
