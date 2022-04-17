using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class HelpController : MonoBehaviour
    {
        [SerializeField] private Button[] _helpBtns;
        [SerializeField] private PlayerData _playerData;

        private PurchasesPF _pF = new PurchasesPF();

        private bool _isRightErrorComplete;
        public bool IsTwoErrorVarintsComplete;
        public bool IsTwoErrorBonusReady;
        public bool IsReloadQuestion;

        public bool IsTwoErrorBonus()
        {
            return _playerData.R2 > 0;
        }

        public void PurchaseR2()
        {
            _pF.PurchaseItem("Tickets", "MinusTwo", "R2", 1);
            _playerData.R2--;
        }

        public void SetInteractibleR2Btn(bool val)
        {
            if (_playerData.R2 > 0)
                _helpBtns[2].interactable = val;
            else
                _helpBtns[2].interactable = false;
        }

        public void SetNotInteracttibleAllHelpBtns()
        {
            foreach (var btn in _helpBtns)
                btn.interactable = false;
        }

        public void RightError()
        {
            if (_isRightErrorComplete)
            {
                RightErrorBonus();
                return;
            }

            _playerData.RightError = true;
            foreach (var btn in _helpBtns)
                btn.interactable = false;
            _isRightErrorComplete = true;
        }

        private void RightErrorBonus()
        {
            _playerData.RightError = true;
            foreach (var btn in _helpBtns)
                btn.interactable = false;
            _pF.PurchaseItem("Tickets", "RightError", "RE", 1);
            _playerData.RE--;
        }

        public void ReloadComplete()
        {
            IsReloadQuestion = true;
        }

        private bool GetBonusRE()
        {
            return _playerData.RE > 0;
        }


        internal void Activator()
        {
            _helpBtns[0].interactable = !_isRightErrorComplete || GetBonusRE();
            _helpBtns[1].interactable = true;
            _helpBtns[2].interactable = !IsTwoErrorVarintsComplete || IsTwoErrorBonus();
            _helpBtns[3].interactable = !IsReloadQuestion;
        }
    }

}