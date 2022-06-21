using UnityEngine;
using UnityEngine.Advertisements;


namespace Victorina
{
    public class InterstellarAdsTrain : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        #region Fields

        [SerializeField] private GameObject _adsLable;
        [SerializeField] string _androidAdUnitId = "Interstitial_Android";
        [SerializeField] string _iOsAdUnitId = "Interstitial_iOS";
        private string _adUnitId;
        private int _countStepForAds = 3;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? _iOsAdUnitId
                : _androidAdUnitId;
        }

        private void Start()
        {
            if (!GameData.GetInstance().Player.IsNotReclama)
                LoadAd();
        }

        #endregion


        #region Methods

        public void LoadAd()
        {
#if UNITY_EDITOR
            Debug.Log("Loading Ad: " + _adUnitId);

#endif            
            Advertisement.Load(_adUnitId, this);
        }

        public void ShowAd()
        {
            var player = GameData.GetInstance().Player;
            if (player.IsNotReclama | player.IsVip) return;
            if (_countStepForAds-- > 0) return;
            _countStepForAds = 3;

#if UNITY_EDITOR
            Debug.Log("Showing Ad: " + _adUnitId);

#endif            
            Advertisement.Show(_adUnitId, this);
            LoadAd();
        }

        public void OnUnityAdsAdLoaded(string adUnitId)
        {
#if UNITY_EDITOR
            Debug.Log("AdsLoaded");

#endif       
        }

        public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
        {
#if UNITY_EDITOR
            Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");

#endif            
            _adsLable?.SetActive(false);
        }

        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
        {
#if UNITY_EDITOR
            Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");

#endif        
        }

        public void OnUnityAdsShowStart(string adUnitId) { }

        public void OnUnityAdsShowClick(string adUnitId) { }

        public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState) { }

        #endregion   
    }
}