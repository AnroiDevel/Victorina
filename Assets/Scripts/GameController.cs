using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;


namespace Victorina
{
    public class GameController : MonoBehaviour
    {
        #region Fields

        private const string Path = "base";
        private const string Yes = "Верно\n";
        private const string No = "<color=red>Ошибка</color>\n";
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
        [SerializeField] private GameObject _logoUnity;
        [SerializeField] private Text _commentText;
        [SerializeField] private Text _questionText;
        [SerializeField] private Text[] _answers;
        [SerializeField] private Text _scoreText;

        private Button _start;

        private int _score;
        private string _base;
        private bool _errorConnection;

        private GameObject _errorAnswerGO;
        private List<Question> _questions;
        private Question _currentQ;
        private Animator _logoUnityRotate;


        #endregion


        #region Unity_Metods

        private void Start()
        {
            _start = _startBtn.GetComponent<Button>();
            _logoUnityRotate = _logoUnity.GetComponent<Animator>();
            GetText();

            float digit = 1f;
            float digit2 = 1f;
            bool isQ = digit == digit2;
            Debug.Log(isQ);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                _exitPanel.SetActive(true);
        }

        #endregion


        #region Public_Metods

        public void DeleteOneErrorAnswer()
        {
             var randomIndexErrorAnswer = Random.Range(1, _answers.Length);
            _errorAnswerGO = _answers[randomIndexErrorAnswer].GetComponentInParent<Button>().gameObject;
            _errorAnswerGO.SetActive(false);
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
                OnFiveAnswer();
            }

            var indexCurrentQuestion = Random.Range(0, _questions.Count);

            _currentQ = _questions[indexCurrentQuestion];
            _questionText.text = _questions[indexCurrentQuestion].TextQuestion;

            List<int> indexList = new List<int>();

            var countAnswers = 0;

            foreach (Text text in _answers)
            {
                indexList.Add(countAnswers++);
            }


            while (indexList.Count > 0)
            {
                var randomIndexAnswers = Random.Range(0, indexList.Count);
                print(countAnswers + " " + indexCurrentQuestion + " " + randomIndexAnswers);
                _answers[--countAnswers].text = _questions[indexCurrentQuestion].Answers[indexList[randomIndexAnswers]];
                indexList.RemoveAt(randomIndexAnswers);
            }

            _questions.RemoveAt(indexCurrentQuestion);
        }

        public void GetText() => StartCoroutine(Get(UrlTextFile,
                (string error) => Debug.Log("Ошибка: " + error),
                (string text) => Debug.Log("+")));

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

                QuestionsListGenerate(allQuestions);
            }

        }

        public void Result(int index)
        {

            if (_answers[index].text == _currentQ.Answers[0])
            {
                _commentText.text = "<color=green>" + Yes + "</color>" + '\n' + _currentQ.Comment;
                _nextBtn.SetActive(true);
                _answersPanel.SetActive(false);
                _scorePanel.SetActive(true);
                _scoreText.text = (++_score).ToString();
            }
            else
            {
                _commentText.text = No + '\n' + _currentQ.Comment;
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

        #endregion


        #region Private_Metods

        private void OnFiveAnswer()
        {
            if (_errorAnswerGO && !_errorAnswerGO.activeSelf)
                _errorAnswerGO.SetActive(true);
        }

        private IEnumerator ProgressLoading(UnityWebRequest unityWebRequest)
        {
            _logoUnityRotate.enabled = true;
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
            _logoUnityRotate.enabled = false;

            _razrabBtn.SetActive(true);
            _start.GetComponentInChildren<Text>().text = StartStr;
            _start.interactable = true;
        }

        private IEnumerator Get(string url, Action<string> onError, Action<string> onSucces)
        {
            UnityWebRequest unityWebRequest = UnityWebRequest.Get(url);

            StartCoroutine(ProgressLoading(unityWebRequest));
            yield return unityWebRequest.SendWebRequest();

            if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError || unityWebRequest.result ==  UnityWebRequest.Result.ProtocolError)
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

        private void QuestionsListGenerate(string[] allQuestions)
        {
            for (var i = 0; i < allQuestions.Length; i++)
            {
                Question question = new Question();

                var answers = 0;
                var comment = false;
                foreach (char ch in allQuestions[i])
                {
                    if (ch == '~')
                        answers++;
                    else if (ch == '☺')
                        comment = true;
                }

                var fullQuestion = allQuestions[i].Split('~', '☺');
                question.TextQuestion = fullQuestion[0];
                question.Answers = new string[answers];

                if (comment)
                    question.Comment = fullQuestion[answers + 1];

                for (var j = 0; j < question.Answers.Length;)
                    question.Answers[j] = fullQuestion[++j];

                _questions.Add(question);
            }
        }

        #endregion

        
    }

}