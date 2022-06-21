using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class ProfileManager : MonoBehaviour
    {
        #region Fields

        private Character _player;
        private GameData _gameData;

        [SerializeField] private Image _imageAvatar;
        [SerializeField] private Image _imageAvatarRankPanel;
        [SerializeField] private Text _workedInfoLabel;
        [SerializeField] private Text _usernameText;
        [SerializeField] private Text _moneyText;
        [SerializeField] private Text _ticketText;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            _gameData = GameData.GetInstance();
            _player = _gameData.Player;
        }

        private void Start()
        {
            PlayFabAccountManager.Instance.GetPlayerInventory();

            if (_moneyText)
                _moneyText.text = _player.Money.ToString();
            if (_usernameText)
                _usernameText.text = _player.Name;
            if (_ticketText)
                _ticketText.text = _player.Tickets.ToString();

            _imageAvatar.sprite = _player.Avatar;
            _imageAvatar.transform.localScale = Vector3.one * _player.ScaleAvatarCoef;

            if (_imageAvatarRankPanel)
            {
                _imageAvatarRankPanel.sprite = _player.Avatar;
                _imageAvatarRankPanel.transform.localScale = Vector3.one * _player.ScaleAvatarCoef;
            }
        }

        #endregion


        #region Methods

        public void AddOrUpdateContactEmail(string emailAddress)
        {
            var request = new AddOrUpdateContactEmailRequest
            {
                EmailAddress = emailAddress
            };
            PlayFabClientAPI.AddOrUpdateContactEmail(request, result =>
            {
                Debug.Log("The player's account has been updated with a contact email");
            }, FailureCallback);
        }

        private void FailureCallback(PlayFabError error)
        {
            Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
            Debug.LogError(error.GenerateErrorReport());
        }

        #endregion  
    }
}