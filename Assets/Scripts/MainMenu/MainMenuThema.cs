using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class MainMenuThema : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Image _backgroundMainMenu;
        [SerializeField] private Image _backgroundShopMenu;
        [SerializeField] private Image _achivPanel;

        [Header("Кнопки главного экрана")]
        [SerializeField] private Image _bonusBtn;
        [SerializeField] private Image _bonusTimerBack;
        [SerializeField] private Image _trainBtn;
        [SerializeField] private Image _questionRedactorBtn;
        [SerializeField] private Image _playBtn;

        [Header("Настройки")]
        [SerializeField] private Image _backgroundOptionMenu;
        [SerializeField] private Image _reportedBtn;

        [Header("Меню")]
        [SerializeField] private Image _bottomAnimMenuBack;
        [SerializeField] private Image _optionIcon;
        [SerializeField] private Image _reitingIcon;
        [SerializeField] private Image _mainIcon;
        [SerializeField] private Image _shopIcon;

        [Header("Магазин")]
        [SerializeField] private Image[] _sendBtns;
        [SerializeField] private Image[] _lots;
        [SerializeField] private Image _loadShop;

        [Header("Выход")]
        [SerializeField] private Image _exitPanel;
        [SerializeField] private Image _exitBtn;
        [SerializeField] private Image _noExitBtn;
        [SerializeField] private Text _exitText;
        [SerializeField] private Text _noExitText;

        [Header("Статистика")]
        [SerializeField] private Image _statisticsPanel;
        [SerializeField] private Image _topLineStatic;
        [SerializeField] private Image _topLineMoveble;
        [SerializeField] private Image[] _cellsStatistic;

        #endregion


        #region UnityMethods

        private void Start()
        {
            var activeThema = ThemaConrtoller.ActiveThema;

            if (activeThema == null) return;

            _bonusBtn.sprite = activeThema.BonusBtnBack;
            _bonusTimerBack.sprite = activeThema.BonusTimerBack;
            _trainBtn.sprite = activeThema.TrainBtnBack;
            _questionRedactorBtn.sprite = activeThema.QuestionRedactorBtn;
            _playBtn.sprite = activeThema.PlayBtnBack;

            _backgroundMainMenu.sprite = activeThema.MainMenuBack;
            _backgroundShopMenu.sprite = activeThema.ShopMenuBack;

            _backgroundOptionMenu.sprite = activeThema.OptionMenuBack;
            _reportedBtn.sprite = activeThema.ReportedBtn;

            _statisticsPanel.sprite = activeThema.StatisticsPanelBack;
            _topLineStatic.sprite = activeThema.TopLineStatic;
            _topLineMoveble.sprite = activeThema.TopLineMoveble;
            foreach (var cell in _cellsStatistic)
                cell.sprite = activeThema.CellStatistic;

            _achivPanel.sprite = activeThema.AchivPanelBack;

            _bottomAnimMenuBack.sprite = activeThema.BottomAnimMenuBack;
            _optionIcon.sprite = activeThema.OptionIcon;
            _reitingIcon.sprite = activeThema.ReitingIcon;
            _mainIcon.sprite = activeThema.MainIcon;
            _shopIcon.sprite = activeThema.ShopIcon;
            _loadShop.sprite = activeThema.LoadShop;

            for (int i = 0; i < _sendBtns.Length; i++)
            {
                Image img = _sendBtns[i];
                img.sprite = activeThema.ByeBtn;
                _lots[i].color = activeThema.LotShopColor;
            }

            _exitPanel.sprite = activeThema.ExitWindow;
            _exitBtn.sprite = activeThema.ExitBtn;
            _noExitBtn.sprite = activeThema.NoExitBtn;
            _exitText.color = activeThema.ExitBtnFontColor;
            _noExitText.color = activeThema.NoExitBtnFontColor;
        }

        #endregion    
    }
}