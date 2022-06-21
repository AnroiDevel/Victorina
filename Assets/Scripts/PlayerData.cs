using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Victorina
{
    [CreateAssetMenu(fileName = "DataPlayer")]
    public class PlayerData : ScriptableObject
    {
        #region Fields

        public static PlayerData Instance { get; private set; }

        [SerializeField] internal int R2;
        [SerializeField] internal int RE;
        [SerializeField] internal int WeeklyRank;
        [SerializeField] internal int MonthRank;
        public bool IsNewPlayer;
        public bool IsNewVersionApp;

        public string CustomId;
        public string PlayFabIdCurrentPlayer;
        public string TitlePlayerAccountId;
        public string CreatedDateTimePlayfabProfile;

        public string ErrorInformation;

        public Sprite Avatar;
        public string PathFileAvatar;
        public float ScaleImageAvatarCoef = 1.0f;

        public string Item;
        public string GuidID;
        public string Name;
        public string Email;
        public string Password;
        public int Bit = 0;

        [Header("Бонус")]
        public bool IsBonusReady;
        public DateTime RechargedBonusT;
        public int MaxBonusTimeSeconds;
        public int BonusRechargeSeconds;
        public Action<bool> BonusComplete;

        public bool IsTrain;
        public bool IsPlayed;
        public bool IsVip;
        public int TicketsBit;
        public uint PriceBitTicket;

        public string[] ItemCatalog;

        public string[] CurrencyVirtual;

        public Action InitComplete;
        public Action<string> ConsumeComplete;
        public Action BitInfoUpdate;
        public Action TicketInfoUpdate;
        public Action ReloadAvatar;
        public Action<int> GetNumberBonus;

        public string LastGameTime;
        public string AllGameTime;
        public string AverageTimeAnswers;

        public int AllQuestionsCount;
        public int RightAnswersCount;
        public bool RightError;

        public Action NewPlayerComplete;
        public Action LoginComplete;
        [SerializeField] internal bool NotReclama;

        private int _lastGameTimeSec;

        #endregion


        #region Methods

        public void Login()
        {

            //PlayFabSettings.staticSettings.TitleId = "D2AD8";

            ////GuidID = PlayerPrefs.GetString("Id");

            ////PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
            ////{
            ////    CustomId = GuidID,
            ////    CreateAccount = false,
            ////}, success =>
            ////{
            ////    Init();
            ////    LoginComplete?.Invoke();

            ////}, OnFailure);

            //PlayFabClientAPI.LoginWithAndroidDeviceID(new LoginWithAndroidDeviceIDRequest()
            //{
            //    CreateAccount = true,
            //    AndroidDeviceId = SystemInfo.deviceUniqueIdentifier
            //}, result =>
            //{
            //    Debug.Log("Logged in");
            //    Init();
            //    LoginComplete?.Invoke();
            //    // Refresh available items
            //}, error => Debug.LogError(error.GenerateErrorReport()));

        }

        public void SubmitScore(int playerScore)
        {
            PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate> {
            new StatisticUpdate {
                StatisticName = "MonthRank",
                Value = playerScore
            },
            new StatisticUpdate {
                StatisticName = "WeeklyRank",
                Value = playerScore
            },
        }
            }, result => OnStatisticsUpdated(result), FailureCallback);
        }

        public void AddQuestionCount()
        {
            PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate> {
            new StatisticUpdate {
                StatisticName = "QuestionCount",
                Value = 1,
            }
        }
            }, result => Debug.Log("Количество вопросов обновлено"), FailureCallback);
        }

        public void AddRightAnswersCount()
        {
            PlayFabClientAPI.UpdatePlayerStatistics(new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate> {
            new StatisticUpdate {
                StatisticName = "RightAnswersCount",
                Value = 1,
            }
        }
            }, result => Debug.Log("Количество правильных ответов обновлено"), FailureCallback);
        }

        public void Reset()
        {
            IsNewPlayer = true;
            IsNewVersionApp = true;

            CustomId = string.Empty;
            PlayFabIdCurrentPlayer = string.Empty;
            TitlePlayerAccountId = string.Empty;
            CreatedDateTimePlayfabProfile = string.Empty;

            GuidID = string.Empty;
            Name = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            Bit = 0;
        }

        internal void AddMoney(int val)
        {
            AddUserVirtualCurrencyRequest request = new AddUserVirtualCurrencyRequest
            {
                Amount = val,
                VirtualCurrency = "BT"
            };

            //PlayFabClientAPI.AddUserVirtualCurrency(request, complete => Debug.Log(complete), error => Debug.Log(error));

        }

        private void OnStatisticsUpdated(UpdatePlayerStatisticsResult updateResult)
        {
            Debug.Log("Successfully submitted high score");
        }

        private void FailureCallback(PlayFabError error)
        {
            Debug.LogWarning("Something went wrong with your API call. Here's some debug information:");
            Debug.LogError(error.GenerateErrorReport());
        }


        private void OnFailure(PlayFabError obj)
        {
            ErrorInformation = obj.GenerateErrorReport();
            Debug.Log(ErrorInformation);
        }

        public void ConsumeItem(string nameItem)
        {
            var iii = string.Empty;
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result =>
            {
                if (result.Inventory.Count <= 0) return;

                var iii = result.Inventory[0].ItemInstanceId;
                foreach (var item in result.Inventory)
                    if (item.DisplayName == nameItem)
                    {
                        iii = item.ItemInstanceId;
                        if (nameItem == "PlayToken")
                        {
                            var createDate = item.PurchaseDate;
                            var a = item.PurchaseDate.Value;
                            var nowDate = DateTime.UtcNow;
                            int sec = (int)(nowDate - createDate).Value.TotalSeconds;

                            _lastGameTimeSec = sec;
                        }
                    }
                if (iii != string.Empty)
                    PlayFabClientAPI.ConsumeItem(new ConsumeItemRequest
                    {
                        ConsumeCount = 1,
                        ItemInstanceId = iii,


                    }, result =>
                    {
                        ConsumeComplete?.Invoke(nameItem);
                    }, OnFailure);

            }, OnFailure);

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

        public void SetPlayMode(bool isMode)
        {
            IsTrain = isMode;
        }

        public void GetQuestionsCount()
        {
            var request = new GetLeaderboardAroundPlayerRequest()
            {
                StatisticName = "QuestionCount",
                MaxResultsCount = 1,
                PlayFabId = PlayFabIdCurrentPlayer,
            };
            PlayFabClientAPI.GetLeaderboardAroundPlayer(request, result =>
            {
                AllQuestionsCount = result.Leaderboard[0].StatValue;

                if (PlayerPrefs.HasKey("AllGameTime"))
                {
                    var prevTime = PlayerPrefs.GetInt("AllGameTime");

                    var average = AllQuestionsCount != 0 ? prevTime / AllQuestionsCount : prevTime;
                    DateTime averageTime = DateTime.MinValue.AddSeconds(average);
                    AverageTimeAnswers = averageTime.ToLongTimeString();


                    int allTime = _lastGameTimeSec + prevTime;
                    AllGameTime = DateTime.MinValue.AddSeconds(allTime).ToLongTimeString();
                    _lastGameTimeSec = 0;
                    PlayerPrefs.SetInt("AllGameTime", allTime);
                }
                else
                {
                    AllGameTime = LastGameTime;
                    DateTime averageTime = DateTime.MinValue;
                    AverageTimeAnswers = averageTime.ToLongTimeString();
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
                PlayFabId = PlayFabIdCurrentPlayer,
            };
            PlayFabClientAPI.GetLeaderboardAroundPlayer(request, result => RightAnswersCount = result.Leaderboard[0].StatValue, error => Debug.LogError(error));
        }

        #endregion    
    }
}