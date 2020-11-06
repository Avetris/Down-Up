using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System;

public class LeaderboardManager
{
    static LeaderboardManager _instance;

    private LeaderboardManager()
    {
    #if UNITY_ANDROID
        var config = new PlayGamesClientConfiguration.Builder().Build();
        PlayGamesPlatform.InitializeInstance(config);
        if (Application.isEditor || Debug.isDebugBuild)
            PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        login(null);
#elif UNITY_IPHONE
            login(null);
#endif
    }

    public static LeaderboardManager instance()
    {
        if (_instance == null)
        {
            _instance = new LeaderboardManager();
        }
        return _instance;
    }

    void login(System.Action<int> callback, int score)
    {
        Social.localUser.Authenticate((bool success) => {
            if (success)
            {
                callback?.Invoke(score);
            }
        });
    }

    void login(System.Action callback)
    {
        Social.localUser.Authenticate((bool success) => {
            if (success)
            {
                callback?.Invoke();
            }
        });
    }

    public void showLeaderboard()
    {
#if UNITY_ANDROID || UNITY_IPHONE
        if (Social.localUser.authenticated)
        {
            Social.ShowLeaderboardUI();
        }
        else
        {
            login(showLeaderboard);
        }
#endif
    }


    public void setPuntuation(int score)
    {

    #if UNITY_ANDROID || UNITY_IPHONE
            string leaderboardString = "";

    #if UNITY_ANDROID

        leaderboardString = GPGSIds.leaderboard_down_up;

#elif UNITY_IPHONE
        leaderboardString = GPGSIds.leaderboard_iphone_down_up;

#endif

        Debug.Log("Leaderboard to insert: " + score);
        int lastScore = PlayerPrefs.GetInt("Score", score);
        Debug.Log("Last Leaderboard to insert: " + lastScore);
        if (lastScore > score)
        {
            Debug.Log("ENTERED");
            score = lastScore;
        }
        PlayerPrefs.SetInt("Score", score);
        if (leaderboardString.Length > 0)
        {
            if (Social.localUser.authenticated)
            {
                Social.ReportScore(
                    score, leaderboardString,
                    (bool success) =>
                    {
                        Debug.Log("(" + leaderboardString + ")Leaderboard update success: " + score);
                    });
            }
            else
            {
                login(setPuntuation, score);
            }
        }
#endif
    }
}