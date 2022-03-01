using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;


namespace Victorina
{
    [CreateAssetMenu(fileName = "DataPlayer")]
    public class PlayerData : ScriptableObject
    {
        public string CreatedDateTimePlayfabProfile;
        public string CustomId;
        public string PlayFabId;
        public string TitlePlayerAccountId;
        public string ErrorInformation;
        public string Item;
        public string GuidID;
        public string Name;
        public string Email;
        public string Password;
        public int Bit = 0;

        public bool IsBonusReady;
        public DateTime RechargedBonusT;
        public int MaxBonusTimeSeconds;
        public int BonusRechargeSeconds;
        public Action<bool> BonusComplete;

        public bool IsPlayed;
        public int TicketsBit;
        public uint PriceBitTicket;

        public string[] ItemCatalog;
        private readonly Dictionary<string, CatalogItem> _catalog = new Dictionary<string, CatalogItem>();

        public string[] CurrencyVirtual;
        private readonly Dictionary<string, int> _virtualCurrency = new Dictionary<string, int>();

        private Timer _bonusTimer;

        public Action InitComplete;
        public Action<string> ConsumeComplete;
        public int GetBonusRechargeSeconds
        {
            get
            {
                PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), GetInventoryComplete, OnFailure);
                return BonusRechargeSeconds;
            }
            set => BonusRechargeSeconds = value;
        }

        public void Init()
        {
            IsPlayed = false;
            IsBonusReady = false;
            GetAccauntUserInfo();
            if (_bonusTimer == null)
                TimerInit();
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

        public void Reset()
        {
            GuidID = string.Empty;
            Name = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            Bit = 0;
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
            CustomId = info.AccountInfo.CustomIdInfo.CustomId;
            PlayFabId = info.AccountInfo.PlayFabId;
            TitlePlayerAccountId = info.AccountInfo.TitleInfo.TitlePlayerAccount.Id;
            Email = info.AccountInfo.PrivateInfo.Email;

            Debug.Log("Информация об аккаунте Playfab получена");
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

            }
            TicketsBit = ticketBit;

            int bitValue;
            var isGetBit = _virtualCurrency.TryGetValue("BT", out bitValue);
            if (isGetBit)
                Bit = bitValue;
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
            Bit += 100;
            IsBonusReady = false;
            PurchaseItemRequest request = new PurchaseItemRequest
            {
                CatalogVersion = "Bonuses",
                ItemId = "bonusBundels",
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
    }
}