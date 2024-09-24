using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;

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

    [SerializeField] Wave[] waves;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] float timeBtwWaves;
    [SerializeField] AudioSource waveAudioSource;
    Wave currentWave;
    [HideInInspector] public int currentWaveIndex;
    Transform player;

    bool isSpawnFinished = false;

    [SerializeField] TextMeshProUGUI waveText;

    bool isFreeTime = true;
    float curtimeBtwWaves;

    [SerializeField] GameObject spawnEffect;

    [SerializeField] AudioClip waveCompleteClip;

    public GameObject CompletePanel; // Публичное поле для ссылки на CompletePanel
    public float delayBeforeShowingPanel = 3.0f; // Задержка перед показом панели (в секундах)

    private void Start()
    {
        waveAudioSource = GetComponent<AudioSource>();

        player = Player.Instance.transform;

        curtimeBtwWaves = timeBtwWaves;

        UpdateText();

        StartCoroutine(CallNextWave(currentWaveIndex));
    }

    private void Update()
    {
        UpdateText();

        if (isSpawnFinished && GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
        {
            isSpawnFinished = false;

            if(currentWaveIndex + 1 < waves.Length)
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

    void UpdateText()
    {
        if (isFreeTime)
        {
            waveText.text = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI", "Next wave").Result + ": " + ((int)(curtimeBtwWaves -= Time.deltaTime)).ToString();
        }
        else
        {
            waveText.text = LocalizationSettings.StringDatabase.GetLocalizedStringAsync("UI", "Current wave").Result + ": " + (currentWaveIndex + 1).ToString();
        }
    }

    IEnumerator CallNextWave(int waveIndex)
    {
        curtimeBtwWaves = timeBtwWaves;

        isFreeTime = true;
        // Воспроизвести звук завершения волны
        if (waveAudioSource != null && waveCompleteClip != null)
        {
            waveAudioSource.PlayOneShot(waveCompleteClip);
        }

        yield return new WaitForSeconds(timeBtwWaves);
        isFreeTime = false;
        StartCoroutine(SpawnWave(waveIndex));
    }

    IEnumerator SpawnWave(int waveIndex)
    {
        currentWave = waves[waveIndex];

        for (int i = 0; i < currentWave.enemyCount; i++)
        {
            if (player == null) yield break;

            GameObject randomEnemy = currentWave.enemies[Random.Range(0, currentWave.enemies.Length)];
            Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

            Instantiate(randomEnemy, randomSpawnPoint.position, Quaternion.identity);
            Instantiate(spawnEffect, randomSpawnPoint.position, Quaternion.identity);

            yield return new WaitForSeconds(currentWave.timeBtwSpawn);
        }

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

    IEnumerator ShowCompletePanelWithDelay()
    {
        yield return new WaitForSeconds(delayBeforeShowingPanel);
        CompletePanel.SetActive(true);
        // Остановка времени
        PauseManager.PauseGame();
    }
}