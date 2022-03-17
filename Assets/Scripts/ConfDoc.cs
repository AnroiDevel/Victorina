using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


namespace Victorina
{
    public class ConfDoc : MonoBehaviour
    {
        //const string CONF_LINK = "https://coxcombic-eliminato.000webhostapp.com/Victorina/privacy_victorina_an_ru.html";
        //const string COND_LINK = "https://coxcombic-eliminato.000webhostapp.com/Victorina/terms_victorina_ru.html";

        [SerializeField] private PlayerData _playerData;

        public void ConfirmConf()
        {
            PlayerPrefs.SetString("Version", Application.version);

            if (_playerData.IsNewPlayer)
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