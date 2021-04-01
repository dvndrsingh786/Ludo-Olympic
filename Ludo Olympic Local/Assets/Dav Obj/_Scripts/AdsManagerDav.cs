using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;


public class AdsManagerDav : MonoBehaviour
{
    private BannerView bannerView;
    string test = "ca-app-pub-3940256099942544/6300978111";
    string notTest = "ca-app-pub-1545613221422810~6159753431";

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(initStatus => { });
        //RequestBanner();
    }
    public void RequestBanner()
    {
        //this.bannerView = new BannerView(test, AdSize.Banner, AdPosition.Top);
        // Create a 320x50 banner at the bottom of the screen.

        AdSize customSize = new AdSize(320, 100);

        Debug.LogError("Assigning banner view");

        bannerView = new BannerView(test, customSize, 0, 500);

        // Called when an ad request has successfully loaded.
        this.bannerView.OnAdLoaded += this.HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.bannerView.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
        // Called when an ad is clicked.
        this.bannerView.OnAdOpening += this.HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        this.bannerView.OnAdClosed += this.HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this.bannerView.OnAdLeavingApplication += this.HandleOnAdLeavingApplication;

        Debug.LogError("Create an empty ad request");
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        Debug.LogError("Load the banner with the request");
        // Load the banner with the request.
        bannerView.LoadAd(request);
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        Debug.LogError("Ad Loaded");
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.LogError("Failed to load ad");
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.Message);
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        Debug.LogError("Ad Opened");
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        Debug.LogError("Ad Closed");
        MonoBehaviour.print("HandleAdClosed event received");
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        Debug.LogError("Ad Leaving Application");
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }
}
