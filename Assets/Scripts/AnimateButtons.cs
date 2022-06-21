using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class AnimateButtons : MonoBehaviour
    {
        #region Fields

        private Button[] _buttons;
        [SerializeField] Button _mainButton;
        [SerializeField] AudioSource _audioSource;
        [SerializeField] private QuitApp _quitApp;

        #endregion


        #region Properties

        private bool IsMute => PlayerPrefs.HasKey("Sfx") ? PlayerPrefs.GetInt("Sfx") == 0 : false;

        #endregion


        #region UnityMethods

        private void Start()
        {
            _buttons = GetComponentsInChildren<Button>();
            foreach (var button in _buttons)
                button.onClick.AddListener(() => AnimationSelected(button));

            _mainButton.onClick.Invoke();
            _quitApp.ActivateMainWindow += ReturnToMainWindow;
        }

        #endregion


        #region Methods

        private void ReturnToMainWindow()
        {
            AnimationSelected(_mainButton);
        }

        public void AnimationSelected(Button btn)
        {
            foreach (var button in _buttons)
            {
                button.animator.SetBool("Selected", false);
                button.animator.SetBool("Normal", true);
            }

            btn.animator.SetBool("Selected", true);
            btn.animator.SetBool("Normal", false);

            if (!IsMute)
                _audioSource.Play();
        }

        #endregion
    }
}