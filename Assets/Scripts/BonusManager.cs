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

        [SerializeField] private GameObject[] _bonusesPlaces;
        [SerializeField] private Sprite _rightErrImg;
        [SerializeField] private Sprite _twoVarRemoveImg;
        [SerializeField] private RewardedAdsButton _rewardedAds;

        private Button _bonusButton;

        private bool _isBonusComplete;
        private DateTime _startTime;


        private void Start()
        {
            _isBonusComplete = _playerData.IsBonusReady;

            _bonusButton = GetComponent<Button>();
            if (_isBonusComplete)
                OnBonusComplete(true);
            _playerData.GetNumberBonus += OnGetNumberBonus;
        }

        private void OnGetNumberBonus(int numberBonus)
        {
            foreach (var go in _bonusesPlaces)
            {
                if (go == null) return;
                go.SetActive(false);
            }

            if (numberBonus == 1)
            {
                _bonusesPlaces[0].SetActive(true);
                _bonusesPlaces[2].SetActive(true);
            }
            else if (numberBonus == 2)
            {
                _bonusesPlaces[3].SetActive(true);
                _bonusesPlaces[4].SetActive(true);
            }
            else if (numberBonus == 3)
            {
                _bonusesPlaces[0].SetActive(true);
                _bonusesPlaces[5].SetActive(true);
                _bonusesPlaces[5].GetComponent<Image>().sprite = _rightErrImg;
            }
            else if (numberBonus == 4)
            {
                _bonusesPlaces[0].SetActive(true);
                _bonusesPlaces[5].SetActive(true);
                _bonusesPlaces[5].GetComponent<Image>().sprite = _twoVarRemoveImg;
            }
        }

        private void OnBonusComplete(bool val = false)
        {
            _bonusButton.interactable = val;
            _timeToNext.text = "Готово";
        }

        private void FixedUpdate()
        {
            if (!_bonusButton.interactable)
                if (_playerData.IsBonusReady)

                    OnBonusComplete(true);
                else
                    _timeToNext.text = _playerData.RechargedBonusT.ToLongTimeString();
        }




        public void GetBonus()
        {
            //if (_rewardedAds.IsShowAdButtonReady) return;
            _playerData.GetBonus();
            _isBonusComplete = false;
            _bonusButton.interactable = false;
            var tempBit = _playerData.Bit;
            _bit.text = tempBit.ToString();
        }
    }

}