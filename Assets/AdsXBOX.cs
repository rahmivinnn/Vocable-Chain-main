\using UnityEngine;
using UnityEngine.Advertisements;  // Namespace untuk Unity Ads
using UnityEngine.SceneManagement;

public class AdsXBOX : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsShowListener
{
    [Header("Unity Ads Setup")]
    public string androidGameId = "your-android-game-id";  // Ganti dengan ID game Androidmu
    public string iosGameId = "your-ios-game-id";          // Ganti dengan ID game iOSmu
    public string placementId = "rewardedVideo";           // ID iklan yang akan ditampilkan
    public bool testMode = true;  // Aktifkan jika kamu ingin melakukan pengujian iklan (gantilah ke false sebelum rilis)

    void Start()
    {
        string gameId = (Application.platform == RuntimePlatform.IPhonePlayer) ? iosGameId : androidGameId;

        Advertisement.Initialize(gameId, testMode, this);
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads Initialization Complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError("Unity Ads Initialization Failed: " + message);
    }

    public void ShowAd()
    {

        if (Advertisement.IsReady(placementId))
        {
            Advertisement.Show(placementId, this);
        }
        else
        {
            Debug.LogWarning("Iklan tidak siap!");
        }
    }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log("Iklan ditonton sepenuhnya! Memberikan reward.");
        }
        else
        {
            Debug.Log("Iklan tidak ditonton sepenuhnya.");
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError("Iklan gagal ditampilkan: " + message);
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log("Iklan mulai ditampilkan.");
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log("Iklan diklik.");
    }
}
