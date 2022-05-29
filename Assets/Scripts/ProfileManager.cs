using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class ProfileManager : MonoBehaviour
    {
        [SerializeField] private Image _imageAvatar;
        [SerializeField] private Image _imageAvatarRankPanel;

        [SerializeField] private Text _workedInfoLabel;

        private string _username;
        private string _mail;
        private string _pass;

        [SerializeField] private Text _usernameText;
        [SerializeField] private Text _moneyText;
        [SerializeField] private Text _ticketText;

        private Character _player;
        private GameData _gameData;

        private void Awake()
        {
            _gameData = GameData.GetInstance();
            _player = _gameData.Player;
        }


        private void Start()
        {
            PlayFabAccountManager.GetInstance().GetPlayerInventory();


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


        private void OnCreateSuccess(AddUsernamePasswordResult result)
        {
            Debug.Log($"Creation Success: {_username}");
            AddOrUpdateContactEmail(_mail);
        }

        public void UpdateUsername(string username)
        {
            _username = username;
        }

        public void UpdateEmail(string mail)
        {
            _mail = mail;
        }

        public void UpdatePassword(string pass)
        {
            _pass = pass;
        }


        private void OnSignInSuccess(LoginResult result)
        {
            Debug.Log($"Sign In Success: {_username}");
        }

        private void OnFailure(PlayFabError error)
        {
            var errorMessage = error.GenerateErrorReport();
            _workedInfoLabel.text += errorMessage;
        }

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
    }
}