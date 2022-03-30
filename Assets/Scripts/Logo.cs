using System.Collections;
using UnityEngine;


namespace Victorina
{
    public class Logo : MonoBehaviour
    {
        [SerializeField] private float _time;
        [SerializeField] private PlayerData _playerData;

        private void Start()
        {
            PlayerPrefs.SetInt("Sfx", 1);
            PlayerPrefs.SetInt("Music", 1);

            StartCoroutine(LogoPlay());
            _playerData.LoginComplete += LoadGame;
        }

        private IEnumerator LogoPlay()
        {
            yield return new WaitForSeconds(_time);

            LoadNextScena();
        }

        private void LoadNextScena()
        {
            var sceneLoader = GetComponent<SceneLoader>();
            if (_playerData.IsNewVersionApp || _playerData.IsNewPlayer)
                sceneLoader.LoadGameScene("Confidencial");
            else
                _playerData.Login();
        }

        private void LoadGame()
        {
            var sceneLoader = GetComponent<SceneLoader>();
            sceneLoader.LoadGameScene("Victorina");

        }
    }

}