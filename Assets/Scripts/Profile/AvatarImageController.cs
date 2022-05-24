using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class AvatarImageController : MonoBehaviour
    {
        [SerializeField] private Image _reitImage;
        [SerializeField] private Image _reitImage2;

        [SerializeField] private Sprite _junior;
        [SerializeField] private Sprite _pro;
        [SerializeField] private Sprite _master;
        [SerializeField] private Sprite _champion;
        [SerializeField] private Sprite _grandChampion;

        private Character _player;

        private void Start()
        {
            var gameData = GameData.GetInstance();
            _player = gameData.Player;
            SetReitImage();
        }


        public void SetReitImage()
        {
            var wRank = _player.WeeklyRank;

            if (wRank > 5)
                _reitImage.sprite = _junior;
            else if (wRank > 3)
                _reitImage.sprite = _pro;
            else if (wRank > 2)
                _reitImage.sprite = _master;
            else if (wRank > 1)
                _reitImage.sprite = _champion;
            else if (wRank == 1)
                _reitImage.sprite = _grandChampion;
            else
                _reitImage.sprite = _junior;

            if (_reitImage2)
                _reitImage2.sprite = _reitImage.sprite;
        }
    }

}