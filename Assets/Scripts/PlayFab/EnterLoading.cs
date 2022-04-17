using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class EnterLoading : MonoBehaviour
    {
        private const string AuthGuidKey = "authorization-guid";
        [SerializeField] private PlayerData _playerData;

        [Header("Для разработчика")]
        [SerializeField] private Toggle _setNewPlayerToogle;
        [SerializeField] private Sprite _defaultAvatar;


        private void Start()
        {
            _playerData.IsNewVersionApp = IsNewVersionApp();
            if (!_playerData.IsNewPlayer)
                _playerData.Login();
            //SetIsNewPlayerOption();
        }

        //private void PlayFabAutorization(PlayerData playerData)
        //{
        //    if (_playerData.IsNewPlayer)
        //    {
        //        playerData.Reset();
        //        _playerData.Avatar = _defaultAvatar;
        //    }

        //    if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
        //    {
        //        PlayFabSettings.staticSettings.TitleId = "D2AD8";
        //    }

        //    var needCreation = _playerData.GuidID == string.Empty;

        //    var id = _playerData.GuidID;
        //    if (id == string.Empty)
        //        id = Guid.NewGuid().ToString();
        //    _playerData.GuidID = id;

        //    PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
        //    {
        //        CustomId = id,
        //        CreateAccount = needCreation,

        //    }, success =>
        //    {
        //        PlayerPrefs.SetString(AuthGuidKey, id);
        //        playerData.Init();
        //    }, Debug.Log);
        //}

        public void SetIsNewPlayerOption()
        {
            _playerData.IsNewPlayer = _setNewPlayerToogle.isOn;
            GetComponent<SceneLoader>().LoadGameScene("Autorization");
        }

        private bool IsNewVersionApp()
        {
            var currentVersion = Application.version;
            string prevVersion = string.Empty;

            if (PlayerPrefs.HasKey("Version"))
                prevVersion = PlayerPrefs.GetString("Version");

            return !currentVersion.Equals(prevVersion);
        }

    }
}
