using System;
using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class BonusManager : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private Text _timeToNext;
        [SerializeField] private Text _bit;

        private Button _bonusButton;

        private bool _isBonusComplete;
        private DateTime _startTime;

        private void Awake()
        {
            _bonusButton = GetComponent<Button>();
            _isBonusComplete = _playerData.IsBonusReady;
        }

        private void Start()
        {
            if (_isBonusComplete)
                OnBonusComplete(true);
        }

        private void OnBonusComplete(bool val = false)
        {
            _bonusButton.interactable = val;
            _timeToNext.text = "Готово";
        }

        private void FixedUpdate()
        {
            if (_playerData.IsBonusReady)
                OnBonusComplete(true);
            else 
                _timeToNext.text = _playerData.RechargedBonusT.ToLongTimeString();
        }

        public void GetBonus()
        {
            _playerData.GetBonus();
            _isBonusComplete = false;
            _bonusButton.interactable = false;
            var tempBit = _playerData.Bit;
            _bit.text = tempBit.ToString();
        }
    }

}