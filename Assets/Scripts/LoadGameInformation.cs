using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.IO;
using UnityEngine;


namespace Victorina
{
    public class LoadGameInformation : MonoBehaviour
    {
        private const string TitleId = "D2AD8";

        [SerializeField] private Sprite _defaultAvatarTexture;

        private Character _player;
        private GameData _gameData;
        private PlayFabAccountManager _accountManager;
        private Action OnWeeklyRankReseived;

        private void Start()
        {
            _gameData = GameData.GetInstance();
            _player = _gameData.Player;
            _accountManager = PlayFabAccountManager.GetInstance();
            LoginWithAndroid();
        }


        private void LoginWithAndroid()
        {
            PlayFabSettings.staticSettings.TitleId = TitleId;

            PlayFabClientAPI.LoginWithAndroidDeviceID(new LoginWithAndroidDeviceIDRequest()
            {
                CreateAccount = true,
                AndroidDeviceId = SystemInfo.deviceUniqueIdentifier
            }, result =>
            {
                Debug.Log("Logged in");
                StartCoroutine(LoadAllAccautInfo());
            }, error => Debug.LogError(error.GenerateErrorReport()));
        }

        private IEnumerator LoadAllAccautInfo()
        {
            GetPlayerInfo();
            yield return new WaitForFixedUpdate();
            _accountManager.GetPlayerInventory();
            yield return new WaitForFixedUpdate();
            _accountManager.GetLeaderBoardWeeklyRank(_player.PlayFabId);
            yield return new WaitForFixedUpdate();
            _accountManager.GetTimeRechargeBonus();
            yield return new WaitForFixedUpdate();
            _accountManager.GetQuestionsCount();
            yield return new WaitForFixedUpdate();
            _accountManager.GetRightAnswersCount();
            yield return new WaitForFixedUpdate();
            _accountManager.GetCatalogItem("Tickets");

        }


        private void GetPlayerInfo()
        {
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(),
                  result =>
                  {
                      Debug.Log("Загружена информация об аккаунте");
                      _player.Name = result.AccountInfo.TitleInfo.DisplayName;
                      _player.CreatedAkkDate = result.AccountInfo.TitleInfo.Created;
                      _player.PlayFabId = result.AccountInfo.PlayFabId;
                      AvatarCreated();
                  },
                  error => Debug.LogError(error.GenerateErrorReport()));
        }





        private void AvatarCreated()
        {
            if (PlayerPrefs.HasKey("AvatarUrl"))
            {
                var path = PlayerPrefs.GetString("AvatarUrl");
                if (File.Exists(path))
                {
                    Texture2D texture = NativeGallery.LoadImageAtPath(path, -1);
                    var avatar = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    avatar.name = "SpriteForAvatar";

                    var scale = texture.width > texture.height ? (float)texture.width / texture.height : texture.height / (float)texture.width;
                    _player.PathToAvatar = path;
                    _player.Avatar = avatar;
                    _player.ScaleAvatarCoef = scale;
                }
            }
        }

    }



}