using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


namespace Victorina
{
    public class GameController : MonoBehaviour
    {
        private const string Path = "base";
        private const string Yes = "Верно";
        private const string No = "Ошибка";
        private const string Res = "Вам удалось ответить верно на все вопросы! Поздравляем!";
        private const string StartStr = "Старт";
        private const string UrlTextFile = "https://coxcombic-eliminato.000webhostapp.com/Test/question.txt";

        private readonly string[] _load = { "загрузка", "загрузка.", "загрузка..", "загрузка..." };

        [SerializeField] private GameObject _nextBtn;
        [SerializeField] private GameObject _answersPanel;
        [SerializeField] private GameObject _reloadGameBtn;
        [SerializeField] private GameObject _startBtn;
        [SerializeField] private GameObject _razrabBtn;
        [SerializeField] private GameObject _loadingImg;
        [SerializeField] private GameObject _scorePanel;
        [SerializeField] private GameObject _exitPanel;
        [SerializeField] private Text _questionText;
        [SerializeField] private Text[] _answers;
        [SerializeField] private Text _scoreText;

        private Button _start;

        private int _score;
        private string _base;
        private bool _errorConnection;

        private List<Question> _questions;
        private Question _currentQ;

        private void Start()
        {
            _start = _startBtn.GetComponent<Button>();
            GetText();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                _exitPanel.SetActive(true);
        }

        public void NextQuestion()
        {

            if (_questions.Count <= 0)
            {
                _questionText.text = Res;
                _answersPanel.SetActive(false);
                _nextBtn.SetActive(false);
                _reloadGameBtn.SetActive(true);
                return;
            }
            else
            {
                _nextBtn.SetActive(false);
                _scorePanel.SetActive(false);
                _answersPanel.SetActive(true);
            }

            var i = Random.Range(0, _questions.Count);

            _currentQ = _questions[i];
            _questionText.text = _questions[i].TextQuestion;

            List<int> index = new List<int>();

            var count = 0;

            foreach (Text text in _answers)
            {
                index.Add(count++);
            }


            while (index.Count > 0)
            {
                var r = Random.Range(0, index.Count);
                _answers[--count].text = _questions[i].Answers[index[r]];
                index.RemoveAt(r);
            }

            _questions.RemoveAt(i);
        }

        public void GetText() => StartCoroutine(Get(UrlTextFile,
                (string error) => Debug.Log("Ошибка: " + error),
                (string text) => Debug.Log("+")));



        private IEnumerator ProgressLoading(UnityWebRequest unityWebRequest)
        {
            var loadingText = _start.GetComponentInChildren<Text>();
            var ind = 0;
            if (unityWebRequest != null)
                while (unityWebRequest.downloadProgress < 1.0f)
                {
                    loadingText.text = _load[ind++];
                    yield return new WaitForSeconds(0.1f);
                    if (ind + 1 >= _load.Length)
                        ind = 0;

                }
            yield return new WaitForSeconds(1);
            _razrabBtn.SetActive(true);
            _start.GetComponentInChildren<Text>().text = StartStr;
            _start.interactable = true;
        }


        private IEnumerator Get(string url, Action<string> onError, Action<string> onSucces)
        {
            UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);

            StartCoroutine(ProgressLoading(unityWebRequest));
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError || unityWebRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                onError(unityWebRequest.error);
                _errorConnection = true;
            }
            else
            {
                onSucces(unityWebRequest.downloadHandler.text);
                _base = unityWebRequest.downloadHandler.text;
                _errorConnection = false;
            }

            CreateQuestionList();
        }

        public void CreateQuestionList()
        {
            _questions = new List<Question>();
            string[] allQuestions;
            if (_errorConnection)
            {
                var resourcesTextFile = Resources.Load<TextAsset>(Path);
                _base = resourcesTextFile.ToString();
                Debug.Log("Вопросы загружены из файла");
            }

            if (_base != null)
            {
                allQuestions = _base.Split('*');
                Debug.Log("Вопросы загружены с сайта");

                for (var i = 0; i < allQuestions.Length; i++)
                {

                    Question question = new Question();
                    var q = allQuestions[i].Split('~');
                    question.TextQuestion = q[0];
                    question.Answers = new string[q.Length - 1];

                    for (var j = 0; j < question.Answers.Length;)
                        question.Answers[j] = q[++j];

                    _questions.Add(question);
                }
            }

        }

        public void Result(int index)
        {

            if (_answers[index].text == _currentQ.Answers[0])
            {
                _questionText.text = Yes;
                _nextBtn.SetActive(true);
                _answersPanel.SetActive(false);
                _scorePanel.SetActive(true);
                _scoreText.text = (++_score).ToString();
            }
            else
            {
                _questionText.text = No;
                _answersPanel.SetActive(false);
                _reloadGameBtn.SetActive(true);
            }
        }

        public void ReloadGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void CloseApp()
        {
            Application.Quit();
        }
    }

}