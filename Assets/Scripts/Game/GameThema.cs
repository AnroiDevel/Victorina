using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class GameThema : MonoBehaviour
    {
        [SerializeField] private Image _backgroundWelcomeGameImg;
        [SerializeField] private Image _gameProgressPanel;
        [SerializeField] private Image _gamePanel;
        [SerializeField] private Image _gameExitPanel;

        private void Start()
        {
            _backgroundWelcomeGameImg.sprite = ThemaConrtoller.ActiveThema?.GameWelcomePanel;
            _gameProgressPanel.sprite = ThemaConrtoller.ActiveThema?.GameProgressPanel;
            _gamePanel.sprite = ThemaConrtoller.ActiveThema?.GamePanel;
            _gameExitPanel.sprite = ThemaConrtoller.ActiveThema?.GameExitPanel;
        }
    }
}