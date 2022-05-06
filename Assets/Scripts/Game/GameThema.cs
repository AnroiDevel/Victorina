using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class GameThema : MonoBehaviour
    {
        private const string Key = "Music";
        [SerializeField] private PlayerData _playerData;

        [SerializeField] private GameObject _welcomePanel;

        [SerializeField] private Image _backgroundWelcomeGameImg;
        [SerializeField] private Image[] _welcomeBtns;
        [SerializeField] private Image _gameProgressPanel;
        [SerializeField] private Image _gamePanel;
        [SerializeField] private Image _questionPanel;
        [SerializeField] private Image[] _answerBtns;

        [Header("Окно выхода")]
        [SerializeField] private Image _gameExitPanel;
        [SerializeField] private Image _exitBtnImg;
        [SerializeField] private Image _noExitBtnImg;
        [SerializeField] private Text _mainText;
        [SerializeField] private Text _exitText;
        [SerializeField] private Text _noExitText;


        [SerializeField] private AudioSource _audioSource;
        [SerializeField] private Image _regulationPanel;
        [SerializeField] private Image _commentPanel;
        [SerializeField] private Image _continueBtn;

        [Header("ResultPanel")]
        [SerializeField] private Image _winPanel;
        [SerializeField] private Image _loosePanel;
        [SerializeField] private Image[] _resultPanelBtns;

        [SerializeField] private AudioSource _crownTuk;


        private void Start()
        {
            if (_crownTuk != null)
                _crownTuk.volume = PlayerPrefs.GetInt("Sfx");

            if (!_playerData.IsTrain)
                _welcomePanel.SetActive(true);

            if (PlayerPrefs.HasKey(Key))
            {
                if (PlayerPrefs.GetInt(Key) > 0)
                {
                    _audioSource.clip = ThemaConrtoller.ActiveThema.GameMusic;
                    _audioSource.Play();
                }
            }

            var activeThema = ThemaConrtoller.ActiveThema;
            if (activeThema == null) return;

            _backgroundWelcomeGameImg.sprite = activeThema.GameWelcomePanel;

            if (_gameProgressPanel)
                _gameProgressPanel.sprite = activeThema.GameProgressPanel;

            _gamePanel.sprite = activeThema.GamePanel;
            _questionPanel.sprite = activeThema.QuestPanel;

            foreach (var answer in _answerBtns)
                answer.sprite = activeThema.AnswerBtn;

            _gameExitPanel.sprite = activeThema.ExitPlayAndGetWinPanelBack;
            _exitBtnImg.sprite = activeThema.ExitBtn;
            _noExitBtnImg.sprite = activeThema.NoExitBtn;
            _mainText.color = activeThema.MainExitPanelFontColor;
            _exitText.color = activeThema.ExitBtnFontColor;
            _noExitText.color = activeThema.NoExitBtnFontColor;

            _regulationPanel.sprite = activeThema.RegulationPanel;
            _commentPanel.color = activeThema.CommentPanelColor;
            _continueBtn.color = activeThema.LotShopColor;

            _winPanel.sprite = activeThema.WinPanel;
            _loosePanel.sprite = activeThema.LoosePanel;

            foreach (var img in _resultPanelBtns)
                img.sprite = activeThema.WinPanelBtn;

            foreach (var img in _welcomeBtns)
                img.sprite = activeThema.WinPanelBtn;

        }
    }
}