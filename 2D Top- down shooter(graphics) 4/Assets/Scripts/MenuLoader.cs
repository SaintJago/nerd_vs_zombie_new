using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLoader : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject levelSelectMenu;
    private const string SHOW_LEVEL_SELECT_KEY = "ShouldShowLevelSelect";

    private void Start()
    {
        Debug.Log("MenuLoader Start called");
        if (PlayerPrefs.GetInt(SHOW_LEVEL_SELECT_KEY, 0) == 1)
        {
            Debug.Log("Showing LevelSelectMenu");
            ShowLevelSelectMenu();
            PlayerPrefs.SetInt(SHOW_LEVEL_SELECT_KEY, 0);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("Showing MainMenu");
            ShowMainMenu();
        }
    }

    private void ShowMainMenu()
    {
        Debug.Log("ShowMainMenu called");
        if (mainMenu != null)
        {
            mainMenu.SetActive(true);
            Debug.Log("MainMenu activated");
        }
        else
        {
            Debug.LogError("MainMenu is null");
        }
        if (levelSelectMenu != null)
        {
            levelSelectMenu.SetActive(false);
            Debug.Log("LevelSelectMenu deactivated");
        }
        else
        {
            Debug.LogError("LevelSelectMenu is null");
        }
    }

    private void ShowLevelSelectMenu()
    {
        Debug.Log("ShowLevelSelectMenu called");
        if (mainMenu != null)
        {
            mainMenu.SetActive(true);
            Debug.Log("MainMenu deactivated");
        }
        else
        {
            Debug.LogError("MainMenu is null");
        }
        if (levelSelectMenu != null)
        {
            levelSelectMenu.SetActive(true);
            Debug.Log("LevelSelectMenu activated");
        }
        else
        {
            Debug.LogError("LevelSelectMenu is null");
        }
    }
}