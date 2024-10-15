using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Localization.Settings;

public class DeathPanel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private AudioClip popSound;
    private AudioSource audioSource;

    private void Start()
{
    Debug.Log("DeathPanel Start method called");

    audioSource = GetComponent<AudioSource>();
    if (audioSource == null)
    {
        Debug.LogError("AudioSource component not found on DeathPanel");
    }

    if (scoreText == null)
    {
        Debug.LogError("scoreText is not assigned in the inspector");
        return;
    }

    WaveSpawner wsP = FindObjectOfType<WaveSpawner>();
    if (wsP == null)
    {
        Debug.LogError("WaveSpawner not found in the scene");
        return;
    }

    Debug.Log("Requesting localized string...");
    var waveString = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI", "Current wave");
    waveString.Completed += op =>
    {
        Debug.Log("Localization operation completed");
        if (!op.IsValid())
        {
            Debug.LogError("Localization operation is invalid");
            return;
        }

        if (op.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            string localizedString = op.Result;
            if (string.IsNullOrEmpty(localizedString))
            {
                Debug.LogWarning("Localized string is null or empty, using default");
                localizedString = "Current wave";
            }

            if (scoreText != null && wsP != null)
            {
                scoreText.text = $"{localizedString}: {wsP.currentWaveIndex + 1}";
                Debug.Log($"Set scoreText to: {scoreText.text}");
            }
            else
            {
                Debug.LogError($"scoreText is null: {scoreText == null}, wsP is null: {wsP == null}");
            }
        }
        else
        {
            Debug.LogError($"Failed to load localized string: {op.OperationException}");
            if (scoreText != null && wsP != null)
            {
                scoreText.text = $"Current wave: {wsP.currentWaveIndex + 1}";
                Debug.Log($"Set scoreText to default: {scoreText.text}");
            }
            else
            {
                Debug.LogError($"scoreText is null: {scoreText == null}, wsP is null: {wsP == null}");
            }
        }
    };
}


    public void Restart()
    {
        Debug.Log("Restart method called");
        PauseManager.ResumeGame();
        if (audioSource != null && popSound != null)
        {
            audioSource.PlayOneShot(popSound);
        }
        else
        {
            Debug.LogWarning("Unable to play pop sound: audioSource or popSound is null");
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMenu()
    {
        Debug.Log("LoadMenu method called");
        PauseManager.ResumeGame();
        if (audioSource != null && popSound != null)
        {
            audioSource.PlayOneShot(popSound);
        }
        else
        {
            Debug.LogWarning("Unable to play pop sound: audioSource or popSound is null");
        }
        SceneManager.LoadScene("Menu");
    }

    private void OnDisable()
    {
        Debug.Log("DeathPanel OnDisable called");
        // Если есть какие-либо подписки на события, отпишитесь от них здесь
    }
}
