using UnityEngine;
using UnityEngine.SceneManagement;
using BayatGames.SaveGameFree;

public class LevelCompleteScript : MonoBehaviour
{
    private const string SHOW_LEVEL_SELECT_KEY = "ShouldShowLevelSelect";

    public void OnLevelComplete()
    {
        Debug.Log("OnLevelComplete called");
        PauseManager.ResumeGame();

        if (LevelSelectionMenuManager.currLevel == LevelSelectionMenuManager.unlockedLevels)
        {
            LevelSelectionMenuManager.unlockedLevels++;
            SaveGame.Save<int>("unlockedLevels", LevelSelectionMenuManager.unlockedLevels);
            Debug.Log($"Unlocked new level. Total unlocked levels: {LevelSelectionMenuManager.unlockedLevels}");
        }

        PlayerPrefs.SetInt(SHOW_LEVEL_SELECT_KEY, 1);
        PlayerPrefs.Save();

        Debug.Log("Loading Menu scene");
        SceneManager.LoadScene("Menu");
    }
}