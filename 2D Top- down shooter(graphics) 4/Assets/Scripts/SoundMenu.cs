using UnityEngine;
using UnityEngine.UI;
using BayatGames.SaveGameFree;


public class SoundMenu : MonoBehaviour
{
    public static SoundMenu Instance; // Ссылка на единственный экземпляр

    public Slider volumeSlider;

    private string volumeKey = "SelectedVolume";

    private void Awake()
    {
        // Проверка наличия другого экземпляра
        if (Instance == null)
        {
            Instance = this; // Устанавливаем текущий экземпляр
            DontDestroyOnLoad(gameObject); // Делаем объект постоянным между сценами
        }
        else
        {
            Destroy(gameObject); // Уничтожаем дублирующиеся экземпляры
        }
    }

    private void Start()
    {
        // При запуске меню устанавливаем значение слайдера в сохраненное значение громкости
        volumeSlider.value = SaveGame.Load<float>(volumeKey, 1.0f);
    }

    // Метод вызывается при изменении положения слайдера
    public void OnVolumeChanged(float volume)
    {
        // Применяем изменение громкости к звуковым источникам в игре
        AudioListener.volume = volume;

        // Сохраняем значение громкости в PlayerPrefs
        SaveGame.Save<float>(volumeKey, volume);
    }
}
