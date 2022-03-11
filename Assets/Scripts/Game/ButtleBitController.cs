using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class ButtleBitController : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private Text _timeGame;

        private LeaderBoardsController _boardsController;


        private void Start()
        {
            _boardsController = GetComponent<LeaderBoardsController>();
        }

        private void OnEnable()
        {
            _playerData.ConsumeComplete += OnConsumeComplete;
        }

        private void OnDisable()
        {
            _playerData.ConsumeComplete -= OnConsumeComplete;
        }

        private void OnConsumeComplete(string item)
        {
            if (item != "PlayToken") return;
            _timeGame.text = _playerData.LastGameTime;
            PlayerPrefs.SetInt("CurrentStep", 0);
        }

        public void GameOver()
        {
            _playerData.ConsumeItem("PlayToken");
        }

        public void GetWinAndGameOver()
        {
            var level = PlayerPrefs.GetInt("CurrentStep");
            _playerData.GetPrize(level);
            GameOver();
            _playerData.SubmitScore(level);
            //var sceneLoader = gameObject?.GetComponent<SceneLoader>();
            //if (sceneLoader != null)
            //    sceneLoader.LoadGameScene("Victorina");

        }
    }

}