using PlayFab;
using PlayFab.ClientModels;
using System;
using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class BonusManager : MonoBehaviour
    {
        [SerializeField] private Text _timeToNext;
        [SerializeField] private Text _bit;
        [SerializeField] private Text _tickets;

        [SerializeField] private GameObject[] _bonusesPlaces;
        [SerializeField] private Sprite _rightErrImg;
        [SerializeField] private Sprite _twoVarRemoveImg;
        [SerializeField] private RewardedAdsButton _rewardedAds;

        private Button _bonusButton;

        private bool _isBonusComplete;

        private Character _player;

        private BonusTimer _bonusTimer;
        private GameData _gameData;

        private PlayFabAccountManager _accountManager;


        private void Awake()
        {
            _gameData = GameData.GetInstance();
            _bonusTimer = BonusTimer.GetInstance();
            _player = _gameData.Player;
            _accountManager = PlayFabAccountManager.Instance;

        }

        private void Start()
        {
            if (_player.Bonus > 0)
                _bonusTimer.StartTimer(_player.BonusSecondsTime);
            _bonusButton = GetComponent<Button>();
            _bonusButton.onClick.AddListener(GetBonus);
            _accountManager.InventoryReceived += OnGetInventory;
        }

        private void OnGetInventory()
        {
            if (_bit)
                _bit.text = _gameData.Player.Money.ToString();
            if (_tickets != null)
                _tickets.text = _gameData.Player.Tickets.ToString();
        }

        private void FixedUpdate()
        {
            if (_isBonusComplete) return;
            _timeToNext.text = _bonusTimer.LeftTime;
            if (_timeToNext.text.Length == 6)
            {
                _bonusButton.interactable = true;
                _isBonusComplete = true;
            }
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


        private void GetBonus()
        {
            _isBonusComplete = false;
            _bonusButton.interactable = false;

            var rnd = UnityEngine.Random.Range(1, 5);
            var itId = "Bonus" + rnd;
            OnGetNumberBonus(rnd);
            PurchaseItemRequest request = new PurchaseItemRequest
            {
                CatalogVersion = "Bonuses",
                ItemId = itId,
                VirtualCurrency = "BS",
                Price = 1,
            };
            PlayFabClientAPI.PurchaseItem(request, ItemsUpdate(), error => Debug.Log(error));
        }

        private Action<PurchaseItemResult> ItemsUpdate()
        {
            return result =>
            {
                Debug.Log("Бонус получен");
                _bonusButton.interactable = false;
                _gameData.Player.Bonus--;

                _accountManager.GetPlayerInventory();

                _bonusTimer.StartTimer(_player.BonusSecondsTime);
                _isBonusComplete = false;
            };
        }
    }
}