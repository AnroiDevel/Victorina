using System.Collections;
using UnityEngine;


namespace Victorina
{
    public class Logo : MonoBehaviour
    {
        [SerializeField] private float _time;
        [SerializeField] private PlayerData _playerData;

        private Character _player;

        private void Awake()
        {
            var gameData = GameData.GetInstance();
            _player = gameData.Player;
        }

        private void Start()
        {
            ThemaConrtoller.SetActiveThema("GreenThema");

            PlayerPrefs.SetInt("Sfx", 1);
            PlayerPrefs.SetInt("Music", 1);

            StartCoroutine(LogoPlay());
        }


        private IEnumerator LogoPlay()
        {
            yield return new WaitForSeconds(_time);

            LoadGame();
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

            if (IsNewVersionApp())
                sceneLoader.LoadGameScene("Confidencial");
            else if (_player.Name.Length == 16)
                sceneLoader.LoadGameScene("Autorization");
            else
                sceneLoader.LoadGameScene("Victorina");
        }
    }
}