using UnityEngine;


namespace Victorina
{
    [CreateAssetMenu(fileName = "DataPlayer")]
    public class PlayerData : ScriptableObject
    {
        public string GuidID;
        public string Name;
        public string Email;
        public string Password;


        public int Money;

        public void Init()
        {
            if (PlayerPrefs.HasKey("authorization-guid"))
            { 
                GuidID = PlayerPrefs.GetString("authorization-guid");
                Name = PlayerPrefs.GetString("Name");
                Email = PlayerPrefs.GetString("Email");
                Password = PlayerPrefs.GetString("Password");
                Money = PlayerPrefs.GetInt("Money");
            }
        }

        public void Reset()
        {
            GuidID = string.Empty;
            Name = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            Money = 0;
        }
    }

}