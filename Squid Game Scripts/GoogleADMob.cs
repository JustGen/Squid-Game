using UnityEngine.Events;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class GoogleADMob : MonoBehaviour
{
    public static GoogleADMob S;

    private RewardedAd rewardedAd1;
    private RewardedAd rewardedAd2;
    private InterstitialAd interstitial;
    private BannerView bannerView;

    private string adUnitId_rewardedAd1;
    private string adUnitId_rewardedAd2;
    private string adUnitId_interstitial;
    private string adUnitId_bannerView;

    private int _currIdReward;

    private void Awake()
    {
        S = this;
    }

    public void Start()
    {
        adUnitId_rewardedAd1 = "ca-app-pub-8820301686154131/9969928241";
        adUnitId_rewardedAd2 = "ca-app-pub-8820301686154131/8413303998";
        adUnitId_interstitial = "ca-app-pub-8820301686154131/9576797144";
        adUnitId_bannerView = "ca-app-pub-8820301686154131/8853477068";

        //adUnitId_rewardedAd1 = "ca-app-pub-3940256099942544/5224354917"; //test
        //adUnitId_rewardedAd2 = "ca-app-pub-3940256099942544/5224354917"; //test
        //adUnitId_interstitial = "ca-app-pub-3940256099942544/1033173712"; //test
        //adUnitId_bannerView = "ca-app-pub-3940256099942544/6300978111"; //test

        MobileAds.Initialize(initStatus => { });

        RequestRewardedAd1();
        RequestRewardedAd2();
        RequestInterstitial();
        RequestBanner();
    }

    private void RequestInterstitial()
    {
        this.interstitial = new InterstitialAd(adUnitId_interstitial);

        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        this.interstitial.OnAdOpening += HandleOnAdOpened;
        this.interstitial.OnAdClosed += HandleOnAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        this.interstitial.LoadAd(request);
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {

    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {

    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        CoreGame.S.StopGame(true);
        RequestInterstitial();
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        interstitial.Destroy();
        CoreGame.S.StopGame(false);
        AudioBox.S.AudioPlayBG();
    }

    public void ShowInterstitialVideo()
    {
        if (interstitial.IsLoaded() && PlayerPrefs.GetInt("ads") == 0)
        {
            interstitial.Show();
        }
        else
        {

        }
    }

    //########################################################################################################

    private void RequestRewardedAd1()
    {
        this.rewardedAd1 = new RewardedAd(adUnitId_rewardedAd1);

        this.rewardedAd1.OnAdLoaded += HandleRewardedAdLoaded;
        this.rewardedAd1.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        this.rewardedAd1.OnAdOpening += HandleRewardedAdOpening;
        this.rewardedAd1.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        this.rewardedAd1.OnUserEarnedReward += HandleUserEarnedReward;
        this.rewardedAd1.OnAdClosed += HandleRewardedAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        this.rewardedAd1.LoadAd(request);
    }

    private void RequestRewardedAd2()
    {
        this.rewardedAd2 = new RewardedAd(adUnitId_rewardedAd1);

        this.rewardedAd2.OnAdLoaded += HandleRewardedAdLoaded;
        this.rewardedAd2.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
        this.rewardedAd2.OnAdOpening += HandleRewardedAdOpening;
        this.rewardedAd2.OnAdFailedToShow += HandleRewardedAdFailedToShow;
        this.rewardedAd2.OnUserEarnedReward += HandleUserEarnedReward;
        this.rewardedAd2.OnAdClosed += HandleRewardedAdClosed;

        AdRequest request = new AdRequest.Builder().Build();
        this.rewardedAd2.LoadAd(request);
    }

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {

    }

    public void HandleRewardedAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {

    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        CoreGame.S.StopGame(true);

        if (_currIdReward == 2)
        {
            AudioBox.S.AudioStopHero();
        }
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {

    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        if (_currIdReward == 1)
        {
            rewardedAd1.Destroy();
            CoreGame.S.StopGame(false);
            RequestRewardedAd1();
        }
        else
        {
            rewardedAd2.Destroy();
            CoreGame.S.StopGame(false);
            RequestRewardedAd2();
        }
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        if (_currIdReward == 1)
        {
            //AudioBox.S.AudioPlayBG();
            MenuManager.S.GetReward();
        }
        else
        {
            CoreGame.S.Respawn();
        }
    }

    public void ShowRewardedVideo(int idRewardAds)
    {
        if (idRewardAds == 1)
        {
            if (rewardedAd1.IsLoaded())
            {
                _currIdReward = 1;
                rewardedAd1.Show();
            }
        }
        else
        {
            if (rewardedAd2.IsLoaded())
            {
                _currIdReward = 2;
                rewardedAd2.Show();
            }
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe from reward video event
        this.interstitial.OnAdLoaded -= HandleOnAdLoaded;
        this.interstitial.OnAdFailedToLoad -= HandleOnAdFailedToLoad;
        this.interstitial.OnAdOpening -= HandleOnAdOpened;
        this.interstitial.OnAdClosed -= HandleOnAdClosed;

        this.rewardedAd1.OnAdLoaded -= HandleRewardedAdLoaded;
        this.rewardedAd1.OnAdFailedToLoad -= HandleRewardedAdFailedToLoad;
        this.rewardedAd1.OnAdOpening -= HandleRewardedAdOpening;
        this.rewardedAd1.OnAdFailedToShow -= HandleRewardedAdFailedToShow;
        this.rewardedAd1.OnUserEarnedReward -= HandleUserEarnedReward;
        this.rewardedAd1.OnAdClosed -= HandleRewardedAdClosed;

        this.rewardedAd2.OnAdLoaded -= HandleRewardedAdLoaded;
        this.rewardedAd2.OnAdFailedToLoad -= HandleRewardedAdFailedToLoad;
        this.rewardedAd2.OnAdOpening -= HandleRewardedAdOpening;
        this.rewardedAd2.OnAdFailedToShow -= HandleRewardedAdFailedToShow;
        this.rewardedAd2.OnUserEarnedReward -= HandleUserEarnedReward;
        this.rewardedAd2.OnAdClosed -= HandleRewardedAdClosed;
    }

    //########################################################################################################

    private void RequestBanner()
    {
        this.bannerView = new BannerView(adUnitId_bannerView, AdSize.Banner, AdPosition.Top);
        AdRequest request1 = new AdRequest.Builder().Build();
        this.bannerView.LoadAd(request1);
    }
}
