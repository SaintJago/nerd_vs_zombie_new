using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public CanvasGroup fadePanel; // Ссылка на CanvasGroup панели затемнения
    public float fadeDuration = 1f; // Продолжительность исчезновения в секундах

    private void Start()
    {
        // Проверяем наличие компонента fadePanel
        if (fadePanel == null)
        {
            Debug.LogError("Fade Panel not assigned! Please assign a CanvasGroup component.");
            return;
        }

        // Устанавливаем начальную непрозрачность панели
        fadePanel.alpha = 1f;
        fadePanel.gameObject.SetActive(true);

        // Запускаем плавное исчезновение панели
        StartCoroutine(FadeOutPanel());
    }

    IEnumerator FadeOutPanel()
    {
        float elapsedTime = 0f;

        // Постепенно уменьшаем прозрачность до 0
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadePanel.alpha = Mathf.Clamp01(1f - (elapsedTime / fadeDuration));
            yield return null;
        }

        // Убеждаемся, что панель полностью прозрачная и деактивируем её
        fadePanel.alpha = 0f;
        fadePanel.gameObject.SetActive(false);
    }
}
