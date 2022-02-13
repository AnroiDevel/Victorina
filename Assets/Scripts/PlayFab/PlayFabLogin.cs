using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Victorina
{
    public class PlayFabLogin : MonoBehaviour
    {
        private const string AuthGuidKey = "authorization-guid";

        private string _username;
        private string _mail;
        private string _pass;

        public Action<bool> Login;

        [SerializeField] private Text _workedInfoLabel;
        [SerializeField] private Text _loadText;
        [SerializeField] private Button _playAndExitButton;

        [SerializeField] private PlayerData _playerData;

        public void Start()
        {

            // Here we need to check whether TitleId property is configured in settings or not
            if (string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            {
                /*
                * If not we need to assign it to the appropriate variable manually
                * Otherwise we can just remove this if statement at all
                */
                PlayFabSettings.staticSettings.TitleId = "D2AD8";
            }

            var needCreation = PlayerPrefs.HasKey(AuthGuidKey);
            var id = PlayerPrefs.GetString(AuthGuidKey, Guid.NewGuid().ToString());

            PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
            {
                CustomId = id,
                CreateAccount = !needCreation
            }, success =>
            {
                PlayerPrefs.SetString(AuthGuidKey, id);
                Login?.Invoke(true);

                _loadText.text = "Загружено";
                _playAndExitButton.gameObject.SetActive(true);

                _playAndExitButton.onClick.AddListener(() => SceneManager.LoadScene("Victorina"));
            }, OnFailure);

        }

        private void OnFailure(PlayFabError error)
        {
            var errorMessage = error.GenerateErrorReport();
            _workedInfoLabel.text += errorMessage;

            Login?.Invoke(false);
            _playAndExitButton.onClick.AddListener(Exit);
            _playAndExitButton.gameObject.SetActive(true);
            _playAndExitButton.GetComponentInChildren<Text>().text = "Выход";
        }


        private void Exit()
        {
            _workedInfoLabel.text += "\nВыходим из приложения";
            Application.Quit();

        }


        public void ResetProgress()
        {
            PlayerPrefs.DeleteAll();
            SceneManager.LoadScene("Autorization");
        }



    }
}