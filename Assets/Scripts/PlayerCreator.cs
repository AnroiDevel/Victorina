using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class PlayerCreator : MonoBehaviour
    {
        private const string Key = "AutorizationKey";
        [SerializeField] private Text _name;
        [SerializeField] private PlayerData _playerData;
        private SceneLoader _sceneLoader;

        private void Start()
        {
            _sceneLoader = GetComponent<SceneLoader>();
        }

        private void OnEnable()
        {
            _playerData.NewPlayerComplete += LoadNextScene;
        }

        private void OnDisable()
        {
            _playerData.NewPlayerComplete -= LoadNextScene;
        }

        public void CreateNewPlayer()
        {
            _playerData.Name = _name.text;
            //_playerData.CreateNewPlayer();
            _playerData.IsNewPlayer = false;

        }

        //private void CreatePlayer()
        //{
        //    PlayFabSettings.staticSettings.TitleId = "D2AD8";
        //    var id = Guid.NewGuid().ToString();

        //    PlayFabClientAPI.LoginWithCustomID(
        //        new LoginWithCustomIDRequest()
        //        {
        //            CustomId = id,
        //            CreateAccount = true,
        //        },
        //        sucsess =>
        //        {
        //            PlayerPrefs.SetString(Key, id);
        //            SetDisplayName(_playerData.Name);
        //        },
        //        error => Debug.Log("sss"));
        //}

        //private void SetDisplayName(string name)
        //{
        //    PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        //    {
        //        DisplayName = name
        //    }, AddedNameComplete, error => Debug.LogError(error.GenerateErrorReport()));
        //}

        private void LoadNextScene()
        {
            _sceneLoader.LoadGameScene("Victorina");
        }
    }

}