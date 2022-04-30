using System.Collections;
using UnityEngine;


namespace Victorina
{
    public class Logo : MonoBehaviour
    {
        [SerializeField] private float _time;
        [SerializeField] private PlayerData _playerData;

        private void Start()
        {
            ThemaConrtoller.SetActiveThema("FuturismThema");

            //PlayerPrefs.DeleteAll();

            //_playerData.SetAvatar();

            PlayerPrefs.SetInt("Sfx", 1);
            PlayerPrefs.SetInt("Music", 1);

            StartCoroutine(LogoPlay());
            //_playerData.LoginComplete += LoadGame;

            if (PlayerPrefs.HasKey("Id"))
            {
                _playerData.IsNewPlayer = false;
                _playerData.IsNewPlayer = IsNewVersionApp();
                _playerData.Login();
            }

        }



        private IEnumerator LogoPlay()
        {
            yield return new WaitForSeconds(_time);

            LoadGame();
        }

        private void LoadNextScena()
        {
            var sceneLoader = GetComponent<SceneLoader>();
            if (_playerData.IsNewVersionApp || _playerData.IsNewPlayer)
                sceneLoader.LoadGameScene("Confidencial");
            else
                _playerData.Login();
        }
        private bool IsNewVersionApp()
        {
            var currentVersion = Application.version;
            string prevVersion = string.Empty;

            if (PlayerPrefs.HasKey("Version"))
                prevVersion = PlayerPrefs.GetString("Version");

            return !currentVersion.Equals(prevVersion);
        }

        private void LoadGame()
        {
            var sceneLoader = GetComponent<SceneLoader>();


            if (_playerData.IsNewPlayer || _playerData.IsNewVersionApp)
                sceneLoader.LoadGameScene("Confidencial");
            else
                sceneLoader.LoadGameScene("Victorina");
        }
    }

}