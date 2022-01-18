using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class AnimateButtons : MonoBehaviour
    {
        private Button[] _buttons;
        [SerializeField] Button _mainButton;


        private void Start()
        {
            _buttons = GetComponentsInChildren<Button>();
            foreach (var button in _buttons)
                button.onClick.AddListener(() => AnimationSelected(button));

            _mainButton.onClick.Invoke();
        }

        private void AnimationSelected(Button btn)
        {
            foreach (var button in _buttons)
            {
                button.animator.SetBool("Selected", false);
                button.animator.SetBool("Normal", true);
            }

            btn.animator.SetBool("Selected", true);
            btn.animator.SetBool("Normal", false);
        }

    }

}