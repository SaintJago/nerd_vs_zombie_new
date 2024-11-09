using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelCompleteScript : MonoBehaviour
{
    private const string SHOW_LEVEL_SELECT_KEY = "ShouldShowLevelSelect";
    private const string UNLOCKED_LEVELS_KEY = "unlockedLevels";

    [SerializeField] private CanvasGroup fadePanel;
    [SerializeField] private float fadeDuration = 1f;

    public void OnLevelComplete()
    {
        Debug.Log("OnLevelComplete called");
        PauseManager.ResumeGame();

        if (LevelSelectionMenuManager.currLevel == LevelSelectionMenuManager.unlockedLevels)
        {
            LevelSelectionMenuManager.unlockedLevels++;
            PlayerPrefs.SetInt(UNLOCKED_LEVELS_KEY, LevelSelectionMenuManager.unlockedLevels);
            PlayerPrefs.Save();
            Debug.Log($"Unlocked new level. Total unlocked levels: {LevelSelectionMenuManager.unlockedLevels}");
        }

        PlayerPrefs.SetInt(SHOW_LEVEL_SELECT_KEY, 1);
        PlayerPrefs.Save();

        StartCoroutine(FadeOutAndLoadMenu());
    }

    private IEnumerator FadeOutAndLoadMenu()
    {
        fadePanel.gameObject.SetActive(true);
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadePanel.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        fadePanel.alpha = 1f;

        Debug.Log("Loading Menu scene");
        SceneManager.LoadScene("Menu");
    }
}
