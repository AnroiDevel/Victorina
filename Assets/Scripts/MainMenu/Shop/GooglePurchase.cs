using UnityEngine;


public class GooglePurchase
{
    #region Fields

    public PayloadData PayloadData;

    public string Store;
    public string TransactionID;
    public string Payload;

    #endregion

    public static GooglePurchase FromJson(string json)
    {
        var purchase = JsonUtility.FromJson<GooglePurchase>(json);
        purchase.PayloadData = PayloadData.FromJson(purchase.Payload);
        return purchase;
    }
}