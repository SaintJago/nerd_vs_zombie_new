using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public CanvasGroup fadePanel; // Ссылка на CanvasGroup панели затемнения
    public float fadeDuration = 1f; // Продолжительность затемнения в секундах

    private void Start()
    {
        // Проверяем наличие компонента fadePanel
        if (fadePanel == null)
        {
            Debug.LogError("Fade Panel not assigned! Please assign a CanvasGroup component.");
            return;
        }

        // Инициализируем начальные значения панели
        fadePanel.alpha = 1f;
        fadePanel.gameObject.SetActive(true);

        // Запускаем корутину исчезновения панели
        StartCoroutine(FadeOutPanel());
    }

    IEnumerator FadeOutPanel()
    {
        float elapsedTime = 0f;

        // Постепенное уменьшение прозрачности до 0
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadePanel.alpha = Mathf.Clamp01(1f - (elapsedTime / fadeDuration));
            yield return null;
        }

        // Убеждаемся, что панель полностью прозрачна и выключаем её
        fadePanel.alpha = 0f;
        fadePanel.gameObject.SetActive(false);
    }

    // Добавим метод для появления панели, если он вам понадобится
    public IEnumerator FadeInPanel()
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
    }
}