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

        //[SerializeField] private int _minLengthQuestion = 5;

        private string _questionTxt;
        private string[] _answersTxt;
        

        private const string UrlTextFile = "https://coxcombic-eliminato.000webhostapp.com/Test/";
        private const string UrlTextFile2 = "http://a0669097.xsph.ru/Victorina/";
        private const string EmptyAnswerError = "Ответ не может быть пустым\n";
        private const string EmptyQuestionError = "Слишком короткий вопрос\n";
        private const string EqualsAnswersError = "Одинаковых ответов быть не должно\n";

        private void Start()
        {
            _answersTxt = new string[_answers.Length];
        }

        private WWWForm _form;

        public void PostText()
        {
            if (IsCorrectQuestion())
                StartCoroutine(Post(UrlTextFile2,
                     (string error) => Debug.Log("Ошибка: " + error),
                     (string text) => Debug.Log("Текст отправлен" + text)));
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
            //foreach (InputField inputField in _inputFields)
            //    inputField.text = string.Empty;
        }
    }

}