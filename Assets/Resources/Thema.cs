using UnityEngine;


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

        public Sprite MainMenuBack;
        public Sprite ShopMenuBack;
        public Sprite StatisticsPanelBack;
        public Sprite AchivPanelBack;

        [Header("Меню")]
        public Sprite BottomAnimMenuBack;
        public Sprite OptionIcon;
        public Sprite ReitingIcon;
        public Sprite MainIcon;
        public Sprite ShopIcon;
        public Sprite OptionMenuBack;
    }
}