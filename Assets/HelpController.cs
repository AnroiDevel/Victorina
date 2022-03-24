using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class HelpController : MonoBehaviour
    {
        [SerializeField] private Button[] _helpBtns;
        [SerializeField] private PlayerData _playerData;

        private bool _isRightErrorComplete;
        public bool IsTwoErrorVarintsComplete;
        public bool IsReloadQuestion;

        public void RightError()
        {
            _playerData.RightError = true;
            foreach (var btn in _helpBtns)
                btn.interactable = false;
            _isRightErrorComplete = true;
        }

        public void ReloadComplete()
        {
            IsReloadQuestion = true;
        }

        internal void Activator()
        {
            _helpBtns[0].interactable = !_isRightErrorComplete;
            _helpBtns[1].interactable = true;
            _helpBtns[2].interactable = !IsTwoErrorVarintsComplete;
            _helpBtns[3].interactable = !IsReloadQuestion;
        }
    }

}