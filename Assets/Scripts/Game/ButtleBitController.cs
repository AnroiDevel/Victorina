using System;
using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class ButtleBitController : MonoBehaviour
    {
        #region Fields

        [SerializeField] private PlayerData _playerData;
        [SerializeField] private Text _timeGame;
        private PlayFabAccountManager _accountManager;
        private Character _player;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            _accountManager = PlayFabAccountManager.Instance;
            var gameData = GameData.GetInstance();
            _player = gameData.Player;
            _player.IsTrain = false;
            _accountManager.ConsumeComplete += OnConsumeComplete;
            _accountManager.PrizeReceived += OnGetPrize;
        }

        #endregion


        #region Methods

        private void OnGetPrize()
        {
            _accountManager.GetPlayerInventory();
        }

        private void OnConsumeComplete()
        {
            _player.LastGameTime = DateTime.MinValue.AddSeconds((int)(DateTime.UtcNow - _player.StartGameTime).Value.TotalSeconds).ToLongTimeString();
            _player.LastGameTimeSec = (int)(DateTime.UtcNow - _player.StartGameTime).Value.TotalSeconds;
            if (_timeGame)
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
            GameOver();
            var level = PlayerPrefs.GetInt("CurrentStep");
            _accountManager.GetPrize(level);
            _accountManager.SubmitScore(level);
        }

        #endregion    
    }
}