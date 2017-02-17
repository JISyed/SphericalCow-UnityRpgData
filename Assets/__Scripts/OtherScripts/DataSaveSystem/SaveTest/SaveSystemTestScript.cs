using UnityEngine;
using UI = UnityEngine.UI;

namespace SphericalCow
{
	namespace Testing
	{
		/// <summary>
		/// 	Only to test serialization of binary data
		/// </summary>
		public class SaveSystemTestScript : MonoBehaviour 
		{
			[SerializeField] private UI.Text strengthNumText;
			[SerializeField] private UI.Text agilityNumText;
			[SerializeField] private UI.Text willpowerNumText;
			[SerializeField] private UI.Text perceptionNumText;
			[SerializeField] private UI.Text luckNumText;
			[SerializeField] private UI.Text healthNumText;

			// Used for early initialization
			void Awake()
			{
				GlobalGameStats.Instance.IncrementLuck(0);
				this.UpdateUiDataDisplay();
			}

			// Use this for initialization
			void Start () 
			{
				
			}


			/// <summary>
			/// 	Updates the UI displaying data from GlobalGameStats. Get called whenever a button is pressed
			/// </summary>
			public void UpdateUiDataDisplay()
			{
				this.strengthNumText.text = GlobalGameStats.Instance.Strength.ToString();
				this.agilityNumText.text = GlobalGameStats.Instance.Agility.ToString();
				this.willpowerNumText.text = GlobalGameStats.Instance.Willpower.ToString();
				this.perceptionNumText.text = GlobalGameStats.Instance.Perception.ToString();
				this.luckNumText.text = GlobalGameStats.Instance.Luck.ToString();
				this.healthNumText.text = GlobalGameStats.Instance.Health.ToString();
			}

			/// <summary>
			/// 	Clears the values of the GlobalGameStats object.
			/// </summary>
			public void ClearValues()
			{
				GlobalGameStats.Instance.SetStrength(0);
				GlobalGameStats.Instance.SetAgility(0);
				GlobalGameStats.Instance.SetWillpower(0);
				GlobalGameStats.Instance.SetPerception(0);
				GlobalGameStats.Instance.SetLuck(0);
				this.UpdateUiDataDisplay();
			}

			/// <summary>
			/// 	Generate random values for the GlobalGameStats object.
			/// </summary>
			public void RandomValues()
			{
				GlobalGameStats.Instance.SetStrength(Random.Range(0, 11));
				GlobalGameStats.Instance.SetAgility(Random.Range(0, 11));
				GlobalGameStats.Instance.SetWillpower(Random.Range(0, 11));
				GlobalGameStats.Instance.SetPerception(Random.Range(0, 11));
				GlobalGameStats.Instance.SetLuck(Random.Range(0, 11));
				this.UpdateUiDataDisplay();
			}

			/// <summary>
			/// 	Saves GlobalGameStats's data to a file
			/// </summary>
			public void SaveValues()
			{
				GlobalGameStats.Instance.Save();
				this.UpdateUiDataDisplay();
			}

			/// <summary>
			/// 	Loads GlobalGameStats's data from a file
			/// </summary>
			public void LoadValues()
			{
				GlobalGameStats.Instance.Load();
				this.UpdateUiDataDisplay();
			}
		}
	}
}
