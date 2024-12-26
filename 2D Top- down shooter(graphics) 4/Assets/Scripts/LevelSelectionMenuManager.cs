using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionMenuManager : MonoBehaviour
{
    public LevelObject[] levelObjects;
    public static int currLevel;
    public static int unlockedLevels;

    [Header("Fade Settings")]
    public CanvasGroup fadePanel;
    public float fadeDuration = 1f;

    private bool isTransitioning = false;
    private Image fadeImage;

    public void onClickLevel(int levelNum)
    {
        if (!isTransitioning)
        {
            currLevel = levelNum;
            StartCoroutine(FadeAndLoadLevel(levelNum));
        }
    }

    IEnumerator FadeAndLoadLevel(int levelNum)
    {
        isTransitioning = true;

        fadePanel.gameObject.SetActive(true);
        fadePanel.alpha = 0f;
        fadePanel.blocksRaycasts = true;

        if (fadeImage != null)
        {
            Color imageColor = Color.black;
            imageColor.a = 1f;
            fadeImage.color = imageColor;
        }

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadePanel.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            yield return null;
        }

        fadePanel.alpha = 1f;

        SceneManager.LoadScene("Level" + (levelNum + 1));
    }

    void Start()
    {
        if (fadePanel == null)
        {
            Debug.LogError("Fade Panel not assigned! Please assign a CanvasGroup component.");
            return;
        }

        SetupFadePanel();

        // Загружаем количество разблокированных уровней с помощью PlayerPrefs
        unlockedLevels = PlayerPrefs.GetInt("unlockedLevels", 0);

        for (int i = 0; i < levelObjects.Length; i++)
        {
            if (unlockedLevels >= i)
            {
                levelObjects[i].levelButton.interactable = true;
            }
        }
    }

    private void SetupFadePanel()
    {
        fadeImage = fadePanel.GetComponent<Image>();
        if (fadeImage == null)
        {
            fadeImage = fadePanel.gameObject.AddComponent<Image>();
        }

        fadeImage.color = Color.black;
        fadeImage.raycastTarget = true;

        fadePanel.alpha = 0f;
        fadePanel.blocksRaycasts = false;
        fadePanel.interactable = false;
        fadePanel.gameObject.SetActive(false);

        Canvas fadePanelCanvas = fadePanel.GetComponent<Canvas>();
        if (fadePanelCanvas == null)
        {
            fadePanelCanvas = fadePanel.gameObject.AddComponent<Canvas>();
        }

        fadePanelCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        fadePanelCanvas.sortingOrder = 9999;

        CanvasScaler scaler = fadePanel.GetComponent<CanvasScaler>();
        if (scaler == null)
        {
            scaler = fadePanel.gameObject.AddComponent<CanvasScaler>();
        }

        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        RectTransform rectTransform = fadePanel.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.sizeDelta = Vector2.zero;
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.localScale = Vector3.one;
        }
    }

    public static void SaveProgress()
    {
        // Сохраняем прогресс с помощью PlayerPrefs
        PlayerPrefs.SetInt("unlockedLevels", unlockedLevels);
        PlayerPrefs.Save(); // Принудительно сохраняем изменения
    }

    void Update()
    {
        // Оставляем пустым, если нет необходимости в постоянном обновлении
    }
}