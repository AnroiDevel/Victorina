using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;


namespace Victorina
{
    public class RewardedAdsButton : MonoBehaviour, IUnityAdsLoadListener, IUnityAdsShowListener
    {
        #region Fields

        public bool IsShowAdButtonReady;
        [SerializeField] private BonusManager _bonusManager;
        [SerializeField] private GameObject _adsLable;
        [SerializeField] private GameObject _bonusOpenPanel;
        [SerializeField] private Button _showAdButton;
        [SerializeField] private string _androidAdUnitId = "Rewarded_Android";
        //[SerializeField] private string _iOSAdUnitId = "Rewarded_iOS";
        private string _adUnitId = null;

        #endregion


        #region UnityMethods

        private void Awake()
        {
#if UNITY_IOS
        _adUnitId = _iOSAdUnitId;
#elif UNITY_ANDROID
            _adUnitId = _androidAdUnitId;
#endif
            _showAdButton.interactable = false;
        }

        #endregion


        #region Methods

        public void LoadAd()
        {
            IsShowAdButtonReady = false;
#if UNITY_EDITOR
            Debug.Log("Loading Ad: " + _adUnitId);

#endif            
            Advertisement.Load(_adUnitId, this);
        }

        public void OnUnityAdsAdLoaded(string adUnitId)
        {
#if UNITY_EDITOR
            Debug.Log("Ad Loaded: " + adUnitId);
#endif
            if (adUnitId.Equals(_adUnitId))
            {
                _showAdButton.onClick.AddListener(ShowAd);
                IsShowAdButtonReady = true;
                _adsLable?.SetActive(true);
            }
        }

        private void ShowAd()
        {
            _bonusOpenPanel.SetActive(true);
            if (GameData.GetInstance().Player.IsNotReclama)
            {
                return;
            }
            IsShowAdButtonReady = false;
            _showAdButton.interactable = false;
            Advertisement.Show(_adUnitId, this);
        }

        public void OnUnityAdsShowComplete(string adUnitId, UnityAdsShowCompletionState showCompletionState)
        {
            if (adUnitId.Equals(_adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
            {
#if UNITY_EDITOR
                Debug.Log("Unity Ads Rewarded Ad Completed");
#endif
                IsShowAdButtonReady = false;
                _bonusOpenPanel.SetActive(true);
            }
        }

        public void OnUnityAdsFailedToLoad(string adUnitId, UnityAdsLoadError error, string message)
        {
#if UNITY_EDITOR
            Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
#endif     
            IsShowAdButtonReady = true;
            _adsLable.SetActive(false);
        }

        public void OnUnityAdsShowFailure(string adUnitId, UnityAdsShowError error, string message)
        {
#if UNITY_EDITOR
            Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
#endif        
        }

        public void OnUnityAdsShowStart(string adUnitId) { }

        public void OnUnityAdsShowClick(string adUnitId) { }

        private void OnDestroy()
        {
            _showAdButton.onClick.RemoveAllListeners();
        }

        #endregion
    }
}