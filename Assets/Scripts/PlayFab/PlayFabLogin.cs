using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;


namespace Victorina
{
    public class PlayFabLogin
    {
        private const string TitleId = "D2AD8";

        #region Methods

        public void LoginWithAndroid()
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

        #endregion    
    }
}