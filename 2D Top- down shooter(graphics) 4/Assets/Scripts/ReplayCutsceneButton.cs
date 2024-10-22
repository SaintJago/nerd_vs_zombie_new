using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ReplayCutsceneButton : MonoBehaviour
{
    [SerializeField] private string cutsceneSceneName = "Cutscene";
    [SerializeField] private Button replayButton;

    private void Start()
    {
        if (replayButton == null)
        {
            replayButton = GetComponent<Button>();
        }

        if (replayButton != null)
        {
            replayButton.onClick.AddListener(ReplayCutscene);
        }
        else
        {
            Debug.LogError("Button component is not assigned to ReplayCutsceneButton!");
        }
    }

    private void ReplayCutscene()
    {
        ComicController.SetReplayMode();
        SceneManager.LoadScene(cutsceneSceneName);
    }

    private void OnDestroy()
    {
        if (replayButton != null)
        {
            replayButton.onClick.RemoveListener(ReplayCutscene);
        }
    }
}