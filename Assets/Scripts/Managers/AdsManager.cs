using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    bool adAvailable;

    void Awake()
    {
        InitializeAds();
    }

    void InitializeAds()
    {
        IronSource.Agent.setConsent(true);

        //IronSource.Agent.setMetaData("is_test_suite", "enable");
        //IronSource.Agent.init("1ed0ca41d", IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL);
        IronSource.Agent.init("1ed0ca41d");
        //IronSource.Agent.validateIntegration();

        IronSourceInterstitialEvents.onAdReadyEvent += OnAdAvailable;
        IronSourceInterstitialEvents.onAdShowSucceededEvent += (IronSourceAdInfo adInfo) => Debug.Log("Ad succeeded to load");
        IronSourceInterstitialEvents.onAdShowFailedEvent += (IronSourceError error, IronSourceAdInfo adInfo) => Debug.Log("Ad failed to load");

        IronSource.Agent.loadInterstitial();

        //Debug.Log("woo");
    }

    private void OnEnable()
    {
        IronSourceEvents.onSdkInitializationCompletedEvent += OnSdkInitializationCompleted;
    }

    void OnAdAvailable(IronSourceAdInfo adInfo)
    {
        //switch (adInfo.adUnit)
        //{
        //    case "Startup":
        //        break;
        //    default:
        //        break;
        //}

        //IronSource.Agent.showInterstitial(adInfo.adUnit);
        adAvailable = true;
        Debug.Log("New ad available!");
    }

    public void ShowInterstitialAd()
    {
        if (!adAvailable) { Debug.Log("Ad not available!"); return; }

        Debug.Log("Ad showed!");
        IronSource.Agent.showInterstitial();
        
    }
    
    void OnSdkInitializationCompleted()
    {
        Debug.Log("IronSource has been initialized successfully");

        //IronSource.Agent.launchTestSuite();
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization is complete.");
    }

    private void OnApplicationPause(bool pause)
    {
        IronSource.Agent.onApplicationPause(pause);
    }
}
