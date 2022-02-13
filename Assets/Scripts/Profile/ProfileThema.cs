using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class ProfileThema : MonoBehaviour
    {
        [SerializeField] private Image _backgroundImg;

        private void Start()
        {
            _backgroundImg.sprite = ThemaConrtoller.ActiveThema?.ProfileBackground;
        }

        public void ThemaApply() 
        {
            _backgroundImg.sprite = ThemaConrtoller.ActiveThema.ProfileBackground;
        }
    }
}