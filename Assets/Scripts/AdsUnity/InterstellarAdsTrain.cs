using UnityEngine;
using UnityEngine.Advertisements;


namespace Victorina
{
    public class InterstellarAdsTrain : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        [SerializeField] private GameObject _adsLable;

        [SerializeField] string _androidAdUnitId = "Interstitial_Android";
        [SerializeField] string _iOsAdUnitId = "Interstitial_iOS";
        private string _adUnitId;

        private int _countStepForAds = 3;

        void Awake()
        {
            // Get the Ad Unit ID for the current platform:
            _adUnitId = (Application.platform == RuntimePlatform.IPhonePlayer)
                ? _iOsAdUnitId
                : _androidAdUnitId;
        }

        private void Start()
        {
            if (!GameData.GetInstance().Player.IsNotReclama)
                LoadAd();
        }

        // Load content to the Ad Unit:
        public void LoadAd()
        {
            // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
            Debug.Log("Loading Ad: " + _adUnitId);
            Advertisement.Load(_adUnitId, this);
        }

        // Show the loaded content in the Ad Unit:
        public void ShowAd()
        {
            if (GameData.GetInstance().Player.IsNotReclama) return;
            if (_countStepForAds-- > 0) return;
            _countStepForAds = 3;

            // Note that if the ad content wasn't previously loaded, this method will fail
            Debug.Log("Showing Ad: " + _adUnitId);
            Advertisement.Show(_adUnitId, this);
            LoadAd();
        }

        // Implement Load Listener and Show Listener interface methods: 
        public void OnUnityAdsAdLoaded(string adUnitId)
        {
            // Optionally execute code if the Ad Unit successfully loads content.
            //_adsLable?.SetActive(true);
        }

        public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Ad Unit: {adUnitId} - {error.ToString()} - {message}");
            // Optionally execute code if the Ad Unit fails to load, such as attempting to try again.
            _adsLable?.SetActive(false);
        }

        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
            // Optionally execute code if the Ad Unit fails to show, such as loading another ad.
        }

        public void OnUnityAdsShowStart(string adUnitId) { }
        public void OnUnityAdsShowClick(string adUnitId) { }
        public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState) { }
    }

}