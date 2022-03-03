using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class ProfileThema : MonoBehaviour
    {
        [SerializeField] private Image _backgroundImg;
        [SerializeField] private GameObject _namePanel;
        [SerializeField] private GameObject _themeBtn;
        [SerializeField] private GameObject _enterFBBtn;
        [SerializeField] private GameObject _sendFrendBtn;

        private void Start()
        {
            var actThema = ThemaConrtoller.ActiveThema;

            ThemaApply();

        }

        public void ThemaApply()
        {
            _backgroundImg.sprite = ThemaConrtoller.ActiveThema?.ProfileBackground;

            var rt = _namePanel.GetComponent<RectTransform>();
            rt.anchorMin = ThemaConrtoller.ActiveThema.NamePanelAnchoredMinPosition;
            rt.anchorMax = ThemaConrtoller.ActiveThema.NamePanelAnchoredMaxPosition;

            rt = _themeBtn.GetComponent<RectTransform>();
            rt.anchorMin = ThemaConrtoller.ActiveThema.ThemeBtnAnchoredMinPosition;
            rt.anchorMax = ThemaConrtoller.ActiveThema.ThemeBtnAnchoredMaxPosition;

            rt = _enterFBBtn.GetComponent<RectTransform>();
            rt.anchorMin = ThemaConrtoller.ActiveThema.EnterFBBtnAnchoredMinPosition;
            rt.anchorMax = ThemaConrtoller.ActiveThema.EnterFBBtnAnchoredMaxPosition;

            rt = _sendFrendBtn.GetComponent<RectTransform>();
            rt.anchorMin = ThemaConrtoller.ActiveThema.SendFrendBtnAnchoredMinPosition;
            rt.anchorMax = ThemaConrtoller.ActiveThema.SendFrendBtnAnchoredMaxPosition;
        }
    }
}