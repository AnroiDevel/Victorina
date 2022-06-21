using UnityEngine;
using UnityEngine.SceneManagement;


namespace Victorina
{
    public class ConfDoc : MonoBehaviour
    {
        #region Fields

        private Character _player;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            var gameData = GameData.GetInstance();
            _player = gameData.Player;
        }

        #endregion


        #region Methods

        public void ConfirmConf()
        {
            PlayerPrefs.SetString("Version", Application.version);

            if (_player.Name == null)
                SceneManager.LoadScene("Autorization");
            else
                SceneManager.LoadScene("Victorina");
        }

        public void GoToLink(string path)
        {
            if (!string.IsNullOrEmpty(path))
                Application.OpenURL(path);
        }

        #endregion   
    }
}