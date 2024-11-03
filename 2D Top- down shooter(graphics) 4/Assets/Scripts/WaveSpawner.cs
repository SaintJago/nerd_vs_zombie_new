using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.ResourceManagement.AsyncOperations;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct BossWave
    {
        public GameObject[] bosses;
        public int count;
    }

    [System.Serializable]
    public struct Wave
    {
        public GameObject[] enemies;
        public int enemyCount;
        public float timeBtwSpawn;
        public float timeBtwBossSpawn;
        public BossWave bossWave;
    }

    [SerializeField] Wave[] waves; // Массив волн
    [SerializeField] Transform[] spawnPoints; // Точки спавна
    [SerializeField] float timeBtwWaves; // Время между волнами
    [SerializeField] AudioSource waveAudioSource;
    Wave currentWave; // Текущая волна
    [HideInInspector] public int currentWaveIndex;
    Transform player;

    bool isSpawnFinished = false;
    [SerializeField] TextMeshProUGUI waveText;
    bool isFreeTime = true;
    float curtimeBtwWaves;

    [SerializeField] GameObject spawnEffect;
    [SerializeField] AudioClip waveCompleteClip;
    public GameObject CompletePanel;
    [SerializeField] private string videoSceneName;
    public float delayBeforeShowingPanel = 3.0f;

    // Инициализация при старте
    private void Start()
    {
        StartCoroutine(InitializeLocalization());
    }

    // Корутина для инициализации локализации
    private IEnumerator InitializeLocalization()
    {
        // Ждём завершения инициализации системы локализации
        yield return LocalizationSettings.InitializationOperation;

        waveAudioSource = GetComponent<AudioSource>();
        player = Player.Instance.transform;
        curtimeBtwWaves = timeBtwWaves;
        UpdateText();
        StartCoroutine(CallNextWave(currentWaveIndex));
    }

    // Обновление каждый кадр
    private void Update()
    {
        UpdateText();

        // Проверка завершения волны
        if (isSpawnFinished && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            isSpawnFinished = false;

            if (currentWaveIndex + 1 < waves.Length)
            {
                currentWaveIndex++;
                StartCoroutine(CallNextWave(currentWaveIndex));
            }
            else
            {
                StartCoroutine(ShowCompletePanelWithDelay());
            }
        }
    }

    // Обновление текста волны
    void UpdateText()
    {
        if (isFreeTime)
        {
            var nextWaveOperation = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI", "Next wave");
            waveText.text = $"{nextWaveOperation.Result}: {((int)(curtimeBtwWaves -= Time.deltaTime)).ToString()}";
        }
        else
        {
            var currentWaveOperation = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI", "Current wave");
            waveText.text = $"{currentWaveOperation.Result}: {(currentWaveIndex + 1).ToString()}";
        }
    }

    // Запуск следующей волны
    IEnumerator CallNextWave(int waveIndex)
    {
        curtimeBtwWaves = timeBtwWaves;
        isFreeTime = true;

        // Воспроизведение звука завершения волны
        if (waveAudioSource != null && waveCompleteClip != null)
        {
            waveAudioSource.PlayOneShot(waveCompleteClip);
        }

        yield return new WaitForSeconds(timeBtwWaves);
        isFreeTime = false;
        StartCoroutine(SpawnWave(waveIndex));
    }

    // Спавн врагов в волне
    IEnumerator SpawnWave(int waveIndex)
    {
        currentWave = waves[waveIndex];

        // Спавн обычных врагов
        for (int i = 0; i < currentWave.enemyCount; i++)
        {
            if (player == null) yield break;

            GameObject randomEnemy = currentWave.enemies[Random.Range(0, currentWave.enemies.Length)];
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            Instantiate(randomEnemy, randomSpawnPoint.position, Quaternion.identity);
            Instantiate(spawnEffect, randomSpawnPoint.position, Quaternion.identity);

            yield return new WaitForSeconds(currentWave.timeBtwSpawn);
        }

        // Спавн боссов
        for (int i = 0; i < currentWave.bossWave.count; i++)
        {
            if (player == null) yield break;

            GameObject randomBoss = currentWave.bossWave.bosses[Random.Range(0, currentWave.bossWave.bosses.Length)];
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            Instantiate(randomBoss, randomSpawnPoint.position, Quaternion.identity);
            Instantiate(spawnEffect, randomSpawnPoint.position, Quaternion.identity);

            yield return new WaitForSeconds(currentWave.timeBtwBossSpawn);
        }

        isSpawnFinished = true;
    }

    // Показ панели завершения с задержкой
    IEnumerator ShowCompletePanelWithDelay()
    {
        yield return new WaitForSeconds(delayBeforeShowingPanel);

        if (!string.IsNullOrEmpty(videoSceneName))
        {
            SceneManager.LoadScene(videoSceneName);
        }
        else
        {
            CompletePanel.SetActive(true);
            PauseManager.PauseGame();
        }
    }
}
