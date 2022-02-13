using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class QuestionRedactorThema : MonoBehaviour
    {
        [SerializeField] private Image _backgroundQuestionRedactorPanel;

        private void Start()
        {
            _backgroundQuestionRedactorPanel.sprite = ThemaConrtoller.ActiveThema?.QuestionRedactorBack;
        }
    }
}