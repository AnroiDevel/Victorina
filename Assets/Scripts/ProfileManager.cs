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

        private void Start()
        {
            _gameData = GameData.GetInstance();
            _player = _gameData.Player;

            if (_moneyText)
                _moneyText.text = _player.Money.ToString();
            if (_usernameText)
                _usernameText.text = _player.Name;
            if (_ticketText)
                _ticketText.text = _player.Tickets.ToString();

            //_imageAvatar.transform.localScale = Vector3.one * _playerData.ScaleImageAvatarCoef;
            //var textureAvatar = _playerData.Avatar;
            //_imageAvatar.sprite =  textureAvatar;


            _imageAvatar.sprite = _player.Avatar;
            _imageAvatar.transform.localScale = Vector3.one * _player.ScaleAvatarCoef;

            if (_imageAvatarRankPanel)
            {
                _imageAvatarRankPanel.sprite = _player.Avatar;
                _imageAvatarRankPanel.transform.localScale = Vector3.one * _player.ScaleAvatarCoef;
            }
        }

        //private void OnReloadAvatar()
        //{
        //    if (gameObject.activeSelf)
        //        StartCoroutine(NewAvatarComplete());
        //}

        //private IEnumerator NewAvatarComplete()
        //{
        //    yield return new WaitForEndOfFrame();
        //    yield return new WaitForSeconds(2);
        //    _imageAvatar.sprite = _playerData.Avatar;
        //}

        //private void OnEnable()
        //{
        //    OnReloadAvatar();
        //    _imageAvatar.sprite = _playerData.Avatar;
        //    _playerData.BitInfoUpdate += MoneyUpdate;
        //    _playerData.TicketInfoUpdate += TicketUpdate;

        //    var coef = _playerData.ScaleImageAvatarCoef;
        //    _imageAvatar.transform.localScale = Vector3.one * coef;
        //}

        private void TicketUpdate()
        {
            if (_ticketText)
                _ticketText.text = _playerData.TicketsBit.ToString();
        }

        private void OnDisable()
        {
            _playerData.BitInfoUpdate -= MoneyUpdate;
            _playerData.TicketInfoUpdate -= TicketUpdate;
        }

        private void MoneyUpdate()
        {
            _moneyText.text = _playerData.Bit.ToString();
        }

        //public void VerificationAccount()
        //{
        //    var newUserReq = new AddUsernamePasswordRequest
        //    {
        //        Username = _username,
        //        Password = _pass,
        //        Email = _mail
        //    };

        //    PlayFabClientAPI.AddUsernamePassword(newUserReq, OnCreateSuccess, OnFailure);

        //    _playerData.Name = _username;
        //    _playerData.Email = _mail;
        //    _playerData.Password = _pass;

        //}

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

        //public void SignIn()
        //{
        //    PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        //    {
        //        Username = _username,
        //        Password = _pass
        //    }, OnSignInSuccess, OnFailure);
        //}

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