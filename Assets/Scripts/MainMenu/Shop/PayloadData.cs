using UnityEngine;


public class PayloadData
{
    #region Fields

    public JsonData JsonData;

    public string signature;
    public string json;

    #endregion

    public static PayloadData FromJson(string json)
    {
        var payload = JsonUtility.FromJson<PayloadData>(json);
        payload.JsonData = JsonUtility.FromJson<JsonData>(payload.json);
        return payload;
    }
}
