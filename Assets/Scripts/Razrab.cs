using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


namespace Victorina
{
    public partial class Razrab : MonoBehaviour
    {
        [SerializeField] private Text _question;
        [SerializeField] private InputField[] _inputFields;
        [SerializeField] private Text[] _answers;
        [SerializeField] private Text _errorSendText;

        private string _questionTxt;
        private string[] _answersTxt;

        private const string UrlTextFile = "https://coxcombic-eliminato.000webhostapp.com/Test/";
        private const string EmptyAnswerError = "Ответ не может быть пустым\n";
        private const string EmptyQuestionError = "Отсутствует вопрос\n";
        private const string EqualsAnswersError = "Одинаковых ответов быть не должно";

        private void Start()
        {
            _answersTxt = new string[_answers.Length];
        }

        private WWWForm _form;

        public void PostText()
        {
            if (IsCorrectQuestion())
                StartCoroutine(Post(UrlTextFile,
                     (string error) => Debug.Log("Ошибка: " + error),
                     (string text) => Debug.Log("Текст отправлен" + text)));
        }

        private IEnumerator Post(string url, Action<string> onError, Action<string> onSucces)
        {
            _form = new WWWForm();
            var str = SendQuestionToBase() + "\n";
            _form.AddField("question", str);
            _form.AddField("question2", str);
            _form.AddField("question3", str);

            UnityWebRequest unityWebRequest = UnityWebRequest.Post(url, _form);
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result == UnityWebRequest.Result.Success)
            {
                onSucces(unityWebRequest.downloadHandler.text);
                ClearAllFields();
                _errorSendText.gameObject.SetActive(true);
                _errorSendText.color = Color.green;
                _errorSendText.text = "Успешно";
            }
            else onError(unityWebRequest.error);
        }


        private string SendQuestionToBase()
        {
            var qStr = string.Empty;
            _questionTxt = "*\n";
            _questionTxt += _question.text + "\n";
            qStr += _questionTxt;
            for (var i = 0; i < _answers.Length; i++)
            {
                _answersTxt[i] = "~" + _answers[i].text + "\n";
                qStr += _answersTxt[i];
            }

            return qStr;
        }

        private bool IsCorrectQuestion()
        {
            var correct = true;
            _errorSendText.color = Color.red;
            _errorSendText.text = string.Empty;

            if (_question.text.Length <= 0)
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

                    if (cnt >= 2)
                    {
                        correct = false;
                        _errorSendText.text = EqualsAnswersError;
                        break;
                    }

                }
            }
            return correct;
        }

        private void ClearAllFields()
        {
            foreach (InputField inputField in _inputFields)
                inputField.text = string.Empty;
        }
    }

}