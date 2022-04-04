using UnityEngine;
using UnityEngine.UI;

namespace Victorina
{
    [CreateAssetMenu(fileName = "DataPlayer")]
    public class Thema : ScriptableObject
    {
        public Sprite Confidential;
        public Sprite ProfileBackground;
        public Sprite WelcomeBackground;
        public Sprite GameWelcomePanel;
        public Sprite GameProgressPanel;
        public Sprite GamePanel;
        public Sprite GameExitPanel;
        public Sprite OptionMenuBack;

        public Sprite MainMenuBack;
        public Sprite ShopMenuBack;
        public Sprite StatisticsPanelBack;
        public Sprite AchivPanelBack;

        public Sprite QuestionRedactorBack;

        [Header("Меню")]
        public Sprite BottomAnimMenuBack;
        public Sprite OptionIcon;
        public Sprite ReportedBtn;
        public Sprite ReitingIcon;
        public Sprite MainIcon;
        public Sprite ShopIcon;

        [Header("Профиль")]
        public Vector2 NamePanelAnchoredMinPosition;
        public Vector2 NamePanelAnchoredMaxPosition;

        public Vector2 ThemeBtnAnchoredMinPosition;
        public Vector2 ThemeBtnAnchoredMaxPosition;

        public Vector2 EnterFBBtnAnchoredMinPosition;
        public Vector2 EnterFBBtnAnchoredMaxPosition;

        public Vector2 SendFrendBtnAnchoredMinPosition;
        public Vector2 SendFrendBtnAnchoredMaxPosition;

        [Header("Окно выхода из приложения")]
        public Sprite ExitWindow;
        public Sprite ExitBtn;
        public Sprite NoExitBtn;

        [Header("Магазин")]
        public Sprite LoadShop;

        public AudioClip GameMusic;
        public Sprite ByeBtn;
        public Color LotShopColor;
        public Sprite WinPanel;
        public Sprite WinPanelBtn;
        public Sprite RegulationPanel;
        public Color CommentPanelColor;
    }
}