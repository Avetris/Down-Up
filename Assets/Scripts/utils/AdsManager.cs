using GoogleMobileAds.Api;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdsManager : MonoBehaviour
{
    public const string PP_GAMES_TO_AD = "GamesToAd";

    const int MAX_GAMES = 5;

    #region SINGLETON
    protected static AdsManager _instance = null;
    public static AdsManager instance()
    {
        if (_instance == null)
        {
            GameObject go = new GameObject();
            _instance = go.AddComponent<AdsManager>();
            go.name = "Ads Manager";
        }
        return _instance;
    }
    #endregion

    BannerView _bannerView;
    InterstitialAd _interstitial;

    bool showRealAds()
    {
        return !Application.isEditor && !Debug.isDebugBuild;
    }

    private void Start()
    {
    #if UNITY_ANDROID
        string appId = AdsSecrets.ADMOB_ANDROID_APPID;
#elif UNITY_IPHONE
        string appId = AdsSecrets.ADMOB_IPHONE_APPID;
#else
        string appId = "unexpected_platform";
#endif

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(appId);
        requestInterstical();
    }

    public void requestBanner()
    {
#if UNITY_ANDROID
        string adUnitId = !showRealAds() ? "ca-app-pub-3940256099942544/6300978111" : AdsSecrets.ADMOB_ANDROID_BANNER;
#elif UNITY_IPHONE
        string adUnitId = !showRealAds() ? "ca-app-pub-3940256099942544/2934735716" : AdsSecrets.ADMOB_IPHONE_BANNER;
#else
        string adUnitId = "unexpected_platform";
#endif

        this._bannerView = new BannerView(adUnitId, AdSize.SmartBanner, AdPosition.Top);

        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();

        // Load the banner with the request.
        this._bannerView.LoadAd(request);

        this._bannerView.Show();
    }

    public void showBanner()
    {
        if (this._bannerView == null)
        {
            requestBanner();
        }
        else
        {
            this._bannerView.Show();
        }
    }

    public void hideBanner()
    {
        if (this._bannerView != null)
        {
            this._bannerView.Hide();
        }
    }


    private void requestInterstical()
    {
#if UNITY_ANDROID
        string adUnitId = !showRealAds() ? "ca-app-pub-3940256099942544/1033173712" : AdsSecrets.ADMOB_ANDROID_INSTERSTICAL;
#elif UNITY_IPHONE
        string adUnitId = !showRealAds() ? "ca-app-pub-3940256099942544/4411468910" : AdsSecrets.ADMOB_IPHONE_INSTERSTICAL;
#else
        string adUnitId = "unexpected_platform";
#endif

        this._interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this._interstitial.LoadAd(request);
    }

    public void showInterstical()
    {
        if (PlayerPrefs.GetInt(PP_GAMES_TO_AD, MAX_GAMES) <= 0)
        {
            PlayerPrefs.SetInt(PP_GAMES_TO_AD, MAX_GAMES);
            if (this._interstitial != null && this._interstitial.IsLoaded())
            {
                this._interstitial.Show();
            }
            else
            {
                requestInterstical();
                this._interstitial.Show();
            }
        }
        else
        {
            PlayerPrefs.SetInt(PP_GAMES_TO_AD, PlayerPrefs.GetInt(PP_GAMES_TO_AD, MAX_GAMES) - 1);
        }
    }

    private void OnApplicationQuit()
    {
        if (_bannerView != null)
        {
            _bannerView.Destroy();
        }
        if (_interstitial != null)
        {
            _interstitial.Destroy();
        }
        if (SceneManager.GetActiveScene().name.Equals("MainScene"))
        {
            PlayerPrefs.SetInt(PP_GAMES_TO_AD, PlayerPrefs.GetInt(PP_GAMES_TO_AD, MAX_GAMES) - 1);
        }
    }

    static string getTime(DateTime date)
    {
        DateTime baseDate = new DateTime(1970, 1, 1);
        TimeSpan diff = date - baseDate;
        return Math.Round(diff.TotalSeconds).ToString();
    }
}