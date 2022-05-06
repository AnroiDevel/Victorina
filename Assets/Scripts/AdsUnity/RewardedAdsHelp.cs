using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;


namespace Victorina
{
    public class RewardedAdsHelp : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        #region Fields

        public bool IsShowAdButtonReady;

        [SerializeField] private PlayerData _playerData;

        [SerializeField] private GameObject _adsLable;

        [SerializeField] private Button _showAdButton;

        [SerializeField] private string _androidAdUnitId = "Rewarded_Android";
        //[SerializeField] private string _iOSAdUnitId = "Rewarded_iOS";

        private string _adUnitId = null; // This will remain null for unsupported platforms

        private int _countStepForAds = 3;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            // Get the Ad Unit ID for the current platform:
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
            _adUnitId = _androidAdUnitId;
#endif

            //Disable the button until the ad is ready to show:
            _showAdButton.interactable = false;
        }


        private void Start()
        {
            if (!_playerData.NotReclama)
                LoadAd();
        }

        #endregion


        #region Methods

        // Load content to the Ad Unit:
        public void LoadAd()
        {
            IsShowAdButtonReady = false;
            // IMPORTANT! Only load content AFTER initialization (in this example, initialization is handled in a different script).
            Debug.Log("Loading Ad: " + _adUnitId);
            Advertisement.Load(_adUnitId, this);
        }


        // If the ad successfully loads, add a listener to the button and enable it:
        public void OnUnityAdsAdLoaded(string adUnitId)
        {
            Debug.Log("Ad Loaded: " + adUnitId);

            if (adUnitId.Equals(_adUnitId))
            {
                // Configure the button to call the ShowAd() method when clicked:
                _showAdButton.onClick.AddListener(ShowAd);
                // Enable the button for users to click:
                //_showAdButton.interactable = true;
                IsShowAdButtonReady = true;

            }
        }


        // Implement a method to execute when the user clicks the button:
        public void ShowAd()
        {

            if (_playerData.NotReclama || _countStepForAds-- > 0)
            {
                //if (_countStepForAds == 0)
                //    _adsLable.SetActive(true);
                //else _adsLable.SetActive(false);

                return;
            }
            _countStepForAds = 3;

            IsShowAdButtonReady = false;
            // Disable the button:
            _showAdButton.interactable = false;
            // Then show the ad:
            Advertisement.Show(_adUnitId, this);
        }


        // Implement the Show Listener's OnUnityAdsShowComplete callback method to determine if the user gets a reward:
        public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
                Debug.Log("Unity Ads Rewarded Ad Completed");
                // Grant a reward.

                // Load another ad:
                Advertisement.Load(_adUnitId, this);

                IsShowAdButtonReady = false;
            }
        }


        // Implement Load and Show Listener error callbacks:
        public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
        {
            Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
            // Use the error details to determine whether to try to load another ad.
            IsShowAdButtonReady = true;

            _adsLable.SetActive(false);
        }


        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
        {
            Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
            // Use the error details to determine whether to try to load another ad.
        }


        public void OnUnityAdsShowStart(string adUnitId) { }


        public void OnUnityAdsShowClick(string adUnitId) { }


        private void OnDestroy()
        {
            // Clean up the button listeners:
            _showAdButton.onClick.RemoveAllListeners();
        }

        #endregion   
    }
}