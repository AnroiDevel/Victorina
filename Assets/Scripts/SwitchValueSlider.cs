using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class SwitchValueSlider : MonoBehaviour
    {
        private Slider _slider;
        [SerializeField] private Image _imageFill;

        private void Start()
        {
            _slider = GetComponent<Slider>();
        }
        public void SwithSliderDir()
        {
            if (_slider != null && _imageFill != null)
            {
                var dir = _slider.direction;
                if (dir == 0)
                {
                    _slider.direction = ++dir;
                    _imageFill.color = Color.green;
                }
                else
                {
                    _slider.direction = --dir;
                    _imageFill.color = Color.red;
                }
            }
            else print("No slider or image");
        }
    }

}
