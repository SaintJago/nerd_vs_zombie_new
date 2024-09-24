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
    [SerializeField] Button[] buyButtons; // Кнопки покупки
    [SerializeField] TextMeshProUGUI[] boughtTexts; // Текстовые поля для отображения состояния покупки
    [SerializeField] int[] prices; // Цены на товары

    [SerializeField] GameObject shopPanel; // Панель магазина

    public delegate void BuySeconPosition();
    public event BuySeconPosition buySeconPosition;

    public static Shop Instance;
    public GameObject soundButton;
    public GameObject soundMenu;
    private bool isShopOpen;

    [SerializeField] Player player;

    [SerializeField] AudioClip popSound, succesBuyClip; // Звуки

    private void Awake()
    {
        Instance = this;
        DeleteShopData(); // Удалить все сохраненные данные магазина
    }

    private void Start()
    {
        InitializeShop();
    }

    // Закрыть магазин
    private void CloseShop()
    {
        isShopOpen = false;
        shopPanel.SetActive(false);
        Resume(); // Снять с паузы
    }

    private void Update()
    {
        HandleShopPanelToggle();
    }

    // Нажатие кнопки звука
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

    // Открыть меню звука
    public void OpenSoundMenu()
    {
        soundMenu.SetActive(true);
        // Остановка времени
        PauseManager.PauseGame();
    }

    // Открыть магазин
    public void OpenShop()
    {
        shopPanel.SetActive(true);
        Check();
        SoundManager.Instance.PlayerSound(popSound);
        // Остановка времени
        PauseManager.PauseGame();
        Cursor.visible = true;

        if (isShopOpen)
        {
            // Магазин уже открыт, закрываем его
            CloseShop();
        }
        else
        {
            // Открываем магазин
            isShopOpen = true;
            shopPanel.SetActive(true);
        }
    }

    // Снять с паузы
    public void Resume()
    {
        shopPanel.SetActive(false);
        soundMenu.SetActive(false);
        // Возобновить игру
        PauseManager.ResumeGame();
        isShopOpen = false; // Сбрасываем флаг открытости магазина
    }

    // Загрузить меню
    public void LoadMenu()
    {
        StopAllCoroutines();
        // Возобновить игру
        PauseManager.ResumeGame();
        SceneManager.LoadScene("Menu");
    }

    // Инициализировать магазин
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

    // Обработчик переключения панели магазина
    void HandleShopPanelToggle()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isShopOpen)
            {
                // Если магазин открыт, закрываем его
                CloseShop();
            }
            else
            {
                // Если магазин закрыт, открываем его
                OpenShop();
            }
        }
    }
    // Проверить состояние кнопок
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

    // Установить состояние кнопки
    async void SetButtonState(int index, bool interactable, string key)
    {
        buyButtons[index].interactable = interactable;
        var op = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI", key);
        boughtTexts[index].text = await op.Task;
    }

    // Купить товар
    public void Buy(int index)
    {
        if (Player.Instance.currentMoney >= prices[index])
        {
            MarkAsBought(index);
            Player.Instance.AddMoney(-prices[index]);
            Check();
        }
    }

    // Отметить товар как купленный
    void MarkAsBought(int index)
    {
        buyButtons[index].interactable = false;
        PlayerPrefs.SetInt("Position" + index, 1);
        PlayerPrefs.Save(); // Сохранить изменения

        if (index == 2 && buySeconPosition != null) buySeconPosition.Invoke();
        if (index == 4 && buySeconPosition != null)
        {
            buySeconPosition.Invoke();

            // Проверяем, что у игрока есть дрон
            if (PlayerPrefs.GetInt("Position4") == 1)
            {
                Player.Instance.droneInstance.SetActive(true);
                // Устанавливаем цель для дрона
                DroneMovement droneMovement = Player.Instance.droneInstance.GetComponent<DroneMovement>();
                if (droneMovement != null)
                {
                    droneMovement.target = Player.Instance.transform;
                }
            }
        }
    }

    // Удалить данные магазина
    void DeleteShopData()
    {
        for (int i = 0; i < buyButtons.Length; i++)
        {
            PlayerPrefs.DeleteKey("Position" + i);
        }
    }

    [ContextMenu("Delete Player Prefs")]
    void DeletePlayerPrefs() => PlayerPrefs.DeleteAll();
}
