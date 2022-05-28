using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class HelpController : MonoBehaviour
    {
        [SerializeField] private Button[] _helpBtns;

        private PurchasesPF _pF = new PurchasesPF();

        private bool _isRightErrorComplete;
        public bool IsTwoErrorVarintsComplete;
        public bool IsTwoErrorBonusReady;
        public bool IsReloadQuestion;

        private Character _player;


        private void Awake()
        {
            _player = GameData.GetInstance().Player;
        }


        public bool IsTwoErrorBonus()
        {
            return _player.HelpicR2 > 0;
        }


        public void PurchaseR2()
        {
            _pF.PurchaseItem("Tickets", "MinusTwo", "R2", 1);
            _player.HelpicR2--;
        }


        public void SetInteractibleR2Btn(bool val)
        {
            if (_player.HelpicR2 > 0)
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

            _player.RightError = true;
            foreach (var btn in _helpBtns)
                btn.interactable = false;
            _isRightErrorComplete = true;
        }


        private void RightErrorBonus()
        {
            _player.RightError = true;
            foreach (var btn in _helpBtns)
                btn.interactable = false;
            _pF.PurchaseItem("Tickets", "RightError", "RE", 1);
            _player.HelpicRE--;
        }


        public void ReloadComplete()
        {
            IsReloadQuestion = true;
        }


        private bool GetBonusRE()
        {
            return _player.HelpicRE > 0;
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