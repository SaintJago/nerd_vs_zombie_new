using UnityEngine;

public class InterstitialAds : MonoBehaviour
{
    //public PopUpController popUpController;

    void Start()
    {
        

        IronSourceInterstitialEvents.onAdReadyEvent += InterstitialAdReadyEvent;
        IronSourceInterstitialEvents.onAdLoadFailedEvent += InterstitialAdLoadFailedEvent;
        IronSourceInterstitialEvents.onAdOpenedEvent += InterstitialAdOpenedEvent;
        IronSourceInterstitialEvents.onAdClickedEvent += InterstitialAdClickedEvent;
        IronSourceInterstitialEvents.onAdShowSucceededEvent += InterstitialAdShowSucceededEvent;
        IronSourceInterstitialEvents.onAdShowFailedEvent += InterstitialAdShowFailedEvent;
        IronSourceInterstitialEvents.onAdClosedEvent += InterstitialAdClosedEvent;
        IronSource.Agent.loadInterstitial();
       // popUpController.showPopUp("Interstitial starts load");
    } //

    private void OnDestroy()
    {
        IronSourceInterstitialEvents.onAdReadyEvent -= InterstitialAdReadyEvent;
        IronSourceInterstitialEvents.onAdLoadFailedEvent -= InterstitialAdLoadFailedEvent;
        IronSourceInterstitialEvents.onAdOpenedEvent -= InterstitialAdOpenedEvent;
        IronSourceInterstitialEvents.onAdClickedEvent -= InterstitialAdClickedEvent;
        IronSourceInterstitialEvents.onAdShowSucceededEvent -= InterstitialAdShowSucceededEvent;
        IronSourceInterstitialEvents.onAdShowFailedEvent -= InterstitialAdShowFailedEvent;
        IronSourceInterstitialEvents.onAdClosedEvent -= InterstitialAdClosedEvent;
    }

    public void showAds()
    {
        

        if (IronSource.Agent.isInterstitialReady())
        {
            IronSource.Agent.showInterstitial();
            //popUpController.showPopUp("Interstitial starts show clicked");
        }
        else
        {
            //debugT.text = "Interstitial not loaded";
            Debug.LogWarning("Interstitial not loaded");
            //popUpController.showPopUp("Interstitial not load");
            IronSource.Agent.loadInterstitial();
        }
    }

    #region callbacks handlers
    void InterstitialAdReadyEvent(IronSourceAdInfo info)
    {
        //debugT.text = "unity-script: I got InterstitialAdReadyEvent";
        Debug.Log("unity-script: I got InterstitialAdReadyEvent");
        //popUpController.showPopUp("Interstitial ads ready");
    }

    void InterstitialAdLoadFailedEvent(IronSourceError error)
    {
        //debugT.text = "unity-script: I got InterstitialAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription();
        Debug.Log("unity-script: I got InterstitialAdLoadFailedEvent, code: " + error.getCode() + ", description : " + error.getDescription());
        //popUpController.showPopUp("Interstitial fail to load code=" + error.getCode() + ", description: " + error.getDescription());
    }

    void InterstitialAdShowSucceededEvent(IronSourceAdInfo info)
    {
        //debugT.text = "unity-script: I got InterstitialAdShowSucceededEvent";
        Debug.Log("unity-script: I got InterstitialAdShowSucceededEvent");
        //popUpController.showPopUp("Interstitial showed success");
        //IronSource.Agent.loadInterstitial();
    }

    void InterstitialAdShowFailedEvent(IronSourceError error, IronSourceAdInfo info)
    {
        //debugT.text = "unity-script: I got InterstitialAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription();
        Debug.Log("unity-script: I got InterstitialAdShowFailedEvent, code :  " + error.getCode() + ", description : " + error.getDescription());
        //popUpController.showPopUp("Interstitial fail to show code=" + error.getCode() + ", description: " + error.getDescription());
    }

    void InterstitialAdClickedEvent(IronSourceAdInfo info)
    {
        //debugT.text = "unity-script: I got InterstitialAdClickedEvent";
        Debug.Log("unity-script: I got InterstitialAdClickedEvent");
        //popUpController.showPopUp("Interstitial clicked");
    }

    void InterstitialAdOpenedEvent(IronSourceAdInfo info)
    {
        //debugT.text = "unity-script: I got InterstitialAdOpenedEvent";
        Debug.Log("unity-script: I got InterstitialAdOpenedEvent");
        //popUpController.showPopUp("Interstitial opens");
    }

    void InterstitialAdClosedEvent(IronSourceAdInfo info)
    {
        //debugT.text = "unity-script: I got InterstitialAdClosedEvent";
        Debug.Log("unity-script: I got InterstitialAdClosedEvent");
        //popUpController.showPopUp("Interstitial closed");
        IronSource.Agent.loadInterstitial();
    }
    #endregion

}