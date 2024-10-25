using System;
using UnityEngine;

public class SimpleRewardedAds : MonoBehaviour
{
		private static Action grantEvent;
		public bool manualInit = false;


		public delegate void rewardedAdsReady();
		public static event rewardedAdsReady OnRewardedReady;

		private void Start()
		{
				if (manualInit)
						return;

				init();
		}

		public void init()
		{
				IronSourceRewardedVideoEvents.onAdOpenedEvent += RewardedVideoOnAdOpenedEvent;
				IronSourceRewardedVideoEvents.onAdClosedEvent += RewardedVideoOnAdClosedEvent;
				IronSourceRewardedVideoEvents.onAdAvailableEvent += RewardedVideoOnAdAvailable;
				IronSourceRewardedVideoEvents.onAdUnavailableEvent += RewardedVideoOnAdUnavailable;
				IronSourceRewardedVideoEvents.onAdShowFailedEvent += RewardedVideoOnAdShowFailedEvent;
				IronSourceRewardedVideoEvents.onAdRewardedEvent += RewardedVideoOnAdRewardedEvent;
				IronSourceRewardedVideoEvents.onAdClickedEvent += RewardedVideoOnAdClickedEvent;
				IronSource.Agent.loadRewardedVideo();
		}

		public bool isRewardedReady()
		{
				return IronSource.Agent.isRewardedVideoAvailable();
		}

		private void OnDestroy()
		{
				IronSourceRewardedVideoEvents.onAdOpenedEvent -= RewardedVideoOnAdOpenedEvent;
				IronSourceRewardedVideoEvents.onAdClosedEvent -= RewardedVideoOnAdClosedEvent;
				IronSourceRewardedVideoEvents.onAdAvailableEvent -= RewardedVideoOnAdAvailable;
				IronSourceRewardedVideoEvents.onAdUnavailableEvent -= RewardedVideoOnAdUnavailable;
				IronSourceRewardedVideoEvents.onAdShowFailedEvent -= RewardedVideoOnAdShowFailedEvent;
				IronSourceRewardedVideoEvents.onAdRewardedEvent -= RewardedVideoOnAdRewardedEvent;
				IronSourceRewardedVideoEvents.onAdClickedEvent -= RewardedVideoOnAdClickedEvent;
				grantEvent = null;
		}

		public void showRewarded(Action actionAfterSuccess)
		{
				grantEvent = actionAfterSuccess;

				if (IronSource.Agent.isRewardedVideoAvailable())
				{
						IronSource.Agent.showRewardedVideo();
				}
				else
				{
						Debug.LogWarning("Rewarded not ready");
						//debugT.text = "Rewarded not ready";
						IronSource.Agent.loadRewardedVideo();
				}
		}

		#region reward handlers
		private void RewardedVideoOnAdClickedEvent(IronSourcePlacement placement, IronSourceAdInfo info)
		{
				Debug.Log("Rewarded ad clicked");
				//debugT.text = "Rewarded ad clicked";
		}

		private void RewardedVideoOnAdRewardedEvent(IronSourcePlacement placement, IronSourceAdInfo info)
		{
				Debug.Log("Rewarded Ads grant a reward");
				//grantEvent?.Invoke();
		}

		private void RewardedVideoOnAdShowFailedEvent(IronSourceError error, IronSourceAdInfo info)
		{
				Debug.LogWarning("Rewarded ads fail to show");
				//debugT.text = "Rewarded ads fail to show";
		}

		private void RewardedVideoOnAdUnavailable()
		{
				Debug.LogWarning("Rewarded ads unavailable");
				//debugT.text = "Rewarded ads unavailable";
		}

		private void RewardedVideoOnAdAvailable(IronSourceAdInfo info)
		{
				Debug.Log("Rewarded Ads available to show");
				OnRewardedReady?.Invoke();
				//debugT.text = "Rewarded Ads available to show";
		}

		private void RewardedVideoOnAdClosedEvent(IronSourceAdInfo info)
		{
				Debug.Log("Rewarded Ads closed. Load a new ads");

				grantEvent?.Invoke();

				//debugT.text = "Rewarded Ads closed. Load a new ads";
				IronSource.Agent.loadRewardedVideo();
		}

		private void RewardedVideoOnAdOpenedEvent(IronSourceAdInfo info)
		{
				Debug.Log("Rewarded Ads opened");
				//debugT.text = "Rewarded Ads opened";
		}
		#endregion
}