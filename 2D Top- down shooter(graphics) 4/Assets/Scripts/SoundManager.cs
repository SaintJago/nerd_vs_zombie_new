using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using BayatGames.SaveGameFree;


namespace MyGame
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }
        [SerializeField] AudioMixer mixer;
        [SerializeField] Slider musicSlider;
        [SerializeField] Slider sfxSlider;
        [SerializeField] Slider shotSlider;
        const string MIXER_MUSIC = "MusicVolume";
        const string MIXER_SFX = "SFXVolume";
        const string MIXER_SHOT = "ShotVolume";
        float _musicVolume;
        float _sfxVolume;
        float _shotVolume;

        AudioSource audS;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.Log("SoundManager instance already exists. Destroying this one.");
                Destroy(gameObject);
            }
            audS = GetComponent<AudioSource>();
            // Загрузка сохраненных значений при запуске
            musicSlider.value = SaveGame.Load<float>(MIXER_MUSIC, 0.75f);
            sfxSlider.value = SaveGame.Load<float>(MIXER_SFX, 0.75f);
            shotSlider.value = SaveGame.Load<float>(MIXER_SHOT, 0.75f);

            musicSlider.onValueChanged.AddListener(SetMusicVolume);
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
            shotSlider.onValueChanged.AddListener(SetShotVolume);
        }
        void Start()
        {
            // Установка громкости при запуске сцены
            SetMusicVolume(musicSlider.value);
            SetSFXVolume(sfxSlider.value);
            SetShotVolume(shotSlider.value);
        }
        public void PlayerSound(AudioClip value)
        {
            if (Instance == null)
            {
                Debug.LogError("SoundManager.Instance is null. Make sure the SoundManager script is set up correctly.");
                return;
            }

            if (audS == null)
            {
                Debug.LogError("AudioSource is null. Make sure it is assigned in the inspector.");
                return;
            }

            audS.pitch = Random.Range(0.9f, 1.1f);
            audS.PlayOneShot(value);
        }

        public void SetVolume(string name, float value)
        {
            mixer.SetFloat(name, Mathf.Log10(value) * 20);
            SaveGame.Save<float>(name, value);
        }

        void SetMusicVolume(float value)
        {
            mixer.SetFloat(MIXER_MUSIC, Mathf.Log10(value) * 20);
            SaveGame.Save<float>(MIXER_MUSIC, value); // Сохранение значения
        }

        void SetSFXVolume(float value)
        {
            mixer.SetFloat(MIXER_SFX, Mathf.Log10(value) * 20);
            SaveGame.Save<float>(MIXER_SFX, value); // Сохранение значения
        }

        void SetShotVolume(float value)
        {
            mixer.SetFloat(MIXER_SHOT, Mathf.Log10(value) * 20);
            SaveGame.Save<float>(MIXER_SHOT, value); // Сохранение значения
        }

        //private void OnApplicationQuit()
        //{
        //    PlayerPrefs.Save(); // Сохранение всех изменений при выходе из игры
        //}
    }
}