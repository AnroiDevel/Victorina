using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class ProfileThema : MonoBehaviour
    {
        [SerializeField] private Image _profileBack;
        [SerializeField] private Image _exitBtnImg;
        [SerializeField] private Image _lineNameLblImg;
        [SerializeField] private Image _themaSwithBtnImg;


        private void Start()
        {
            ThemaApply();
        }

        public void ThemaApply()
        {
            var activeThema = ThemaConrtoller.ActiveThema;

            if (activeThema == null) return;

            _profileBack.sprite = activeThema.ProfileBackground;
            _exitBtnImg.sprite = activeThema.ExitProfileBtn;
            _lineNameLblImg.sprite = activeThema.LineImg;
            _themaSwithBtnImg.sprite = activeThema.ThemaBtnImg;
        }
    }
}