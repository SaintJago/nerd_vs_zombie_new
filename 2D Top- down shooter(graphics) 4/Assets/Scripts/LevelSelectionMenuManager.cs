using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using BayatGames.SaveGameFree;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionMenuManager : MonoBehaviour
{
    public LevelObject[] levelObjects;
    public static int currLevel;
    public static int unlockedLevels;

    [Header("Fade Settings")]
    public CanvasGroup fadePanel; // Ссылка на CanvasGroup панели затемнения
    public float fadeDuration = 1f; // Продолжительность затемнения в секундах

    private bool isTransitioning = false;
    private Image fadeImage; // Ссылка на компонент Image

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

        // Активируем панель и настраиваем начальное состояние
        fadePanel.gameObject.SetActive(true);
        fadePanel.alpha = 0f;
        fadePanel.blocksRaycasts = true; // Блокируем взаимодействие с UI под панелью

        // Убеждаемся, что изображение черное и полностью непрозрачное
        if (fadeImage != null)
        {
            Color imageColor = Color.black;
            imageColor.a = 1f;
            fadeImage.color = imageColor;
        }

        // Плавно увеличиваем непрозрачность
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadePanel.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            Debug.Log($"Fade Alpha: {fadePanel.alpha}"); // Отладочный вывод
            yield return null;
        }

        // Убеждаемся, что достигли полной непрозрачности
        fadePanel.alpha = 1f;

        // Загружаем новый уровень
        SceneManager.LoadScene("Level" + (levelNum + 1));
    }

    void Start()
    {
        // Проверяем наличие компонента fadePanel
        if (fadePanel == null)
        {
            Debug.LogError("Fade Panel not assigned! Please assign a CanvasGroup component.");
            return;
        }

        // Настраиваем панель затемнения
        SetupFadePanel();

        // Загружаем количество разблокированных уровней с помощью Save Game Free
        unlockedLevels = SaveGame.Load<int>("unlockedLevels", 0);

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
        // Получаем компонент Image
        fadeImage = fadePanel.GetComponent<Image>();
        if (fadeImage == null)
        {
            fadeImage = fadePanel.gameObject.AddComponent<Image>();
        }

        // Настраиваем Image компонент
        fadeImage.color = Color.black;
        fadeImage.raycastTarget = true;

        // Настраиваем CanvasGroup
        fadePanel.alpha = 0f;
        fadePanel.blocksRaycasts = false;
        fadePanel.interactable = false;
        fadePanel.gameObject.SetActive(false);

        // Получаем компонент Canvas
        Canvas fadePanelCanvas = fadePanel.GetComponent<Canvas>();
        if (fadePanelCanvas == null)
        {
            fadePanelCanvas = fadePanel.gameObject.AddComponent<Canvas>();
        }

        // Настраиваем Canvas для панели затемнения
        fadePanelCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        fadePanelCanvas.sortingOrder = 9999;

        // Получаем или добавляем CanvasScaler
        CanvasScaler scaler = fadePanel.GetComponent<CanvasScaler>();
        if (scaler == null)
        {
            scaler = fadePanel.gameObject.AddComponent<CanvasScaler>();
        }

        // Настраиваем CanvasScaler
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
        scaler.matchWidthOrHeight = 0.5f;

        // Настраиваем RectTransform
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
        SaveGame.Save<int>("unlockedLevels", unlockedLevels);
    }

    void Update()
    {
        // Оставляем пустым, если нет необходимости в постоянном обновлении
    }
}