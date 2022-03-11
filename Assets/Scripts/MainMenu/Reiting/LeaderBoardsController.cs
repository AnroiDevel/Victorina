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
        [SerializeField] private Text _monthReiting;
        [SerializeField] private Text _weekReiting;

        private void Start()
        {
            GetLeaderBoardMonthRank();
            GetLeaderBoardWeeklyRank();
        }

        private void GetLeaderBoardMonthRank()
        {
            var request = new GetLeaderboardAroundPlayerRequest()
            {
                StatisticName = "MonthRank",
                MaxResultsCount = 1,
            };
            PlayFabClientAPI.GetLeaderboardAroundPlayer(request, SetMonthRangValue, error=>Debug.LogError(error));
        }

        private void GetLeaderBoardWeeklyRank()
        {
            var request = new GetLeaderboardAroundPlayerRequest()
            {
                StatisticName = "WeeklyRank",
                MaxResultsCount = 1,
            };
            PlayFabClientAPI.GetLeaderboardAroundPlayer(request, SetWeeklyRangValue, error=>Debug.LogError(error));
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