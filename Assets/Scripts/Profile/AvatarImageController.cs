using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class AvatarImageController : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private Image _reitImage;

        [SerializeField] private Sprite _junior;
        [SerializeField] private Sprite _pro;
        [SerializeField] private Sprite _master;
        [SerializeField] private Sprite _champion;
        [SerializeField] private Sprite _grandChampion;


        private void Start()
        {
            SetReitImage();
        }

        private void OnEnable()
        {
            //SetReitImage();
        }

        public void SetReitImage()
        {
            var wRank = _playerData.WeeklyRank;

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
        }
    }

}