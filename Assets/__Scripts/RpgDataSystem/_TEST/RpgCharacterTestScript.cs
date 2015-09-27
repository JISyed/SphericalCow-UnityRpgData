using UnityEngine;
using UI = UnityEngine.UI;

namespace SphericalCow.Testing
{
	public class RpgCharacterTestScript : MonoBehaviour 
	{
		public string playerName;

		public UI.Text playerNameLabel;

		private RpgCharacterData player;

		// Use this for initialization
		void Start () 
		{
			this.player = new RpgCharacterData();
			this.player.SetCharacterName(this.playerName);
			this.playerNameLabel.text = this.player.CharacterName;
		}
		
		// Update is called once per frame
		void Update () 
		{
			
		}
	}
}