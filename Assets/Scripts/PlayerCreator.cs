using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class PlayerCreator : MonoBehaviour
    {
        [SerializeField] private Text _name;
        private SceneLoader _sceneLoader;
        private Character _player;


        private void Awake()
        {
            var gameData = GameData.GetInstance();
            _player = gameData.Player;
        }


        private void Start()
        {
            _sceneLoader = GetComponent<SceneLoader>();
        }


        public void CreateNewPlayer()
        {
            SetDisplayName(_name.text);
        }


        private void SetDisplayName(string name)
        {
            if (name.Length < 3)
                name += "  ";
            PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
            {
                DisplayName = name
            }, result => LoadNextScene(), error => Debug.LogError(error.GenerateErrorReport()));
        }


        private void LoadNextScene()
        {
            _player.Name = _name.text;
            _sceneLoader.LoadGameScene("Victorina");
        }
    }

}