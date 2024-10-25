using TMPro;
using UnityEngine;

public class MediationInit : MonoBehaviour
{
    [SerializeField] private string _androidAppKey;
    [SerializeField] private string _appleAppKey;

    private static bool isInit = false;

    //public TMP_Text debugT;

    private void Awake()
    {
        if (isInit)
        {
            Debug.Log("Already init IS ads");
            return;
        }

        IronSourceEvents.onSdkInitializationCompletedEvent += adsInitSuccess;
        initAds();
    }

    public void initAds()
    {
#if UNITY_ANDROID
        string appKey = _androidAppKey;
#elif UNITY_IPHONE
        string appKey = _appleAppKey;
#else
        string appKey = "unexpected_platform";
#endif

        IronSource.Agent.validateIntegration();
        IronSource.Agent.shouldTrackNetworkState(true);
        IronSource.Agent.setAdaptersDebug(true);
        IronSource.Agent.init(appKey);
    }

    void OnApplicationPause(bool isPaused)
    {
        // Debug.Log("unity-script: OnApplicationPause = " + isPaused);
        IronSource.Agent.onApplicationPause(isPaused);
    }

    private void adsInitSuccess()
    {
        Debug.Log("Init mediation completed");
        isInit = true;
    }
}