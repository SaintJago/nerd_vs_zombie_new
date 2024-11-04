using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using TMPro;
using System.Linq;

public class MainMenu : MonoBehaviour
{
    public GameObject languagePanel; // Панель для кнопок языка
    public GameObject languageButtonPrefab; // Префаб кнопки языка
    public GameObject LevelSelectMenu; // Панель для выбора уровней


    private string languageKey = "SelectedLanguage";

    private Dictionary<string, string> localizedLanguageNames = new Dictionary<string, string>
    {
        {"en", "English"},
        {"ru", "Русский"},
        {"uk", "Українська"}
    };

    void Start()
    {
        // При запуске игры устанавливаем язык из сохраненных настроек
        string savedLanguage = PlayerPrefs.GetString(languageKey, "en");
        ChangeLanguage(savedLanguage);

    
    }

    public void OpenLevelSelectMenu()
    {
        LevelSelectMenu.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void OpenLanguagePanel()
    {
        foreach (Transform child in languagePanel.transform)
        {
            Destroy(child.gameObject);
        }

        foreach (var locale in LocalizationSettings.AvailableLocales.Locales)
        {
            GameObject button = Instantiate(languageButtonPrefab, languagePanel.transform);
            string languageCode = locale.Identifier.Code;
            string languageName = localizedLanguageNames.ContainsKey(languageCode) ? localizedLanguageNames[languageCode] : locale.name;
            button.GetComponentInChildren<TextMeshProUGUI>().text = languageName;
            button.GetComponent<Button>().onClick.AddListener(() => ChangeLanguage(locale.Identifier.Code));
        }

        languagePanel.SetActive(true);
    }

    public void ChangeLanguage(string localeIdentifier)
    {
        var locale = LocalizationSettings.AvailableLocales.Locales.FirstOrDefault(l => l.Identifier.Code == localeIdentifier);
        if (locale != null)
        {
            LocalizationSettings.SelectedLocale = locale;

            // Сохраняем выбранный язык в PlayerPrefs
            PlayerPrefs.SetString(languageKey, localeIdentifier);
            PlayerPrefs.Save();
        }
        languagePanel.SetActive(false);
    }
}
