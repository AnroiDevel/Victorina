using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class TicketManager : MonoBehaviour
    {
        private const string TicketEnter = "вход по билету";
        private const string ContinueGame = "продолжить игру";
        private const string VipEnter = "VIP вход";

        [SerializeField] private Text _priceBitTicket;
        [SerializeField] private Text _money;

        [SerializeField] private Button _enterBtn;
        [SerializeField] private Button _exitBtn;

        [SerializeField] private GameObject _ticketPricePanel;
        [SerializeField] private GameObject _welcomePanel;
        [SerializeField] private GameObject _progressPanel;

        [SerializeField] private Animator _animator;
        [SerializeField] private AudioSource _animTicket;
        [SerializeField] private AudioSource _byeTicket;

        private QuestionLoader _questionLoader;
        private bool _isStartBtnListenerComplete;
        private Character _player;
        private PlayFabAccountManager _accountManager;


        private void Awake()
        {
            var gameData = GameData.GetInstance();
            _player = gameData.Player;
            _accountManager = PlayFabAccountManager.GetInstance();
        }


        private void Start()
        {
            _questionLoader = gameObject.GetComponent<QuestionLoader>();
            _money.text = _player.Money.ToString();
            _accountManager.GetPlayerInventory();
        }


        private void OnEnable()
        {
            InitComplete();
            _exitBtn.interactable = true;
        }


        private void InitComplete()
        {
            if (_player.Tickets > 0)
                _ticketPricePanel.SetActive(false);

            if (_enterBtn && !_isStartBtnListenerComplete)
                CreateClikEvent(_enterBtn);
        }


        private void CreateClikEvent(Button button)
        {
            if (!_isStartBtnListenerComplete)
                button.gameObject.SetActive(true);

            if (_player.IsVip && !_player.IsPlay)
            {
                _ticketPricePanel?.SetActive(false);
                button.GetComponentInChildren<Text>().text = VipEnter;
                button.onClick.AddListener(EnterOnVip);
            }
            else if (!_player.IsPlay && _player.Tickets <= 0)
            {
                _ticketPricePanel?.SetActive(true);
                SetPriceTicket(_player.PriceBitTicket.ToString());
                button.onClick.AddListener(BuyingTicket);
                if (_player.Money < _player.PriceBitTicket)
                {
                    button.interactable = false;
                    Debug.Log("Недостаточно средств для покупки билета");
                    return;
                }

            }
            else if (!_player.IsPlay && _player.Tickets > 0)
            {
                button.GetComponentInChildren<Text>().text = TicketEnter;
                button.onClick.AddListener(EnterOnTicket);
            }
            else if (_player.IsPlay)
            {
                _ticketPricePanel?.SetActive(false);
                button.GetComponentInChildren<Text>().text = ContinueGame;
                button.onClick.AddListener(ContinuePlay);
            }
        }


        private void ContinuePlay()
        {
            _enterBtn.interactable = false;
            _questionLoader?.LoadOneQuestion();
            _isStartBtnListenerComplete = true;
            _progressPanel.SetActive(true);
            _welcomePanel.SetActive(false);
        }


        private void EnterOnTicket()
        {
            _enterBtn.interactable = false;
            BuyingPlayTocken("TI", 1);
            AnimateEquipTicket();
            _questionLoader?.LoadOneQuestion();
            _isStartBtnListenerComplete = true;
            _progressPanel.SetActive(true);
            _welcomePanel.SetActive(false);
        }


        private void EnterOnVip()
        {
            _enterBtn.interactable = false;
            BuyingPlayTocken("BT", 0);
            AnimateEquipTicket();
            _questionLoader?.LoadOneQuestion();
            _isStartBtnListenerComplete = true;
            _progressPanel.SetActive(true);
            _welcomePanel.SetActive(false);
        }


        private void AnimateEquipTicket()
        {
            _animator.enabled = true;
            _animator.Play("TicketTrash");
            _animTicket?.Play();
        }


        public void BuyingTicket()
        {
            _enterBtn.interactable = false;
            _byeTicket.enabled = true;
            if (PlayerPrefs.GetInt("Sfx") > 0)
                _byeTicket.Play();
            _money.text = (int.Parse(_money.text) - _player.PriceBitTicket).ToString();

            PurchaseItemRequest request = new PurchaseItemRequest
            {
                CatalogVersion = "Tickets",
                ItemId = "BitTicket",
                VirtualCurrency = "BT",
                Price = _player.PriceBitTicket,
            };
            PlayFabClientAPI.PurchaseItem(request, result => OnBuyingTicketComplete(), error => Debug.Log(error));
        }

        private void BuyingPlayTocken(string vc, int price)
        {
            PurchaseItemRequest request = new PurchaseItemRequest
            {
                CatalogVersion = "Tickets",
                ItemId = "PlayToken",
                VirtualCurrency = vc,
                Price = price,
            };
            PlayFabClientAPI.PurchaseItem(request, PlayTockenComplete, error => Debug.Log(error));
        }

        private void PlayTockenComplete(PurchaseItemResult result)
        {
            _accountManager.GetPlayerInventory();
            _byeTicket.enabled = false;
            _player.IsPlay = true;
            _questionLoader?.LoadOneQuestion();
            _progressPanel.SetActive(true);
            _welcomePanel.SetActive(false);
        }

        private void OnBuyingTicketComplete()
        {
            BuyingPlayTocken("BT", 0);
            _isStartBtnListenerComplete = true;
            Debug.Log("Билет куплен");
        }

        private void SetPriceTicket(string price)
        {
            _ticketPricePanel.gameObject.SetActive(true);
            _priceBitTicket.text = price;
        }

        private IEnumerator WaitLoadFirstQuestion(bool isLoadComplete)
        {
            _questionLoader?.LoadOneQuestion();
            while (!isLoadComplete)
            {
                isLoadComplete = _questionLoader.IsLoadComplete;
                yield return new WaitForSeconds(1);
            }
            _progressPanel.SetActive(true);
            _welcomePanel.SetActive(false);
        }

    }

}