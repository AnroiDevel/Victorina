using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Victorina
{
    public class Player :MonoBehaviour
    {
        private string _namePlayer;
        private int _moneyPlayer;
        private string _mailPlayer;

        private void LoadPlayerData()
        {
            if (PlayerPrefs.HasKey("NamePlayer"))
            {
                _namePlayer = PlayerPrefs.GetString("NamePlayer");
                _moneyPlayer = PlayerPrefs.GetInt("MoneyPlayer");
                _mailPlayer = PlayerPrefs.GetString("EmailPlayer");
            }
        }

    }

}