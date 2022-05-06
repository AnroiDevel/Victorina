using UnityEngine;
using UnityEngine.UI;

namespace Victorina
{
    [CreateAssetMenu(fileName = "DataPlayer")]
    public class Thema : ScriptableObject
    {
        public Sprite Confidential;
        public Sprite WelcomeBackground;

        public Sprite OptionMenuBack;

        [Header("Главное меню")]
        public Sprite MainMenuBack;
        public Sprite BonusBtnBack;
        public Sprite BonusTimerBack;
        public Sprite TrainBtnBack;
        public Sprite QuestionRedactorBtn;
        public Sprite PlayBtnBack;

        public Sprite ShopMenuBack;
        public Sprite AchivPanelBack;


        [Header("Редактор")]
        public Sprite QuestionRedactorBack;
        public Sprite HelpBtnImg;
        public Sprite ExitBtnImg;
        public Sprite HeaderRedactorImg;
        public Sprite QuestionRedactorPanelImg;
        public Sprite AnswerFieldRedactorImg;
        public Sprite SendRedactorBtnImg;

        public Color HeaderTopColor;
        public Color HeaderBottomColor;
        public Color LettersCntColor;
        public Color QuestionColor;
        public Color AnswersColor;
        public Color SendBtnColor;



        [Header("Режим игры")]
        public Sprite GameWelcomePanel;
        public Sprite GameProgressPanel;
        public Sprite GamePanel;
        public Sprite QuestPanel;
        public Sprite AnswerBtn;

        [Header("Меню")]
        public Sprite BottomAnimMenuBack;
        public Sprite OptionIcon;
        public Sprite ReportedBtn;
        public Sprite ReitingIcon;
        public Sprite MainIcon;
        public Sprite ShopIcon;

        [Header("Профиль")]
        public Sprite ProfileBackground;
        public Sprite ExitProfileBtn;
        public Sprite LineImg;
        public Sprite ThemaBtnImg;

        [Header("Окно выхода из приложения")]
        public Sprite ExitWindow;
        public Sprite ExitBtn;
        public Sprite NoExitBtn;
        public Color MainExitPanelFontColor;
        public Color ExitBtnFontColor;
        public Color NoExitBtnFontColor;

        [Header("Магазин")]
        public Sprite LoadShop;

        public AudioClip GameMusic;
        public Sprite ByeBtn;
        public Color LotShopColor;
        public Sprite WinPanel;
        public Sprite WinPanelBtn;
        public Sprite RegulationPanel;
        public Color CommentPanelColor;
        public Sprite LoosePanel;
        public Sprite ExitPlayAndGetWinPanelBack;

        [Header("Статистика")]
        public Sprite StatisticsPanelBack;
        public Sprite TopLineStatic;
        public Sprite TopLineMoveble;
        public Sprite CellStatistic;
    }
}