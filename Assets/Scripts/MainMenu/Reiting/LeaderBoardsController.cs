using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class LeaderBoardsController : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;

        [SerializeField] private Text _monthReiting;
        [SerializeField] private Text _weekReiting;
        [SerializeField] private Text _pointsLable;
        [SerializeField] private Text _questionsCnt;
        [SerializeField] private Text _rightAnswersCnt;

        private void Start()
        {
            GetLeaderBoardMonthRank();
            GetLeaderBoardWeeklyRank();
            _pointsLable.text = _playerData.Bit.ToString();
            _questionsCnt.text = _playerData.AllQuestionsCount.ToString();
            _rightAnswersCnt.text = _playerData.RightAnswersCount.ToString();
        }

        private void OnEnable()
        {
            _pointsLable.text = _playerData.Bit.ToString();
            _questionsCnt.text = _playerData.AllQuestionsCount.ToString();
            _rightAnswersCnt.text = _playerData.RightAnswersCount.ToString();
        }

        private void GetLeaderBoardMonthRank()
        {
            var request = new GetLeaderboardAroundPlayerRequest()
            {
                StatisticName = "MonthRank",
                PlayFabId = _playerData.PlayFabId,
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
                PlayFabId = _playerData.PlayFabId,
            };
            PlayFabClientAPI.GetLeaderboardAroundPlayer(request, SetWeeklyRangValue, error => Debug.LogError(error));
        }


        private void SetWeeklyRangValue(GetLeaderboardAroundPlayerResult obj)
        {
            var rang = obj.Leaderboard[0].Position;
            _weekReiting.text = (++rang).ToString();
        }

        private void SetMonthRangValue(GetLeaderboardAroundPlayerResult obj)
        {
            var rang = obj.Leaderboard[0].Position;
            _monthReiting.text = (++rang).ToString();
        }


    }

}