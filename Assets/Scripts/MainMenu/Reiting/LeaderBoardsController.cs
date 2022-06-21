using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class LeaderBoardsController : MonoBehaviour
    {
        #region Fields

        [SerializeField] private PlayerData _playerData;
        [SerializeField] private Text _monthReiting;
        [SerializeField] private Text _weekReiting;
        [SerializeField] private Text _pointsLable;
        [SerializeField] private Text _questionsCnt;
        [SerializeField] private Text _rightAnswersCnt;
        [SerializeField] private Text _averageTimeAnswers;
        [SerializeField] private Text _allTimeGame;

        private Character _player;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            var gameData = GameData.GetInstance();
            _player = gameData.Player;
        }

        private void Start()
        {
            GetLeaderBoardMonthRank();
            GetLeaderBoardWeeklyRank();
            _pointsLable.text = _player.Money.ToString();
            _questionsCnt.text = _player.AllQuestionsCount.ToString();
            _rightAnswersCnt.text = _player.RightAnswersCount.ToString();
            _averageTimeAnswers.text = _player.AverageTimeAnswers;
            _allTimeGame.text = _player.AllGameTime;
        }

        #endregion


        #region Methods

        private void GetLeaderBoardMonthRank()
        {
            var request = new GetLeaderboardAroundPlayerRequest()
            {
                StatisticName = "MonthRank",
                PlayFabId = _player.PlayFabId,
                MaxResultsCount = 1,
            };
            PlayFabClientAPI.GetLeaderboardAroundPlayer(request, SetMonthRangValue, error => Debug.LogError(error));
        }

        private void GetLeaderBoardWeeklyRank()
        {
            var request = new GetLeaderboardAroundPlayerRequest()
            {
                StatisticName = "WeeklyRank",
                MaxResultsCount = 1,
                PlayFabId = _player.PlayFabId,
            };
            PlayFabClientAPI.GetLeaderboardAroundPlayer(request, SetWeeklyRangValue, error => Debug.LogError(error));
        }


        private void SetWeeklyRangValue(GetLeaderboardAroundPlayerResult obj)
        {
            var rang = obj.Leaderboard[0].Position;
            _weekReiting.text = (++rang).ToString();
            _player.WeeklyRank = rang;
        }

        private void SetMonthRangValue(GetLeaderboardAroundPlayerResult obj)
        {
            var rang = obj.Leaderboard[0].Position;
            _monthReiting.text = (++rang).ToString();
            _player.MonthRank = rang;
        }

        #endregion    
    }
}