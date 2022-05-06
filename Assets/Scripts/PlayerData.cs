using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using UnityEngine;


namespace Victorina
{
    [CreateAssetMenu(fileName = "DataPlayer")]
    public class PlayerData : ScriptableObject
    {
        [SerializeField] internal int R2;
        [SerializeField] internal int RE;
        [SerializeField] internal int WeeklyRank;
        [SerializeField] internal int MonthRank;
        //private string UrlAvatar = "AvatarUrl";
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
        private readonly Dictionary<string, CatalogItem> _catalog = new Dictionary<string, CatalogItem>();

        public string[] CurrencyVirtual;
        private readonly Dictionary<string, int> _virtualCurrency = new Dictionary<string, int>();

        private Timer _bonusTimer;

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

        public void CreateNewPlayer()
        {
            PlayFabSettings.staticSettings.TitleId = "D2AD8";
            var id = Guid.NewGuid().ToString();
            PlayerPrefs.SetString("Id", id);

            PlayFabClientAPI.LoginWithCustomID(
                new LoginWithCustomIDRequest()
                {
                    CustomId = id,
                    CreateAccount = true,
                },
                sucsess =>
                {
                    SetDisplayName(Name);
                    GuidID = id;
                },
                error => Debug.Log("sss"));
        }

        public void Login()
        {

            PlayFabSettings.staticSettings.TitleId = "D2AD8";

            GuidID = PlayerPrefs.GetString("Id");

            PlayFabClientAPI.LoginWithCustomID(new LoginWithCustomIDRequest()
            {
                CustomId = GuidID,
                CreateAccount = false,
            }, success =>
            {
                Init();
                LoginComplete?.Invoke();

            }, OnFailure);

        }

        public void Init()
        {
            IsPlayed = false;
            IsVip = false;
            IsBonusReady = false;
            NotReclama = false;
            CustomId = string.Empty;

            if (PlayerPrefs.HasKey("Id"))
            {
                GuidID = PlayerPrefs.GetString("Id");
                IsNewPlayer = false;
            }
            else
                IsNewPlayer = true;

            GetAccauntUserInfo();
            if (_bonusTimer == null)
                TimerInit();

        }

        //public void SetAvatar() 
        //{
        //    LoadAvatarAsync();
        //}

        //private async void LoadAvatarAsync()
        //{
        //    await SetAvatarAsync();
        //}

        //private async Task SetAvatarAsync()
        //{
        //    if (PlayerPrefs.HasKey(UrlAvatar))
        //    {
        //        PathFileAvatar = PlayerPrefs.GetString(UrlAvatar);
        //        if (PathFileAvatar == string.Empty)
        //            PathFileAvatar = Application.dataPath + "/logo.png";
        //    }
        //    else
        //    {
        //        Sprite spr = Resources.Load("photo", typeof(Sprite)) as Sprite;
        //        Avatar = spr;
        //    }

        //    if (File.Exists(PathFileAvatar))
        //    {
        //        var www = new WWW("file://" + PathFileAvatar);
        //        Texture2D texture2D = www.texture;
        //        Sprite sprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
        //        Avatar = sprite;

        //        if (texture2D != null)
        //            ScaleImageAvatarCoef = texture2D.width > texture2D.height ? (float)texture2D.width / texture2D.height : texture2D.height / (float)texture2D.width;
        //    }

        //    await Task.Run(() => Debug.Log("kk"
        //        +6324));

        //}




        public int GetBonusRechargeSeconds
        {
            get
            {
                PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), GetInventoryComplete, OnFailure);
                return BonusRechargeSeconds;
            }
            set => BonusRechargeSeconds = value;
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
            //PlayerPrefs.DeleteAll();
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

        private void TimerInit()
        {
            _bonusTimer = new Timer(1000);
            _bonusTimer.Elapsed += TimeUpdate;
        }

        private void TimeUpdate(object sender, ElapsedEventArgs e)
        {
            BonusRechargeSeconds = BonusRechargeSeconds <= 0 ? 0 : BonusRechargeSeconds - 1;
            if (BonusRechargeSeconds > 0)
            {
                RechargedBonusT = DateTime.MinValue;
                RechargedBonusT += DateTime.Now.AddSeconds(BonusRechargeSeconds) - DateTime.Now;
            }
            else
            {
                IsBonusReady = true;
                _bonusTimer?.Stop();
            }
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


        private void GetAccauntUserInfo()
        {
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnCompletePlayFabAccountInfo, OnFailure);
            PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest { CatalogVersion = "Tickets" }, OnGetCatalogSuccess, OnFailure);
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), GetInventoryComplete, OnFailure);
        }


        private void OnCompletePlayFabAccountInfo(GetAccountInfoResult info)
        {
            CreatedDateTimePlayfabProfile = info.AccountInfo.Created.ToString();
            PlayFabIdCurrentPlayer = info.AccountInfo.PlayFabId;
            TitlePlayerAccountId = info.AccountInfo.TitleInfo.TitlePlayerAccount.Id;
            Email = info.AccountInfo.PrivateInfo.Email;
            Name = info.AccountInfo.TitleInfo.DisplayName;


            GetLeaderBoardWeeklyRank();
            GetLeaderBoardMonthRank();
            GetQuestionsCount();
            GetRightAnswersCount();

            Debug.Log("Информация об аккаунте Playfab получена");
        }

        public void SetDisplayName(string name)
        {
            Name = name;

            PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = name
            }, AddedNameComplete, error => Debug.LogError(error.GenerateErrorReport()));
        }

        private void AddedNameComplete(UpdateUserTitleDisplayNameResult obj)
        {
            IsNewPlayer = false;
            NewPlayerComplete?.Invoke();
            Init();
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

        private void GetInventoryComplete(GetUserInventoryResult result)
        {
            _virtualCurrency.Clear();
            CurrencyVirtual = new string[result.VirtualCurrency.Count];
            var cnt = 0;
            foreach (var pair in result.VirtualCurrency)
            {
                CurrencyVirtual[cnt++] = pair.Key + " = " + pair.Value;
                _virtualCurrency.Add(pair.Key, pair.Value);
                if (pair.Key == "R2")
                    R2 = pair.Value;
                if (pair.Key == "RE")
                    RE = pair.Value;
            }

            var ticketBit = 0;
            foreach (var item in result.Inventory)
            {
                if (item.DisplayName == "BitTicket")
                {
                    ticketBit++;
                }
                else if (item.DisplayName == "PlayToken")
                    IsPlayed = true;
                else if (item.DisplayName == "Vip Day")
                    IsVip = true;
                else if (item.DisplayName == "Not reclama")
                    NotReclama = true;

            }
            TicketsBit = ticketBit;

            int bitValue;
            var isGetBit = _virtualCurrency.TryGetValue("BT", out bitValue);
            if (isGetBit)
            {
                Bit = bitValue;
                BitInfoUpdate?.Invoke();
            }

            int ticketValue;
            var isGetTicket = _virtualCurrency.TryGetValue("TI", out ticketValue);
            if (isGetTicket)
            {
                TicketsBit = ticketValue;
                TicketInfoUpdate?.Invoke();
            }

            int bonus;
            var isGetBonus = _virtualCurrency.TryGetValue("BS", out bonus);
            if (isGetBonus)
            {
                VirtualCurrencyRechargeTime BSRechargedTimes;
                var isT = result.VirtualCurrencyRechargeTimes.TryGetValue("BS", out BSRechargedTimes);
                if (isT)
                {
                    BonusRechargeSeconds = BSRechargedTimes.SecondsToRecharge;
                }
                if (bonus > 0)
                {
                    MaxBonusTimeSeconds = BSRechargedTimes.SecondsToRecharge;
                    IsBonusReady = true;
                    _bonusTimer.Stop();
                }
                else
                {
                    IsBonusReady = false;
                    _bonusTimer.Start();
                }
            }

            InitComplete?.Invoke();

        }

        private void OnGetCatalogSuccess(GetCatalogItemsResult result)
        {
            HandleCatalog(result.Catalog);
            Debug.Log($"Каталог успешно загружен");

        }

        private void HandleCatalog(List<CatalogItem> catalog)
        {

            ItemCatalog = new string[catalog.Count];
            _catalog.Clear();

            var cnt = 0;
            foreach (var item in catalog)
            {
                ItemCatalog.SetValue(item.ItemId, cnt++);
                _catalog.Add(item.ItemId, item);
                if (item.DisplayName != null)
                    if (item.DisplayName.Equals("BitTicket"))
                        PriceBitTicket = item.VirtualCurrencyPrices["BT"];
            }
        }
        private void OnFailure(PlayFabError obj)
        {
            ErrorInformation = obj.GenerateErrorReport();
            Debug.Log(ErrorInformation);
        }

        public void GetIsCompletetedBonus()
        {
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), IsBonusComplete, Debug.Log);
        }

        private void IsBonusComplete(GetUserInventoryResult result)
        {
            int bonusValue;
            if (result.VirtualCurrency.TryGetValue("BS", out bonusValue))
            {
                IsBonusReady = bonusValue > 0 ? true : false;
            }
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), GetInventoryComplete, OnFailure);
        }



        public void GetBonus()
        {
            // Bit += 100;
            IsBonusReady = false;

            var rnd = UnityEngine.Random.Range(1, 5);
            var itId = "Bonus" + rnd;

            GetNumberBonus?.Invoke(rnd);

            PurchaseItemRequest request = new PurchaseItemRequest
            {
                CatalogVersion = "Bonuses",
                ItemId = itId,
                VirtualCurrency = "BS",
                Price = 1,

            };
            PlayFabClientAPI.PurchaseItem(request, result => OnBonusComplete(), error => Debug.Log(error));
            RechargedBonusT = DateTime.MinValue.AddSeconds(MaxBonusTimeSeconds);
        }




        private void OnBonusComplete()
        {
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), GetInventoryComplete, OnFailure);
        }


        public void GetUserToBonusRechargedTime() =>
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), OnRechargedTime, OnFailure);

        private void OnRechargedTime(GetUserInventoryResult result)
        {
            VirtualCurrencyRechargeTime BSRechargedTimes;
            var isT = result.VirtualCurrencyRechargeTimes.TryGetValue("BS", out BSRechargedTimes);
            if (isT)
            {
                var tempTime = BSRechargedTimes.SecondsToRecharge;
                RechargedBonusT += DateTime.Now.AddSeconds(tempTime) - DateTime.Now;
            }
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
                        ItemInstanceId = iii

                    }, result =>
                    {
                        Init();
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
                Init();
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


        private void GetLeaderBoardMonthRank()
        {
            var request = new GetLeaderboardAroundPlayerRequest()
            {
                StatisticName = "MonthRank",
                PlayFabId = PlayFabIdCurrentPlayer,
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
                PlayFabId = PlayFabIdCurrentPlayer,
            };
            PlayFabClientAPI.GetLeaderboardAroundPlayer(request, SetWeeklyRangValue, error => Debug.LogError(error));
        }


        private void SetWeeklyRangValue(GetLeaderboardAroundPlayerResult obj)
        {
            var rang = obj.Leaderboard[0].Position;
            WeeklyRank = rang + 1;
        }

        private void SetMonthRangValue(GetLeaderboardAroundPlayerResult obj)
        {
            var rang = obj.Leaderboard[0].Position;
            MonthRank = rang + 1;
        }


    }
}