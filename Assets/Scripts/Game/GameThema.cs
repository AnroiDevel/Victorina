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
        [SerializeField] private Image _gameExitPanel;
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

            if(!_playerData.IsTrain)
            _welcomePanel.SetActive(true);

            _backgroundWelcomeGameImg.sprite = ThemaConrtoller.ActiveThema?.GameWelcomePanel;

            if (_gameProgressPanel)
                _gameProgressPanel.sprite = ThemaConrtoller.ActiveThema?.GameProgressPanel;

            _gamePanel.sprite = ThemaConrtoller.ActiveThema?.GamePanel;
            _questionPanel.sprite = ThemaConrtoller.ActiveThema?.QuestPanel;

            foreach (var answer in _answerBtns)
                answer.sprite = ThemaConrtoller.ActiveThema?.AnswerBtn;

            _gameExitPanel.sprite = ThemaConrtoller.ActiveThema?.GameExitPanel;
            _regulationPanel.sprite = ThemaConrtoller.ActiveThema?.RegulationPanel;
            _commentPanel.color = ThemaConrtoller.ActiveThema.CommentPanelColor;
            _continueBtn.color = ThemaConrtoller.ActiveThema.LotShopColor;

            _winPanel.sprite = ThemaConrtoller.ActiveThema?.WinPanel;
            _loosePanel.sprite = ThemaConrtoller.ActiveThema?.LoosePanel;

            foreach (var img in _resultPanelBtns)
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