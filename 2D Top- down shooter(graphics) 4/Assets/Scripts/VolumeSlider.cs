using UnityEngine;
using UnityEngine.UI;
using MyGame;
using BayatGames.SaveGameFree;


public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private string volumeName;
    private Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.value = SaveGame.Load<float>(volumeName, 0.75f);
        slider.onValueChanged.AddListener(SetVolume);
    }

    private void SetVolume(float value)
    {
        SoundManager.Instance.SetVolume(volumeName, value);
    }
}
