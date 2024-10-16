using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

[System.Serializable]
public class ComicPanel
{
    public Image image;
    public AudioClip audio;
}

public class ComicController : MonoBehaviour
{
    public ComicPanel[] comicPanels;
    public float normalDisplayTime = 3f;
    public float firstPanelDelay = 0.5f;
    public Button skipButton;
    public string menuSceneName = "Menu"; // Имя сцены меню

    private int currentPanelIndex = 0;
    private AudioSource audioSource;
    private Coroutine displayCoroutine;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();

        foreach (ComicPanel panel in comicPanels)
        {
            panel.image.gameObject.SetActive(false);
        }

        skipButton.onClick.AddListener(SkipComic);
        displayCoroutine = StartCoroutine(DisplayComic());
    }

    private IEnumerator DisplayComic()
    {
        for (currentPanelIndex = 0; currentPanelIndex < comicPanels.Length; currentPanelIndex++)
        {
            ComicPanel currentPanel = comicPanels[currentPanelIndex];

            currentPanel.image.gameObject.SetActive(true);

            if (currentPanel.audio != null)
            {
                audioSource.clip = currentPanel.audio;
                audioSource.Play();
            }

            if (currentPanelIndex == 0)
            {
                yield return new WaitForSeconds(firstPanelDelay);
            }

            yield return new WaitForSeconds(normalDisplayTime);

            // Удалено отключение предыдущих панелей
        }

        Debug.Log("Комикс завершен");
    }

    private void SkipComic()
    {
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
        }

        // Отобразить все панели
        foreach (ComicPanel panel in comicPanels)
        {
            panel.image.gameObject.SetActive(true);
        }

        audioSource.Stop();
        currentPanelIndex = comicPanels.Length;
        Debug.Log("Комикс пропущен");
    }
    
    private void OnDisable()
    {
        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
        }
        audioSource.Stop();
    }
}
