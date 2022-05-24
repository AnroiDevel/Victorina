using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Victorina
{
    public  class PlayFabLogin
    {
        private const string TitleId = "D2AD8";


        public  void LoginWithAndroid()
        {
            if (PlayFabClientAPI.IsClientLoggedIn()) return;

            PlayFabSettings.staticSettings.TitleId = TitleId;

            PlayFabClientAPI.LoginWithAndroidDeviceID(new LoginWithAndroidDeviceIDRequest()
            {
                CreateAccount = true,
                AndroidDeviceId = SystemInfo.deviceUniqueIdentifier
            }, result =>
            {
                Debug.Log("Logged in");
            }, error => Debug.LogError(error.GenerateErrorReport()));
        }

    }
}