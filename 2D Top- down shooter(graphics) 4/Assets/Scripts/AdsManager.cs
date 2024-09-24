using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    [SerializeField] GameObject rewardedAdsButton, deathPanel;

    [SerializeField] AudioClip popSound;
    private AudioSource audioSource; // Добавляем компонент AudioSource

    void Start()
    {
        Advertisement.Initialize("5552395", false, this);
        audioSource = GetComponent<AudioSource>(); // Получаем компонент AudioSource
    }
    public void ShowAd()
    {
        Advertisement.Show("Rewarded_Android", this);
        audioSource.PlayOneShot(popSound); // Используем AudioSource для воспроизведения звука
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Complete");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
    }

    public void OnUnityAdsShowClick(string placementId)
    {
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == "Rewarded_Android")
        {
        // Возобновить игру
            PauseManager.ResumeGame();
            rewardedAdsButton.SetActive(false);
            deathPanel.SetActive(false);
            Player.Instance.gameObject.SetActive(true);
            Player.Instance.AddHealth(Player.Instance.maxHealth);
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
    }

    public void OnUnityAdsShowStart(string placementId)
    {
    }
}
