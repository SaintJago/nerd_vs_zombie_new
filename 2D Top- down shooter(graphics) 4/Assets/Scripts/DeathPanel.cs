using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Localization.Settings;

public class DeathPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] AudioClip popSound;
    private AudioSource audioSource; // Добавляем компонент AudioSource

    private void Start()
    {
        audioSource = GetComponent<AudioSource>(); // Получаем компонент AudioSource
        WaveSpawner wsP = FindObjectOfType<WaveSpawner>();
        var waveString = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI", "Current wave");
        waveString.Completed += op =>
        {
            scoreText.text = op.Result + ": " + (wsP.currentWaveIndex + 1).ToString();
        };
    }

    public void Restart()
    {
        PauseManager.ResumeGame();
        audioSource.PlayOneShot(popSound); // Используем AudioSource для воспроизведения звука
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadMenu()
    {
        PauseManager.ResumeGame();
        audioSource.PlayOneShot(popSound); // Используем AudioSource для воспроизведения звука при загрузке меню
        SceneManager.LoadScene("Menu");
    }
}
