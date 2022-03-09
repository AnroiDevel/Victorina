using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class GameThema : MonoBehaviour
    {
        private const string Key = "Music";

        [SerializeField] private Image _backgroundWelcomeGameImg;
        [SerializeField] private Image[] _welcomeBtns;
        [SerializeField] private Image _gameProgressPanel;
        [SerializeField] private Image _gamePanel;
        [SerializeField] private Image _gameExitPanel;
        [SerializeField] private Image _winPanel;
        [SerializeField] private AudioSource _audioSource;

        private void Start()
        {
            _backgroundWelcomeGameImg.sprite = ThemaConrtoller.ActiveThema?.GameWelcomePanel;
            _gameProgressPanel.sprite = ThemaConrtoller.ActiveThema?.GameProgressPanel;
            _gamePanel.sprite = ThemaConrtoller.ActiveThema?.GamePanel;
            _gameExitPanel.sprite = ThemaConrtoller.ActiveThema?.GameExitPanel;
            _winPanel.sprite = ThemaConrtoller.ActiveThema?.WinPanel;

            foreach(var img in _welcomeBtns)
                img.sprite = ThemaConrtoller.ActiveThema?.WinPanelBtn;

            if (PlayerPrefs.HasKey(Key))
            {
                if (PlayerPrefs.GetInt(Key) > 0)
                {
                    _audioSource.clip = ThemaConrtoller.ActiveThema.GameMusic;
                    _audioSource.Play();
                }
            }
        }
    }
}