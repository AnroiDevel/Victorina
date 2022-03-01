using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Victorina
{
    public class ButtleBitController : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;


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
            var sceneLoader =  gameObject?.GetComponent<SceneLoader>();
            if(sceneLoader != null)
            sceneLoader.LoadGameScene("Victorina");
        }

        public void GameOver()
        {
            _playerData.ConsumeItem("PlayToken");
        }

        public void GetWinAndGameOver()
        {
            GameOver();
        }
    }

}