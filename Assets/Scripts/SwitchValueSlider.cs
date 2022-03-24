using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class SwitchValueSlider : MonoBehaviour
    {
        private Slider _slider;
        [SerializeField] private Image _imageFill;
        [SerializeField] private AudioSource _audioSource;

        private void Start()
        {
            _slider = GetComponent<Slider>();
            if (PlayerPrefs.HasKey(_slider.name))
            {
                _slider.direction = (Slider.Direction)PlayerPrefs.GetInt(_slider.name);
            }
            else _slider.direction = (Slider.Direction)1;
            _imageFill.color = _slider.direction > 0 ? Color.green : Color.red;
        }
        public void SwithSliderDir()
        {
            if (_slider != null && _imageFill != null)
            {

                _slider.direction = (Slider.Direction)(_slider.direction > 0 ? 0 : 1);

                PlayerPrefs.SetInt(_slider.name, (int)_slider.direction);

                _imageFill.color = _slider.direction > 0 ? Color.green : Color.red;

                if (PlayerPrefs.GetInt("Sfx") != 0)
                    _audioSource.Play();
            }
            else print("No slider or image");
        }
    }

}
