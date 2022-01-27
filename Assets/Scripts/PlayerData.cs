using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Victorina
{
    [CreateAssetMenu(fileName = "DataPlayer")]
    public class PlayerData : ScriptableObject
    {
        public string Created;
        public string CustomId;
        public string PlayFabId;
        public string TitlePlayerAccountId;

        public string Item;

        public string GuidID;
        public string Name;
        public string Email;
        public string Password;

        public string ErrorInformation;

        private readonly Dictionary<string, CatalogItem> _catalog = new Dictionary<string, CatalogItem>();
        private readonly Dictionary<string, int> _virtualCurrency = new Dictionary<string, int>();

        public int Gold;

        public void Init()
        {
            //if (PlayerPrefs.HasKey("authorization-guid"))
            //{ 
            //    GuidID = PlayerPrefs.GetString("authorization-guid");
            //    Name = PlayerPrefs.GetString("Name");
            //    Email = PlayerPrefs.GetString("Email");
            //    Password = PlayerPrefs.GetString("Password");
            //    Money = PlayerPrefs.GetInt("Money");
            //}
            
            GetAccauntUserInfo();
        }

        public void Reset()
        {
            GuidID = string.Empty;
            Name = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            Gold = 0;
        }

        private void GetAccauntUserInfo()
        {
            PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), OnCompletePlayFabAccountInfo, OnFailure);
        }


        private void OnCompletePlayFabAccountInfo(GetAccountInfoResult info)
        {
            Created = info.AccountInfo.Created.ToString();
            CustomId = info.AccountInfo.CustomIdInfo.CustomId;
            PlayFabId = info.AccountInfo.PlayFabId;
            TitlePlayerAccountId = info.AccountInfo.TitleInfo.TitlePlayerAccount.Id;
            Email = info.AccountInfo.PrivateInfo.Email;

            PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest(), OnGetCatalogSuccess, OnFailure);

        }

        private void GetInventoryComplete(GetUserInventoryResult result)
        {
            _virtualCurrency.Clear();
            foreach (var pair in result.VirtualCurrency)
            {
                _virtualCurrency.Add(pair.Key, pair.Value);
            }
            int goldValue;
            var isGetGold = _virtualCurrency.TryGetValue("GD", out goldValue);
            if (isGetGold)
                Gold = goldValue;
        }

        private void OnGetCatalogSuccess(GetCatalogItemsResult result)
        {
            //HandleCatalog(result.Catalog);
            Debug.Log($"Catalog was loaded successfully!");


            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), GetInventoryComplete, OnFailure);
        }

        private void HandleCatalog(List<CatalogItem> catalog)
        {
            foreach (var item in catalog)
            {
                _catalog.Add(item.ItemId, item);
                Debug.Log($"Catalog item {item.ItemId} was added successfully!");
            }
        }
        private void OnFailure(PlayFabError obj)
        {
            ErrorInformation = obj.GenerateErrorReport();
        }



    }

}