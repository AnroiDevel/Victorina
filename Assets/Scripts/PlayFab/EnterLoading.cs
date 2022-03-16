using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;


namespace Victorina
{
    public class EnterLoading : MonoBehaviour
    {
        private const string AuthGuidKey = "authorization-guid";
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private bool _isNewUser;

        private void Start()
        {
            if (!_isNewUser)
                PlayFabAutorization(_playerData);
        }

        private void PlayFabAutorization(PlayerData playerData)
        {
            if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            {
                PlayFabSettings.staticSettings.TitleId = "D2AD8";
            }

            var needCreation = PlayerPrefs.HasKey(AuthGuidKey);
            var id = PlayerPrefs.GetString(AuthGuidKey, Guid.NewGuid().ToString());

            PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
            {
                CustomId = id,
                CreateAccount = !needCreation,

            }, success =>
            {
                PlayerPrefs.SetString(AuthGuidKey, id);
                playerData.Init();

            }, Debug.Log);
        }

    }
}
