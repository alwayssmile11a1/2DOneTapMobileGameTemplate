using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_ANALYTICS
using UnityEngine.Analytics;
#endif


public class AnalyticsManager : MonoBehaviour {

    #region Static
    public static AnalyticsManager Instance
    {
        get
        {
            if (m_Instance != null) return m_Instance;

            m_Instance = FindObjectOfType<AnalyticsManager>();

            if (m_Instance != null) return m_Instance;

            //create new
            GameObject gameObject = new GameObject("AnalyticsManager");
            gameObject = Instantiate(gameObject);
            m_Instance = gameObject.AddComponent<AnalyticsManager>();

            return m_Instance;
        }

    }

    protected static AnalyticsManager m_Instance;
    #endregion

#if UNITY_ANALYTICS

    public AdvertisingNetwork adsNetwork = AdvertisingNetwork.UnityAds;


    protected readonly string m_RewardedAdsPlacementID = "rewardedVideo";

    public void AdOffer(bool adsRewarded)
    {
        if (adsRewarded)
        {
            AnalyticsEvent.AdOffer(adsRewarded, adsNetwork, m_RewardedAdsPlacementID);
        }
        else
        {
            AnalyticsEvent.AdOffer(adsRewarded, adsNetwork);
        }

    }

    public void AdStart(bool adsRewarded)
    {
        if (adsRewarded)
        {
            AnalyticsEvent.AdStart(adsRewarded, adsNetwork, m_RewardedAdsPlacementID);
        }
        else
        {
            AnalyticsEvent.AdStart(adsRewarded, adsNetwork);
        }

    }

    public void AdSkip(bool adsRewarded)
    {
        if (adsRewarded)
        {
            AnalyticsEvent.AdSkip(adsRewarded, adsNetwork, m_RewardedAdsPlacementID);
        }
        else
        {
            AnalyticsEvent.AdSkip(adsRewarded, adsNetwork);
        }

    }

    public void AdComplete(bool adsRewarded)
    {
        if (adsRewarded)
        {
            AnalyticsEvent.AdComplete(adsRewarded, adsNetwork, m_RewardedAdsPlacementID);
        }
        else
        {
            AnalyticsEvent.AdComplete(adsRewarded, adsNetwork);
        }

    }







#endif


}
