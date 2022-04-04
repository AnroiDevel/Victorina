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
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private Text _priceBitTicket;
        [SerializeField] private Text _money;
        [SerializeField] private GameObject _ticketPricePanel;
        [SerializeField] private Button _enterBtn;
        [SerializeField] private GameObject _welcomePanel;
        [SerializeField] private GameObject _progressPanel;
        [SerializeField] private Animator _animator;
        [SerializeField] private AudioSource _animTicket;
        [SerializeField] private AudioSource _byeTicket;

        private QuestionLoader _questionLoader;
        private bool _isStartBtnListenerComplete;


        private void Start()
        {
            _playerData.InitComplete += InitComplete;

            _playerData.Init();

            _questionLoader = gameObject.GetComponent<QuestionLoader>();
            _money.text = _playerData.Bit.ToString();
            SetPriceTicket(_playerData.PriceBitTicket.ToString());
            if (_playerData.TicketsBit > 0)
                _ticketPricePanel.SetActive(false);
        }

        private void InitComplete()
        {
            if (_enterBtn && !_isStartBtnListenerComplete)
                CreateClikEvent(_enterBtn);

        }

        private void CreateClikEvent(Button button)
        {
            if (!_isStartBtnListenerComplete)
                button.gameObject.SetActive(true);

            if (_playerData.IsVip && !_playerData.IsPlayed)
            {
                _ticketPricePanel?.SetActive(false);
                button.GetComponentInChildren<Text>().text = "VIP вход";
                button.onClick.AddListener(EnterOnVip);
            }
            else if (!_playerData.IsPlayed && _playerData.TicketsBit <= 0)
                button.onClick.AddListener(BuyingTicket);
            else if (!_playerData.IsPlayed && _playerData.TicketsBit > 0)
            {
                button.GetComponentInChildren<Text>().text = TicketEnter;
                button.onClick.AddListener(EnterOnTicket);
            }
            else if (_playerData.IsPlayed)
            {
                _ticketPricePanel?.SetActive(false);
                button.GetComponentInChildren<Text>().text = "продолжить игру";
                button.onClick.AddListener(ContinuePlay);
            }
        }

        private void ContinuePlay()
        {
            _enterBtn.gameObject.SetActive(false);
            StartCoroutine(WaitLoadFirstQuestion(false));

            _isStartBtnListenerComplete = true;
        }

        private void EnterOnTicket()
        {
            _enterBtn.gameObject.SetActive(false);

            _playerData.ConsumeItem("BitTicket");
            BuyingPlayTocken();
            AnimateEquipTicket();
            StartCoroutine(WaitLoadFirstQuestion(false));

            _isStartBtnListenerComplete = true;
        }

        private void EnterOnVip()
        {
            _enterBtn.gameObject.SetActive(false);
            BuyingPlayTocken();
            AnimateEquipTicket();
            StartCoroutine(WaitLoadFirstQuestion(false));
            _isStartBtnListenerComplete = true;
        }

        private void AnimateEquipTicket()
        {
            _animator.enabled = true;
            _animator.Play("TicketTrash");
            _animTicket.Play();
        }

        public void BuyingTicket()
        {
            _enterBtn.gameObject.SetActive(false);

            if (_playerData.Bit < _playerData.PriceBitTicket)
            {
                Debug.Log("Недостаточно средств для покупки билета");
                return;
            }

            _byeTicket.enabled = true;
            if (PlayerPrefs.GetInt("Sfx") > 0)
                _byeTicket.Play();

            _money.text = (int.Parse(_money.text) - _playerData.PriceBitTicket).ToString();

            PurchaseItemRequest request = new PurchaseItemRequest
            {
                CatalogVersion = "Tickets",
                ItemId = "BitTicket",
                VirtualCurrency = "BT",
                Price = (int)_playerData.PriceBitTicket,

            };
            PlayFabClientAPI.PurchaseItem(request, result => OnBuyingTicketComplete(), error => Debug.Log(error));
        }

        private void BuyingPlayTocken()
        {
            PurchaseItemRequest request = new PurchaseItemRequest
            {
                CatalogVersion = "Tickets",
                ItemId = "PlayToken",
                VirtualCurrency = "BT",
                Price = 0,

            };
            PlayFabClientAPI.PurchaseItem(request, PlayTockenComplete, error => Debug.Log(error));
        }

        private void PlayTockenComplete(PurchaseItemResult result)
        {
            _byeTicket.enabled = false;
            _playerData.ConsumeItem("BitTicket");
            StartCoroutine(WaitLoadFirstQuestion(false));
        }

        private void OnBuyingTicketComplete()
        {
            BuyingPlayTocken();
            _isStartBtnListenerComplete = true;
            Debug.Log("Билет куплен");
        }

        private void SetPriceTicket(string price)
        {
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