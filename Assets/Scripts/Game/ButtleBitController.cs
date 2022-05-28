using System;
using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class ButtleBitController : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private Text _timeGame;

        private PlayFabAccountManager _accountManager;
        private Character _player;


        private void Awake()
        {
            _accountManager = PlayFabAccountManager.GetInstance();
            var gameData = GameData.GetInstance();
            _player = gameData.Player;
            _player.IsTrain = false;
        }


        private void OnEnable()
        {
            _accountManager.InventoryReceived += OnGetInventory;
            _accountManager.ConsumeComplete += OnConsumeComplete;
            _accountManager.PrizeReceived += OnGetPrize;
        }

        private void OnGetPrize()
        {
            _accountManager.GetPlayerInventory();
        }

        private void OnGetInventory()
        {
            if (_player.PlayToken != null)
                GameOver();
        }

        private void OnDisable()
        {
            _accountManager.ConsumeComplete -= OnConsumeComplete;
        }


        private void OnConsumeComplete()
        {
            _player.LastGameTime = DateTime.MinValue.AddSeconds((int)(DateTime.UtcNow - _player.StartGameTime).Value.TotalSeconds).ToLongTimeString();
            _timeGame.text = _player.LastGameTime;
            PlayerPrefs.SetInt("CurrentStep", 0);
            _player.PlayToken = null;
            _player.IsPlay = false;
        }


        public void GameOver()
        {
            _accountManager.ConsumeItem(_player.PlayToken);
        }


        public void GetWinAndGameOver()
        {
            var level = PlayerPrefs.GetInt("CurrentStep");
            _accountManager.GetPrize(level);
            _accountManager.SubmitScore(level);
        }
    }
}