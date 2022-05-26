using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;


namespace Victorina
{
    public class PlayFabAccountManager
    {
        private static PlayFabAccountManager _instance;

        private PlayFabLogin _playFabLogin;
        private GameData _gameData;
        private Character _player;

        public Action WeeklyRankReceived;
        public Action InventoryReceived;


        public PlayFabAccountManager()
        {
            _playFabLogin = new PlayFabLogin();
            _gameData = GameData.GetInstance();
            _player = _gameData.Player;
        }


        public static PlayFabAccountManager GetInstance()
        {
            if (_instance == null)
                _instance = new PlayFabAccountManager();
            return _instance;
        }


        public void GetLeaderBoardWeeklyRank(string playFabID)
        {
            var request = new GetLeaderboardAroundPlayerRequest()
            {
                StatisticName = "WeeklyRank",
                MaxResultsCount = 1,
                PlayFabId = playFabID,
            };
            PlayFabClientAPI.GetLeaderboardAroundPlayer(request,
                result =>
                {
                    _player.WeeklyRank = result.Leaderboard[0].Position + 1;
                    WeeklyRankReceived?.Invoke();
                },
                error => Debug.LogError(error));
        }


        public void GetPlayerInventory()
        {
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
                result =>
                {
                    foreach (var pair in result.VirtualCurrency)
                    {
                        switch (pair.Key)
                        {
                            case "BT":
                                _player.Money = pair.Value;
                                break;
                            case "TI":
                                _player.Tickets = pair.Value;
                                break;
                            case "BS":
                                _player.Bonus = pair.Value;
                                break;
                            case "R2":
                                _player.HelpicR2 = pair.Value;
                                break;
                            case "RE":
                                _player.HelpicRE = pair.Value;
                                break;
                        }
                    }
                    foreach (var pair in result.Inventory)
                    {
                        switch (pair.DisplayName)
                        {
                            case "BitTicket":
                                _player.BitTicket++;
                                break;
                            case "PlayToken":
                                _player.IsPlay = true;
                                break;
                            case "Vip Day":
                                _player.IsVip = true;
                                break;
                            case "Not reclama":
                                _player.IsNotReclama = true;
                                break;

                        }
                    }
                    InventoryReceived?.Invoke();
                }
                ,
                error => Debug.LogError(error.GenerateErrorReport()));
        }


        public void GetTimeRechargeBonus()
        {
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(),
                result =>
                {
                    VirtualCurrencyRechargeTime bonusTime;
                    result.VirtualCurrencyRechargeTimes.TryGetValue("BS", out bonusTime);
                    _player.BonusSecondsTime = bonusTime.SecondsToRecharge;
                },
                error => Debug.LogError(error.GenerateErrorReport()));
        }


        public void GetQuestionsCount()
        {
            var request = new GetLeaderboardAroundPlayerRequest()
            {
                StatisticName = "QuestionCount",
                MaxResultsCount = 1,
                PlayFabId = _player.PlayFabId,
            };
            PlayFabClientAPI.GetLeaderboardAroundPlayer(request, result =>
            {
                _player.AllQuestionsCount = result.Leaderboard[0].StatValue;

                if (PlayerPrefs.HasKey("AllGameTime"))
                {
                    var prevTime = PlayerPrefs.GetInt("AllGameTime");

                    var average = _player.AllQuestionsCount != 0 ? prevTime / _player.AllQuestionsCount : prevTime;
                    DateTime averageTime = DateTime.MinValue.AddSeconds(average);
                    _player.AverageTimeAnswers = averageTime.ToLongTimeString();


                    int allTime = _player.LastGameTimeSec + prevTime;
                    _player.AllGameTime = DateTime.MinValue.AddSeconds(allTime).ToLongTimeString();
                    _player.LastGameTimeSec = 0;
                    PlayerPrefs.SetInt("AllGameTime", allTime);
                }
                else
                {
                    _player.AllGameTime = _player.LastGameTime;
                    DateTime averageTime = DateTime.MinValue;
                    _player.AverageTimeAnswers = averageTime.ToLongTimeString();
                    PlayerPrefs.SetInt("AllGameTime", 0);
                }
            }, error => Debug.LogError(error));
        }


        public void GetRightAnswersCount()
        {
            var request = new GetLeaderboardAroundPlayerRequest()
            {
                StatisticName = "RightAnswersCount",
                MaxResultsCount = 1,
                PlayFabId = _player.PlayFabId,
            };
            PlayFabClientAPI.GetLeaderboardAroundPlayer(request, result => _player.RightAnswersCount = result.Leaderboard[0].StatValue, error => Debug.LogError(error));
        }


        public void GetCatalogItem(string catalogVersion)
        {
            PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest { CatalogVersion = catalogVersion },
                result =>
                {
                    foreach (var item in result.Catalog)
                    {
                        switch (item.DisplayName)
                        {
                            case "BitTicket":
                                _player.PriceBitTicket = (int)item.VirtualCurrencyPrices["BT"];
                                break;
                        }
                    }
                }, error => Debug.LogError(error));
        }


        public void SetDisplayName(string name)
        {
            PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = name
            }, result => _player.Name = name, error => Debug.LogError(error.GenerateErrorReport()));
        }


        public void GetPrize(int level)
        {
            if (level == 0) return;

            PurchaseItemRequest request = new PurchaseItemRequest
            {
                CatalogVersion = "Prizes",
                ItemId = "Prize-" + level,
                VirtualCurrency = "BT",
                Price = 0,
            };
            PlayFabClientAPI.PurchaseItem(request, result =>
            {
                PlayerPrefs.SetInt("CurrentStep", 0);
            }, error => Debug.Log(error));
        }


    }
}