using UnityEngine;
using UnityEngine.Advertisements;


public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    #region Fields

    [SerializeField] string _androidGameId;
    [SerializeField] string _iOSGameId;
    [SerializeField] bool _testMode = true;
    private string _gameId;

    #endregion


    #region UnityMethods

    private void Awake()
    {
        InitializeAds();
    }

    #endregion


    #region Methods
    public void InitializeAds()
    {
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOSGameId
            : _androidGameId;
        Advertisement.Initialize(_gameId, _testMode, this);
    }

#if UNITY_EDITOR
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
#endif

    #endregion
}
