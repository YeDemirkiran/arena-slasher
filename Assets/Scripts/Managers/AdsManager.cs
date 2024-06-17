using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    void Awake()
    {
        InitializeAds();
    }

    void InitializeAds()
    {
        IronSource.Agent.setConsent(true);
        IronSourceEvents.onSdkInitializationCompletedEvent += OnSdkInitializationCompleted;

        IronSource.Agent.setMetaData("is_test_suite", "enable");
        IronSource.Agent.init("1ed0ca41d", IronSourceAdUnits.REWARDED_VIDEO, IronSourceAdUnits.INTERSTITIAL);
        IronSource.Agent.validateIntegration();

        IronSourceRewardedVideoEvents.onAdAvailableEvent += OnAdAvailable;
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

        Debug.Log("New ad available!");
    }

    public void ShowInterstitialAd()
    {
        IronSource.Agent.showInterstitial("Startup");
    }
    
    void OnSdkInitializationCompleted()
    {
        Debug.Log("IronSource has been initialized successfully");

        IronSource.Agent.launchTestSuite();
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
