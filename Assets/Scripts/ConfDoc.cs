using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class ConfDoc : MonoBehaviour
    {
        const string CONF_LINK = "https://coxcombic-eliminato.000webhostapp.com/Victorina/privacy_victorina_an_ru.html";
        const string COND_LINK = "https://coxcombic-eliminato.000webhostapp.com/Victorina/terms_victorina_ru.html";

        [SerializeField] private Text _confText;
        [SerializeField] private Text _condText;

        private void Start()
        {
            if (PlayerPrefs.HasKey("Conf"))
            {
                gameObject.SetActive(false);
                return;
            }

        }

        public void ConfirmConf()
        {
            PlayerPrefs.SetInt("Conf", 0);
        }


        public void GoToLink(string path)
        {
            if (!string.IsNullOrEmpty(path))
                Application.OpenURL(path);
        }

    }

}