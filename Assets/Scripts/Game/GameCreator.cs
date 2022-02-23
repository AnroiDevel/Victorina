using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Victorina
{
    public class GameCreator : MonoBehaviour
    {
        [SerializeField] private Button _play;
        [SerializeField] private Text _btnPlayText;
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private Text _priceInfo;
        [SerializeField] private Text _money;
        [SerializeField] private GameObject _moneyInfoPanel;

        private int _priceTicket = 5000;
        private ButtleType _typeGame;


        private void Start()
        {
            _moneyInfoPanel.SetActive(false);

            if (_playerData.IsPlayed)
                _btnPlayText.text = "����������";
            else if (_playerData.TicketsBit > 0)
                _btnPlayText.text = "����� �� ������";
            else
            {
                _money.text = _playerData.Bit.ToString();
                _priceInfo.text = $"���� ������: {_priceTicket}";
                _moneyInfoPanel.SetActive(true);
                _btnPlayText.text = "������ ����� � ������";
            }
        }

    }

    public enum ButtleType
    {
        Bit,
        Cubit,
    }

}