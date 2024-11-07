using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeController : MonoBehaviour
{
    public CanvasGroup fadePanel; // ������ �� CanvasGroup ������ ����������
    public float fadeDuration = 1f; // ����������������� ������������ � ��������

    private void Start()
    {
        // ��������� ������� ���������� fadePanel
        if (fadePanel == null)
        {
            Debug.LogError("Fade Panel not assigned! Please assign a CanvasGroup component.");
            return;
        }

        // ������������� ��������� �������������� ������
        fadePanel.alpha = 1f;
        fadePanel.gameObject.SetActive(true);

        // ��������� ������� ������������ ������
        StartCoroutine(FadeOutPanel());
    }

    IEnumerator FadeOutPanel()
    {
        float elapsedTime = 0f;

        // ���������� ��������� ������������ �� 0
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadePanel.alpha = Mathf.Clamp01(1f - (elapsedTime / fadeDuration));
            yield return null;
        }

        // ����������, ��� ������ ��������� ���������� � ������������ �
        fadePanel.alpha = 0f;
        fadePanel.gameObject.SetActive(false);
    }
}
