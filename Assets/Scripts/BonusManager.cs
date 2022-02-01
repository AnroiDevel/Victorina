using System;
using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class BonusManager : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private Text _timeToNext;

        private Button _bonusButton;

        private bool _isBonusComplete;
        private DateTime _startTime;

        private void Start()
        {
            _bonusButton = GetComponent<Button>();
            _startTime = DateTime.Now;
            _timeToNext.text = _playerData.RechargedBonusTime;
        }

        private void FixedUpdate()
        {
            if (!_isBonusComplete)
            {
                var tempTime = _playerData.RechargedBonusT - (DateTime.Now - _startTime);
                _timeToNext.text = tempTime.ToLongTimeString();
                if (tempTime.Minute < 1 && tempTime.Second < 1)
                {
                    _isBonusComplete = true;
                    _bonusButton.interactable = true;
                }
            }
        }

        public void StartTimer()
        {
            _startTime = DateTime.Now;
            _isBonusComplete = false;
            _bonusButton.interactable = false;
            BonusMoney(100);
        }

        private void BonusMoney(int val)
        {
            _playerData.AddMoney(val);
        }
    }

}