using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Victorina
{
    public class QuitApp : MonoBehaviour
    {
        [SerializeField] private GameObject _exitWindow;
        [SerializeField] private GameObject _mainMenuWindow;

        [SerializeField] private GameObject[] _otherPanels;

        public Action ActivateMainWindow;




        private void Update()
        {
            if (_mainMenuWindow.activeInHierarchy)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                    _exitWindow.SetActive(true);

            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                foreach (var go in _otherPanels)
                    go.SetActive(false);
                _mainMenuWindow.SetActive(true);
                ActivateMainWindow?.Invoke();
            }

        }

        public void Exit()
        {
            Application.Quit();
        }
    }

}