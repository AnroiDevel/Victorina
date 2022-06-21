using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


namespace Victorina
{
    public class QuestionLoader : MonoBehaviour
    {
        #region Fields

        public Toggle _endQuestion;

        private const string URLGetQuestion = "http://a0669097.xsph.ru/Victorina/Question/GetterQuestion.php";
        private const string URLSetGrade = "http://a0669097.xsph.ru/Victorina/Question/SetGrade.php";
        private const string LoadingQuestion = "загрузка вопроса";
        private const string GradeInfo = "Оцените сложность";
        private const string RightResult = "Верно";
        private const string ErrorResult = "Ошибка";

        [SerializeField] private RateStarsSelector _rateStarsSelector;

        [SerializeField] private Text _question;
        [SerializeField] private Text _timeText;
        [SerializeField] private Text[] _answers;
        [SerializeField] private Text _status;
        [SerializeField] private Text _gradeText;
        [SerializeField] private Text _comment;
        [SerializeField] private Text _result;
        [SerializeField] private Button _startBtn;
        [SerializeField] private Button[] _answerButtons;
        [SerializeField] private Image[] _progressCells;
        [SerializeField] private Color _defaultColor;
        [SerializeField] private Color _activeColor;
        [SerializeField] private TMP_ColorGradient _colorGradient;

        [SerializeField] private GameObject _ratePanel;
        [SerializeField] private GameObject _progressPanel;
        [SerializeField] private GameObject _resultPanel;
        [SerializeField] private GameObject _victory;
        [SerializeField] private GameObject _loose;
        [SerializeField] private GameObject _commentPanel;
        [SerializeField] private GameObject _reviewPanel;

        [SerializeField] private AudioSource _backAudio;
        [SerializeField] private AudioSource _winAudio;
        [SerializeField] private AudioSource _looseAudio;
        [SerializeField] private Animator _animatorReload;
        [SerializeField] private Text _gameTime;

        private HelpController _helpController;


        private readonly Queue<int> _questionIndexes = new Queue<int>();
        private readonly float _stepGrade = 0.1f;

        private int _currentStepProgress = 1;
        private string _commentText;

        private Coroutine _coroutine;
        private int _trueIndex = -1;
        private float _grade = 3.0f;
        private int _indexCurrentQuestion;
        private Image _prevImgSignal;

        private bool _isLoose;
        private int countTry = 5;

        private Character _player;
        private PlayFabAccountManager _accountManager;

        private int _repeatQuestionCnt = 0;

        #endregion


        #region Properties

        public bool IsLoadComplete { get; private set; }

        #endregion


        #region UnityMethods

        private void Awake()
        {
            var gameData = GameData.GetInstance();
            _player = gameData.Player;
            _accountManager = PlayFabAccountManager.Instance;
        }

        private void Start()
        {
            _helpController = GetComponent<HelpController>();

            if (!_player.IsTrain)
                SetCurrentStep();
            else LoadOneQuestion();
        }

        private void OnLoadNewQuestion()
        {
            if (_helpController)
                _helpController.Activator();
            _repeatQuestionCnt = 0;
        }

        private void PreviousCellMark()
        {
            var cnt = 0;
            if (_currentStepProgress > 0)
                foreach (var cell in _progressCells)
                {
                    cell.color = Color.yellow;
                    cnt++;
                    if (cnt > _currentStepProgress)
                        return;
                }
            else foreach (var cell in _progressCells)
                    cell.color = _defaultColor;
        }

        public void SetCurrentStep()
        {
            _currentStepProgress = PlayerPrefs.GetInt("CurrentStep");
            PreviousCellMark();
        }

        #endregion


        #region PublicMethods
        public void LoadOneQuestion()
        {
            if (!_player.IsTrain)
            {
                if (_isLoose)
                {
                    Loose();
                    GetComponent<ButtleBitController>().GameOver();
                    _isLoose = false;
                    return;
                }
            }

            if (!_player.IsTrain)
                if (_currentStepProgress >= _progressCells.Length)
                {
                    Win();
                    return;
                }
            _question.text = LoadingQuestion;

            if (_player.IsTrain)
                _rateStarsSelector.SetDefaultRateImage();
            StopAllCoroutines();

            var nextLevel = _currentStepProgress > 2 ? _currentStepProgress / 5 + 2 : 1;

            if (!_player.IsTrain)
            {
                StartCoroutine(PrevRaundPause(_currentStepProgress++));
            }
            else
            {
                StartCoroutine(GetQuestion());
            }
        }

        public void Win()
        {
            if (_currentStepProgress > 5)
            {
                if (!PlayerPrefs.HasKey("MarkReview"))
                    _reviewPanel.SetActive(true);
            }
            else if (_currentStepProgress <= 1)
            {
                GetComponent<SceneLoader>().LoadGameScene("Game");
                PlayerPrefs.SetInt("CurrentStep", 0);
                return;
            }

            _resultPanel.SetActive(true);
            _gameTime.text = _player.LastGameTime;
            _loose.SetActive(false);
            _victory.SetActive(true);
            _backAudio.Stop();

            if (PlayerPrefs.GetInt("Music").Equals(1))
                _winAudio.Play();

            if (!_player.IsTrain)
            {
                GetComponent<ButtleBitController>().GetWinAndGameOver();
                _accountManager.GetQuestionsCount();
                _accountManager.GetRightAnswersCount();
            }
            PlayerPrefs.SetInt("CurrentStep", 0);

        }

        //public void StartingTimer()
        //{
        //    if (_coroutine != null)
        //        StopCoroutine(_coroutine);
        //    _coroutine = StartCoroutine(TimerUpdate());
        //}

        public void SendAnswer(int index)
        {
            if (index == _trueIndex)
            {
                _answers[index].color = Color.green;
                //_ratePanel.SetActive(true);
                if (_coroutine != null)
                    StopCoroutine(_coroutine);

                if (!_player.IsTrain)
                    _accountManager.AddRightAnswersCount();

                _result.color = Color.green;
                _result.text = RightResult;
                _commentPanel.SetActive(true);

                if (!_player.IsTrain)
                    _player.RightError = false;
            }
            else if (_player.RightError)
            {
                _answers[index].color = Color.red;
                //_playerData.RightError = false;
            }
            else
                CommentLoose();

        }

        private void CommentLoose()
        {
            _result.color = Color.red;
            _result.text = ErrorResult;
            _commentPanel.SetActive(true);
            _isLoose = true;

            if (!_player.IsTrain)
                GetComponent<ButtleBitController>().GameOver();

        }

        private void Loose()
        {
            _accountManager.GetQuestionsCount();
            _accountManager.GetRightAnswersCount();
            _resultPanel.SetActive(true);
            _loose.SetActive(true);
            _backAudio.Stop();
            if (PlayerPrefs.GetInt("Music").Equals(1))
                _looseAudio.Play();
            PlayerPrefs.SetInt("CurrentStep", 0);
            _isLoose = false;
        }



        public void FiftyFifty()
        {
            var cnt = 0;
            var errCnt = 0;

            if (_answerButtons.Length > 0)
            {
                while (cnt < 2)
                {
                    var rnd = Random.Range(0, _answers.Length);
                    if (rnd != _trueIndex)
                    {
                        if (_answerButtons[rnd].isActiveAndEnabled)
                        {
                            _answerButtons[rnd].gameObject.SetActive(false);
                            cnt++;
                        }
                    }

                    errCnt++;
                    if (errCnt > 100)
                        return;
                }
            }
            cnt = 0;
            foreach (var btn in _answerButtons)
                if (btn.isActiveAndEnabled)
                    cnt++;

            if (cnt >= 3)
                if (_helpController.IsTwoErrorVarintsComplete)
                {
                    if (cnt >= 3)
                    {
                        _helpController.PurchaseR2();
                        _helpController.SetInteractibleR2Btn(true);
                    }
                    else if (cnt <= 2)
                    {
                        _helpController.PurchaseR2();
                        _helpController.SetNotInteracttibleAllHelpBtns();
                        _helpController.SetInteractibleR2Btn(false);
                    }
                }
                else
                {
                    _helpController.IsTwoErrorVarintsComplete = true;
                    _helpController.SetInteractibleR2Btn(true);
                }
        }

        public void DelOneErrorVariant()
        {
            var cnt = 0;

            var errCnt = 0;

            if (_answerButtons.Length > 0)
                while (cnt < 1)
                {
                    var rnd = Random.Range(0, _answers.Length);
                    if (rnd != _trueIndex)
                    {
                        if (_answerButtons[rnd].isActiveAndEnabled)
                        {
                            _answerButtons[rnd].gameObject.SetActive(false);
                            cnt++;
                        }
                    }
                    errCnt++;
                    if (errCnt > 100)
                        return;
                }

            cnt = 0;
            foreach (var btn in _answers)
            {
                if (btn.isActiveAndEnabled)
                    cnt++;
            }

            if (cnt <= 2)
                _helpController.SetInteractibleR2Btn(false);
        }


        public void SendGrade()
        {
            if (_progressPanel)
                _progressPanel.SetActive(true);
            StartCoroutine(SendUserGrade(URLSetGrade));
        }

        #endregion


        #region PrivateMethods

        private IEnumerator GetQuestion()
        {
            //_animatorReload.SetBool("IsLoading", true);
            _trueIndex = -1;
            var lvl = _currentStepProgress > 2 ? _currentStepProgress / 5 + 2 : 1;
            if (_player.IsTrain)
                lvl = Random.Range(0, 5);

            if (_repeatQuestionCnt > 10)
                lvl += 1;
            _repeatQuestionCnt++;

            WWWForm form = new WWWForm();
            form.AddField("level", lvl);

            using UnityWebRequest request = UnityWebRequest.Post(URLGetQuestion, form);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
                _status.text = "Отсутствует связ с сервером";

                countTry--;
                StartCoroutine(GetQuestion());

                if (countTry <= 0)
                    StopAllCoroutines();
                _status.text = "Отсутствует связ с сервером";

            }
            else
            {
                QuestionCreated(request);
                if (!_player.IsTrain)
                    _progressPanel.SetActive(false);
            }
        }

        private void QuestionCreated(UnityWebRequest request)
        {
            var allText = request.downloadHandler.text;
            var texts = allText.Split('☺');

            if (texts.Length < 1)
            {
                _question.text = texts[0];
                return;
            }

            _indexCurrentQuestion = int.Parse(texts[0]);

            if (!IsNewIndex(_indexCurrentQuestion))
            {
                StopAllCoroutines();
                StartCoroutine(GetQuestion());
                return;
            }
            else IsLoadComplete = true;

            _question.text = texts[1];

            List<string> textsList = new List<string>();
            foreach (var text in texts)
                textsList.Add(text);

            textsList.RemoveAt(0);
            textsList.RemoveAt(0);

            try
            {
                _grade = float.Parse(textsList[textsList.Count - 2]);
            }
            catch (System.Exception)
            {
                StopAllCoroutines();
                LoadOneQuestion();
            }

            _gradeText.text = ((int)_grade).ToString();


            _commentText = textsList[textsList.Count - 1];
            textsList.RemoveAt(textsList.Count - 1);
            textsList.RemoveAt(textsList.Count - 1);

            while (textsList.Count > 0)
            {
                int rnd = Random.Range(0, textsList.Count);
                _answers[textsList.Count - 1].text = textsList[rnd];
                textsList.RemoveAt(rnd);

                if (rnd == 0 && _trueIndex == -1)
                    _trueIndex = textsList.Count;
            }

            //_animatorReload.SetBool("IsLoading", false);

            SetDefaultColorAnswersText();

            //if (!_startBtn.gameObject.activeInHierarchy)
            //    _startBtn.gameObject.SetActive(true);

            _comment.text = _commentText;
            _commentPanel.SetActive(false);

            if (!_player.IsTrain)
                _accountManager.AddQuestionCount();

            OnLoadNewQuestion();

            StartCoroutine(EquipMinFontSizeAllAnswers());
        }


        private IEnumerator EquipMinFontSizeAllAnswers()
        {
            foreach (var t in _answers)
            {
                t.resizeTextMaxSize = 300;
            }
            yield return new WaitForFixedUpdate();

            var maxLength = 0;
            var longerstText = _answers[0];

            foreach (var t in _answers)
            {
                if (t.text.Length > maxLength)
                {
                    maxLength = t.text.Length;
                    longerstText = t;
                }
            }

            var fontSize = longerstText.cachedTextGenerator.fontSizeUsedForBestFit;


            foreach (var t in _answers)
            {
                t.resizeTextMaxSize = fontSize;
                //t.fontSize = fontSize;
            }
            _comment.resizeTextMaxSize = fontSize;

            yield return new WaitForFixedUpdate();
        }

        private bool IsNewIndex(int index)
        {
            bool isNewIndex = true;
            foreach (var ind in _questionIndexes)
                if (ind == index)
                    isNewIndex = false;
            if (isNewIndex)
                _questionIndexes.Enqueue(index);
            if (_questionIndexes.Count > 26)
                _questionIndexes.Dequeue();
            return isNewIndex;
        }

        private void SetDefaultColorAnswersText()
        {
            foreach (var answer in _answers)
                answer.color = Color.black;

            foreach (var answer in _answerButtons)
                if (!answer.isActiveAndEnabled)
                    answer.gameObject.SetActive(true);
            if (_player.IsTrain)
            {
                _ratePanel.SetActive(false);
                _rateStarsSelector.NextBtn.interactable = false;
                _rateStarsSelector.NextBtn.GetComponentInChildren<Text>().text = GradeInfo;
            }
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



        private void CreateGradeValue()
        {
            _grade = _grade > _rateStarsSelector.UserGrade ? _grade - _stepGrade : _grade + _stepGrade;
            _grade = _grade < 1 ? 1 : _grade;
        }

        private IEnumerator PrevRaundPause(int currentStep)
        {
            _progressPanel.SetActive(true);
            var clip = 3;
            if (_prevImgSignal != null)
                _prevImgSignal.color = _defaultColor;
            _prevImgSignal = _progressCells[currentStep].GetComponentsInChildren<Image>()[1];
            _prevImgSignal.color = _activeColor;

            _progressCells[currentStep].GetComponentInChildren<TMP_Text>().colorGradientPreset = _colorGradient;

            if (!_player.IsTrain)
                while (clip > 0)
                {
                    _progressCells[currentStep].color = Color.green;
                    yield return new WaitForSeconds(0.5f);
                    _progressCells[currentStep].color = Color.yellow;
                    yield return new WaitForSeconds(0.5f);
                    clip--;
                }
            StartCoroutine(GetQuestion());

            //StartingTimer();

            PlayerPrefs.SetInt("CurrentStep", currentStep);

        }


        private IEnumerator SendUserGrade(string url)
        {
            CreateGradeValue();
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

            yield return new WaitForSeconds(1);


            LoadOneQuestion();

        }

        #endregion
    }
}