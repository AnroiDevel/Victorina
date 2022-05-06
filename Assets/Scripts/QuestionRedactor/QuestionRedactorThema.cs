using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class QuestionRedactorThema : MonoBehaviour
    {
        [SerializeField] private Image _backgroundQuestionRedactorPanel;
        [SerializeField] private Image _helpBtn;
        [SerializeField] private Image _exitBtn;
        [SerializeField] private Image _header;
        [SerializeField] private Image _questionPanel;
        [SerializeField] private Image[] _answersFields;
        [SerializeField] private Image _sendBtn;

        [SerializeField] private Image _helpPanel;

        [SerializeField] private Text _headerTop;
        [SerializeField] private Text _headerBottom;
        [SerializeField] private Text[] _lettersCnt;
        [SerializeField] private Text _question;
        [SerializeField] private Text[] _answers;
        [SerializeField] private Text _send;


        private void Start()
        {
            var activeThema = ThemaConrtoller.ActiveThema;
            if (activeThema == null) return;

            _backgroundQuestionRedactorPanel.sprite = activeThema.QuestionRedactorBack;

            _helpBtn.sprite = activeThema.HelpBtnImg;
            _exitBtn.sprite = activeThema.ExitBtnImg;
            _header.sprite = activeThema.HeaderRedactorImg;
            _questionPanel.sprite = activeThema.QuestionRedactorPanelImg;

            foreach (var img in _answersFields)
                img.sprite = activeThema.AnswerFieldRedactorImg;

            _sendBtn.sprite = activeThema.SendRedactorBtnImg;

            _helpPanel.sprite = activeThema.RegulationPanel;

            _headerTop.color = activeThema.HeaderTopColor;
            _headerBottom.color = activeThema.HeaderBottomColor;

            foreach (var l in _lettersCnt)
                l.color = activeThema.LettersCntColor;

            _question.color = activeThema.QuestionColor;

            foreach(var answer in _answers)
                answer.color = activeThema.AnswersColor;

            _send.color = activeThema.SendBtnColor;

        }
    }
}