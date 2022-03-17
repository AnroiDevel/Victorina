using PlayFab;
using PlayFab.ClientModels;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class PlayFabAccountManager : MonoBehaviour
    {
        [SerializeField] private Text _titleLabel;
        [SerializeField] private Text _workedInfoLabel;

        private PlayFabLogin _playFabLogin;

        [SerializeField] private PlayerData _playerData;

        private void Start()
        {
            _playFabLogin = FindObjectOfType<PlayFabLogin>();

            if (_playFabLogin != null)
                _playFabLogin.Login += OnLogin;

        }

        private void OnGetAccountSuccess(GetAccountInfoResult result)
        {
            _playerData.Init();
        }

        private void OnFailure(PlayFabError error)
        {
            var errorMessage = error.GenerateErrorReport();
            Debug.LogError($"Something went wrong: {errorMessage}");
        }

        private void OnLogin(bool islogin)
        {
            if (islogin)
            {
                PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(),
    OnGetAccountSuccess, OnFailure);
            }
            else
            {
                _workedInfoLabel.text += "\nНе удалось подключиться к серверу";
            }

        }
    }

}