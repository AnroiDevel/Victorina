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
        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Image _regulationPanel;
        [SerializeField] private Image _commentPanel;
        [SerializeField] private Image _continueBtn;

        [Header("ResultPanel")]
        [SerializeField] private Image _winPanel;
        [SerializeField] private Image _loosePanel;
        [SerializeField] private Image[] _resultPanelBtns;

        private void Start()
        {
            _backgroundWelcomeGameImg.sprite = ThemaConrtoller.ActiveThema?.GameWelcomePanel;
            _gameProgressPanel.sprite = ThemaConrtoller.ActiveThema?.GameProgressPanel;
            _gamePanel.sprite = ThemaConrtoller.ActiveThema?.GamePanel;
            _gameExitPanel.sprite = ThemaConrtoller.ActiveThema?.GameExitPanel;
            _regulationPanel.sprite = ThemaConrtoller.ActiveThema?.RegulationPanel;
            _commentPanel.color = ThemaConrtoller.ActiveThema.CommentPanelColor;
            _continueBtn.color = ThemaConrtoller.ActiveThema.LotShopColor;

            _winPanel.sprite = ThemaConrtoller.ActiveThema?.WinPanel;
            _loosePanel.sprite = ThemaConrtoller.ActiveThema?.LoosePanel;

            foreach(var img in _resultPanelBtns)
                img.sprite = ThemaConrtoller.ActiveThema?.WinPanelBtn;

            foreach (var img in _welcomeBtns)
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