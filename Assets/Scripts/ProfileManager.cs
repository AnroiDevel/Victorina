using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class ProfileManager : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;

        [SerializeField] private Button _createAkkBtn;
        [SerializeField] private Button _signInAkkBtn;
        [SerializeField] private Button _signOutAkkBtn;

        [SerializeField] private Text _workedInfoLabel;

        private string _username;
        private string _mail;
        private string _pass;

        [SerializeField] private Text _moneyText;

        private void Start()
        {
            _moneyText.text = _playerData.Bit.ToString();
        }

        private void OnEnable()
        {
            _playerData.BitInfoUpdate += MoneyUpdate;
        }

        private void OnDisable()
        {
            _playerData.BitInfoUpdate -= MoneyUpdate;
        }

        private void MoneyUpdate()
        {
            _moneyText.text = _playerData.Bit.ToString();
        }

        public void VerificationAccount()
        {
            var newUserReq = new AddUsernamePasswordRequest
            {
                Username = _username,
                Password = _pass,
                Email = _mail
            };

            PlayFabClientAPI.AddUsernamePassword(newUserReq, OnCreateSuccess, OnFailure);

            _playerData.Name = _username;
            _playerData.Email = _mail;
            _playerData.Password = _pass;

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

        public void SignIn()
        {
            PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
            {
                Username = _username,
                Password = _pass
            }, OnSignInSuccess, OnFailure);
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