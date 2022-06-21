using System.Collections;
using UnityEngine;


namespace Victorina
{
    public class Logo : MonoBehaviour
    {
        #region Fields

        [SerializeField] private float _time;
        [SerializeField] private Thema _defaultThema;
        private Character _player;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            var gameData = GameData.GetInstance();
            _player = gameData.Player;
        }

        private void Start()
        {
            ThemaConrtoller.SetActiveThema(_defaultThema.name);

            PlayerPrefs.SetInt("Sfx", 1);
            PlayerPrefs.SetInt("Music", 1);

            StartCoroutine(LogoPlay());
        }

        #endregion


        #region Methods

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
            {
                sceneLoader.LoadGameScene("Confidencial");
                PlayerPrefs.SetInt("MarkReview", 0);
            }
            else if (_player.Name == null)
            {
                PlayerPrefs.DeleteKey("AvatarUrl");
                sceneLoader.LoadGameScene("Autorization");
            }
            else
                sceneLoader.LoadGameScene("Victorina");
        }

        #endregion    
    }
}