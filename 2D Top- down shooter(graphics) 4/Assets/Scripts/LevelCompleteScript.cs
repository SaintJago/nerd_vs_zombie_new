using UnityEngine;
using UnityEngine.SceneManagement;
using BayatGames.SaveGameFree;

public class LevelCompleteScript : MonoBehaviour
{
    public void OnLevelComplete()
    {
        Debug.Log("OnLevelComplete called");
        PauseManager.ResumeGame();

        if (LevelSelectionMenuManager.currLevel == LevelSelectionMenuManager.unlockedLevels)
        {
            LevelSelectionMenuManager.unlockedLevels++;
            SaveGame.Save<int>("unlockedLevels", LevelSelectionMenuManager.unlockedLevels);
        }

        SceneManager.LoadScene("Menu");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu")
        {
            Debug.Log("Menu scene loaded, searching for LevelSelectMenu");
            Invoke("ActivateLevelSelectMenu", 0.1f);
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void ActivateLevelSelectMenu()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            Transform mainMenu = canvas.transform.Find("MainMenu");
            if (mainMenu != null)
            {
                Transform levelSelectMenu = mainMenu.Find("LevelSelectMenu");
                if (levelSelectMenu != null)
                {
                    levelSelectMenu.gameObject.SetActive(true);
                    Debug.Log("LevelSelectMenu activated successfully");
                }
                else
                {
                    Debug.LogError("LevelSelectMenu not found in MainMenu");
                }
            }
            else
            {
                Debug.LogError("MainMenu not found in Canvas");
            }
        }
        else
        {
            Debug.LogError("Canvas not found in the scene");
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
