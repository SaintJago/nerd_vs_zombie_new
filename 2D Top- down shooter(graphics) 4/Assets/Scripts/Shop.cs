using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;
using MyGame;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
		[SerializeField] Button[] buyButtons;
		[SerializeField] TextMeshProUGUI[] boughtTexts;
		[SerializeField] int[] prices;
		[SerializeField] GameObject shopPanel;


		public delegate void BuySeconPosition();
		public event BuySeconPosition buySeconPosition;

		public static Shop Instance;
		public GameObject soundButton;
		public GameObject soundMenu;
		private bool isShopOpen;

		[SerializeField] Player player;
		[SerializeField] private GameObject dronePrefab; // Add this at the top of Shop class



		[SerializeField] AudioClip popSound, succesBuyClip;

		private void Awake()
		{
				Instance = this;
		}

		private void Start()
		{
				InitializeShop();
		}

		private void CloseShop()
		{
				isShopOpen = false;
				shopPanel.SetActive(false);
				
				// Показываем контроллер обратно

				Resume();
		}

		public void OpenShop()
		{
				shopPanel.SetActive(true);
				Check();
				SoundManager.Instance.PlayerSound(popSound);
				
				// Скрываем контроллер

				PauseManager.PauseGame();
				Cursor.visible = true;

				if (isShopOpen)
				{
						CloseShop();
				}
				else
				{
						isShopOpen = true;
						shopPanel.SetActive(true);
				}
		}

		private void Update()
		{
				HandleShopPanelToggle();
		}

		public void SoundButtonPressed()
		{
				if (soundMenu.activeSelf && !shopPanel.activeSelf)
				{
						soundMenu.SetActive(false);
						Resume();
				}
				else if (soundMenu.activeSelf && shopPanel.activeSelf)
				{
						soundMenu.SetActive(false);
				}
				else
				{
						OpenSoundMenu();
				}
		}

		public void OpenSoundMenu()
		{
				soundMenu.SetActive(true);
				PauseManager.PauseGame();
		}

		public void Resume()
		{
				shopPanel.SetActive(false);
				soundMenu.SetActive(false);
				PauseManager.ResumeGame();
				isShopOpen = false;
		}

		public void LoadMenu()
		{
				StopAllCoroutines();
				PauseManager.ResumeGame();
				SceneManager.LoadScene("Menu");
		}

		void InitializeShop()
		{
				for (int i = 0; i < buyButtons.Length; i++)
				{
						int position = PlayerPrefs.GetInt("Position" + i, 0);
						if (position == 1)
						{
								MarkAsBought(i);
						}
				}

				Check();
		}

		void HandleShopPanelToggle()
		{
				if (Input.GetKeyDown(KeyCode.Escape))
				{
						if (isShopOpen)
						{
								CloseShop();
						}
						else if (!soundMenu.activeSelf)
						{
								OpenShop();
						}
				}
		}

		void Check()
		{
				for (int i = 0; i < buyButtons.Length; i++)
				{
						if (PlayerPrefs.GetInt("Position" + i) == 1)
						{
								SetButtonState(i, false, "Bought");
						}
						else if (Player.Instance.currentMoney < prices[i])
						{
								SetButtonState(i, false, "NotEnoughCoins");
						}
						else
						{
								SetButtonState(i, true, "Buy");
						}
				}
		}

		async void SetButtonState(int index, bool interactable, string key)
		{
				buyButtons[index].interactable = interactable;
				var op = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI", key);
				boughtTexts[index].text = await op.Task;
		}

		public void Buy(int index)
		{
				if (Player.Instance.currentMoney >= prices[index])
				{
						MarkAsBought(index);
						Player.Instance.AddMoney(-prices[index]);
						
						if (succesBuyClip != null)
						{
								SoundManager.Instance.PlayerSound(succesBuyClip);
						}
						
						Check();
				}
		}

		void MarkAsBought(int index)
		{
				buyButtons[index].interactable = false;
				PlayerPrefs.SetInt("Position" + index, 1);
				PlayerPrefs.Save();

				if (index == 2 && buySeconPosition != null)
				{
						buySeconPosition.Invoke();
				}

				if (index == 4 && buySeconPosition != null)
        {
            buySeconPosition.Invoke();

             if (PlayerPrefs.GetInt("Position4") == 1 && Player.Instance.droneInstance != null)
            {
                Player.Instance.droneInstance.SetActive(true);
                DroneMovement droneMovement = Player.Instance.droneInstance.GetComponent<DroneMovement>();
                if (droneMovement != null)
                {
                    droneMovement.target = Player.Instance.transform;
                }
            }
        }


		}

		[ContextMenu("Reset Shop Data")]
		void DeleteShopData()
		{
				for (int i = 0; i < buyButtons.Length; i++)
				{
						PlayerPrefs.DeleteKey("Position" + i);
				}
				PlayerPrefs.Save();
				Debug.Log("Shop data has been reset");
		}

		[ContextMenu("Delete All PlayerPrefs")]
		void DeletePlayerPrefs()
		{
				PlayerPrefs.DeleteAll();
				Debug.Log("All PlayerPrefs have been deleted");
		}
}