using System;
using System.Collections;
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
            _playerData.BonusComplete += OnBonusComplete;
            _isBonusComplete = _playerData.IsBonusReady;
        }

        private void OnEnable()
        {
            StartCoroutine(BonusTimerUpdater(_playerData.BonusRechargeSeconds));
        }

        private void OnDisable()
        {
            StopCoroutine(BonusTimerUpdater(_playerData.BonusRechargeSeconds));
        }

        private void OnBonusComplete(bool val)
        {
            _bonusButton.interactable = val;
            _timeToNext.text = "Готово";
        }


        private IEnumerator BonusTimerUpdater(int seconds)
        {
            if (!_playerData.IsBonusReady)
                while (seconds-- > 0)
                {
                    var tempTime = DateTime.MinValue.AddSeconds(seconds);
                    _timeToNext.text = tempTime.ToLongTimeString();
                    yield return new WaitForSeconds(1);
                }
            OnBonusComplete(true);
        }


        public void GetBonus()
        {          
            _playerData.GetBonus();

            _bonusButton.interactable = false;
            var tempBit = _playerData.Bit;
            _bit.text = tempBit.ToString();
            StartCoroutine(BonusTimerUpdater(_playerData.BonusRechargeSeconds));
        }
    }

}