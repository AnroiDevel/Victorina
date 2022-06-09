using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    class RateStarsSelector : MonoBehaviour
    {

        public Button NextBtn;
        [SerializeField] private GameObject _prevBtn;
        [SerializeField] private Sprite _disabledStar;
        [SerializeField] private Sprite _enabledStar;

        private const string Next = "оценить";

        [SerializeField] private Image[] _gradeImages;

        public int UserGrade { get; private set; }

        private void OnEnable()
        {
            SetDefaultRateImage();
        }

        public void SetUserGrade(int value)
        {
            SetDefaultRateImage();

            for (var i = 0; i < value; i++)
            {
                _gradeImages[i].sprite = _enabledStar;
            }
            UserGrade = value;
            NextBtn.GetComponentInChildren<Text>().text = Next;
            NextBtn.interactable = true;
            NextBtn.gameObject.SetActive(true);

            if(_prevBtn != null)
                _prevBtn.SetActive(true);
        }

        public void SetDefaultRateImage()
        {
            foreach (var img in _gradeImages)
                img.sprite = _disabledStar;
        }

    }
}