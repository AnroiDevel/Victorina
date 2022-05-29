using UnityEngine;
using UnityEngine.SceneManagement;


namespace Victorina
{
    public class ConfDoc : MonoBehaviour
    {
        //const string CONF_LINK = "https://coxcombic-eliminato.000webhostapp.com/Victorina/privacy_victorina_an_ru.html";
        //const string COND_LINK = "https://coxcombic-eliminato.000webhostapp.com/Victorina/terms_victorina_ru.html";

        private Character _player;

        private void Awake()
        {
            var gameData = GameData.GetInstance();
            _player = gameData.Player;
        }

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
    }
}