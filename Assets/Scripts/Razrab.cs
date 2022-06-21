using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


namespace Victorina
{
    public partial class Razrab : MonoBehaviour
    {
        #region Fields

        private const string UrlTextFile2 = "http://a0669097.xsph.ru/Victorina/";
        private const string EmptyAnswerError = "����� �� ����� ���� ������";
        private const string EmptyQuestionError = "������� �������� ������\n";
        private const string EqualsAnswersError = "���������� ������� ���� �� ������";

        [SerializeField] private Text _question;
        [SerializeField] private InputField[] _inputFields;
        [SerializeField] private Text[] _answers;
        [SerializeField] private Text _errorSendText;

        private string _questionTxt;
        private string[] _answersTxt;
        private WWWForm _form;

        #endregion


        #region UnityMethods

        private void Start()
        {
            _answersTxt = new string[_answers.Length];
        }

        #endregion


        #region Methods

        public void PostText()
        {
            if (IsCorrectQuestion())
                StartCoroutine(Post(UrlTextFile2,
                     (string error) => Debug.Log("������: " + error),
                     (string text) => Debug.Log("����� ���������" + text)));
        }

        private IEnumerator Post(string url, Action<string> onError, Action<string> onSucces)
        {
            _form = new WWWForm();
            _form.AddField("question", _question.text);
            _form.AddField("ansverTrue", _answers[0].text);
            _form.AddField("answerFalseVariantOne", _answers[1].text);
            _form.AddField("answerFalseVariantTwo", _answers[2].text);
            _form.AddField("answerFalseVariantThree", _answers[3].text);
            _form.AddField("answerFalseVariantFour", _answers[4].text);

            UnityWebRequest unityWebRequest = UnityWebRequest.Post(url, _form);
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result == UnityWebRequest.Result.Success)
            {
                onSucces(unityWebRequest.downloadHandler.text);
                ClearAllFields();
                _errorSendText.gameObject.SetActive(true);
                _errorSendText.color = Color.green;
                _errorSendText.text = "�������";
            }
            else onError(unityWebRequest.error);
        }

        private bool IsCorrectQuestion()
        {
            var correct = true;
            _errorSendText.color = Color.red;
            _errorSendText.text = string.Empty;

            if (_question.text.Length <= 5)
            {
                correct = false;
                _errorSendText.text = EmptyQuestionError;
            }

            foreach (Text text in _answers)
            {
                if (text.text.Length <= 0)
                {
                    correct = false;
                    _errorSendText.gameObject.SetActive(true);
                    _errorSendText.text += EmptyAnswerError;
                    break;
                }
                var cnt = 0;

                for (var i = 0; i < _answers.Length; i++)
                {
                    if (_answers[i].text == text.text)
                    {
                        cnt++;
                    }

                    if (cnt > 2)
                    {
                        correct = false;
                        _errorSendText.text = EqualsAnswersError;
                        break;
                    }
                }
            }

            Debug.Log(correct);

            return correct;
        }

        private void ClearAllFields()
        {
            foreach (InputField inputField in _inputFields)
                inputField.text = string.Empty;
        }

        #endregion   
    }
}