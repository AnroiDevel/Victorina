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

        }
    }
}