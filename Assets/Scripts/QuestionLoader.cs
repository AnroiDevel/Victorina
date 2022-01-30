using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


namespace Victorina
{
    public class QuestionLoader : MonoBehaviour
    {
        #region Fields

        private const string URLGetQuestion = "https://coxcombic-eliminato.000webhostapp.com/Victorina/Question/GetterQuestion.php";
        private const string URLSetGrade = "https://coxcombic-eliminato.000webhostapp.com/Victorina/Question/SetGrade.php";

        [SerializeField] private Text _question;
        [SerializeField] private Button _startBtn;
        [SerializeField] private Text _timeText;
        [SerializeField] private Button[] _answerButtons;
        [SerializeField] private Text[] _answers;
        [SerializeField] private Text _status;
        [SerializeField] private Animator _animatorReload;
        [SerializeField] private Text _gradeText;
        [SerializeField] private Image[] _gradeImages;
        [SerializeField] private GameObject _ratePanel;
        [SerializeField] private Button _nextBtn;

        private Coroutine _coroutine;
        private int _trueIndex = -1;
        private float _grade;
        private float _stepGrade = 0.1f;
        private int _userGradeQuestion;
        private int _indexCurrentQuestion;

        #endregion


        #region UnityMethods

        private void OnEnable()
        {
            LoadOneQuestion();
        }

        #endregion


        #region PublicMethods
        public void LoadOneQuestion()
        {
            SetDefaultRateImage();

            StopAllCoroutines();
            StartCoroutine(GetQuestion());
        }
        public void StartingTimer()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(TimerUpdate());
        }

        public void SendAnswer(int index)
        {
            if (index == _trueIndex)
            {
                _answers[index].color = Color.green;
                _ratePanel.SetActive(true);
                if (_coroutine != null)
                    StopCoroutine(_coroutine);
            }
            else
                _answers[index].color = Color.red;
        }
        public void SetUserGrade(int value)
        {
            SetDefaultRateImage();

            for (var i = 0; i < value; i++)
            {
                _gradeImages[i].color = Color.yellow;
            }
            _userGradeQuestion = value;
            _nextBtn.gameObject.SetActive(true);
        }
        public void FiftyFifty()
        {
            var cnt = 0;
            if (_answerButtons.Length > 0)
                while (cnt < 2)
                {
                    var rnd = UnityEngine.Random.Range(0, _answers.Length);
                    if (rnd != _trueIndex)
                    {
                        if (_answerButtons[rnd].isActiveAndEnabled)
                        {
                            _answerButtons[rnd].gameObject.SetActive(false);
                            cnt++;
                        }
                    }
                }
        }
        public void SendGrade()
        {
            StartCoroutine(SendUserGrade(URLSetGrade));
        }

        #endregion


        #region PrivateMethods
        private IEnumerator GetQuestion()
        {
            _animatorReload.SetBool("IsLoading", true);
            _trueIndex = -1;

            UnityWebRequest request = UnityWebRequest.Get(URLGetQuestion);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
                _status.text = "Отсутствует связ с сервером";
            }
            else
            {
                QuestionCreated(request);
            }
        }

        private void QuestionCreated(UnityWebRequest request)
        {
            var allText = request.downloadHandler.text;
            var texts = allText.Split('☺');

            _indexCurrentQuestion = int.Parse(texts[0]);

            _question.text = texts[1];

            List<string> textsList = new List<string>();
            foreach (var text in texts)
                textsList.Add(text);

            textsList.RemoveAt(0);
            textsList.RemoveAt(0);

            textsList.RemoveAt(textsList.Count - 1);

            if (textsList[textsList.Count - 1].Length > 1)
                if (textsList[textsList.Count - 1][1] == '.')
                {
                    IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
                    _grade = float.Parse(textsList[textsList.Count - 1], formatter);
                }
                else
                    _grade = float.Parse(textsList[textsList.Count - 1]);

            _gradeText.text = ((int)_grade).ToString();

            textsList.RemoveAt(textsList.Count - 1);

            while (textsList.Count > 0)
            {
                int rnd = UnityEngine.Random.Range(0, textsList.Count);
                _answers[textsList.Count - 1].text = textsList[rnd];
                textsList.RemoveAt(rnd);

                if (rnd == 0 && _trueIndex == -1)
                    _trueIndex = textsList.Count;
            }

            _animatorReload.SetBool("IsLoading", false);

            SetDefaultColorAnswersText();

            if (!_startBtn.gameObject.activeInHierarchy)
                _startBtn.gameObject.SetActive(true);
            StartingTimer();
        }

        private void SetDefaultColorAnswersText()
        {
            foreach (var answer in _answers)
                answer.color = Color.black;

            foreach (var answer in _answerButtons)
                if (!answer.isActiveAndEnabled)
                    answer.gameObject.SetActive(true);
        }

        private IEnumerator TimerUpdate()
        {
            var tempTime = System.DateTime.MinValue;
            while (true)
            {

                _timeText.text = tempTime.ToString("T", DateTimeFormatInfo.InvariantInfo);
                tempTime = tempTime.AddSeconds(1);
                yield return new WaitForSeconds(1);
            }
        }

        private void SetDefaultRateImage()
        {
            foreach (var img in _gradeImages)
                img.color = Color.white;
        }

        private IEnumerator SendUserGrade(string url)
        {
            _grade = _grade > _userGradeQuestion ? _grade - _stepGrade : _grade + _stepGrade;
            var textGrade = _grade.ToString();

            WWWForm form = new WWWForm();
            form.AddField("grade", textGrade);
            form.AddField("id", _indexCurrentQuestion);

            using (UnityWebRequest request = UnityWebRequest.Post(url, form))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                    Debug.Log("Оценку вопроса отправить не удалось");
            }

            LoadOneQuestion();

        }

        #endregion    
    }
}