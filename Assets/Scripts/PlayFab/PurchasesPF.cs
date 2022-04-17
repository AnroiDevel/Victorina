using PlayFab;
using UnityEngine;
using PlayFab.ClientModels;


namespace Victorina
{
    public class PurchasesPF
    {
        public void PurchaseItem(string catalogVersion, string itemId, string virtualCurrency, int price)
        {
            PurchaseItemRequest request = new PurchaseItemRequest
            {
                CatalogVersion = catalogVersion,
                ItemId = itemId,
                VirtualCurrency = virtualCurrency,
                Price = price,
            };

            PlayFabClientAPI.PurchaseItem(request, result => Debug.Log(result), failture => Debug.Log(failture));
        }
    }

}