using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class AutorizationThema : MonoBehaviour
    {
        [SerializeField] private Image _confidencial;
        [SerializeField] private Image _welcomeBackground;

        private void Start()
        {
            PlayerPrefs.SetInt("Sfx", 1);
            PlayerPrefs.SetInt("Music", 1);

            _confidencial.sprite = ThemaConrtoller.ActiveThema?.Confidential;
            _welcomeBackground.sprite = ThemaConrtoller.ActiveThema?.WelcomeBackground;
        }
    }

}