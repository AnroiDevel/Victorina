using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class MainMenuThema : MonoBehaviour
    {
        [SerializeField] private Image _backgroundMainMenu;
        [SerializeField] private Image _backgroundShopMenu;
        [SerializeField] private Image _backgroundOptionMenu;
        [SerializeField] private Image _statisticsPanel;
        [SerializeField] private Image _achivPanel;



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


        private void Start()
        {
            _backgroundMainMenu.sprite = ThemaConrtoller.ActiveThema?.MainMenuBack;
            _backgroundShopMenu.sprite = ThemaConrtoller.ActiveThema?.ShopMenuBack;
            _backgroundOptionMenu.sprite = ThemaConrtoller.ActiveThema?.OptionMenuBack;
            _statisticsPanel.sprite = ThemaConrtoller.ActiveThema?.StatisticsPanelBack;
            _achivPanel.sprite = ThemaConrtoller.ActiveThema?.AchivPanelBack;


            _bottomAnimMenuBack.sprite = ThemaConrtoller.ActiveThema?.BottomAnimMenuBack;
            _optionIcon.sprite = ThemaConrtoller.ActiveThema?.OptionIcon;
            _reitingIcon.sprite = ThemaConrtoller.ActiveThema?.ReitingIcon;
            _mainIcon.sprite = ThemaConrtoller.ActiveThema?.MainIcon;
            _shopIcon.sprite = ThemaConrtoller.ActiveThema?.ShopIcon;
            _loadShop.sprite = ThemaConrtoller.ActiveThema?.LoadShop;

            for (int i = 0; i < _sendBtns.Length; i++)
            {
                Image img = _sendBtns[i];
                img.sprite = ThemaConrtoller.ActiveThema?.ByeBtn;
                _lots[i].color = ThemaConrtoller.ActiveThema.LotShopColor;
            }

            _exitPanel.sprite = ThemaConrtoller.ActiveThema?.ExitWindow;
            _exitBtn.sprite = ThemaConrtoller.ActiveThema?.ExitBtn;
            _noExitBtn.sprite = ThemaConrtoller.ActiveThema?.NoExitBtn;
        }
    }
}