using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Victorina
{
    public class QuitApp : MonoBehaviour
    {
        [SerializeField] private GameObject _exitWindow;
        [SerializeField] private GameObject _mainMenuWindow;


        private void Update()
        {
            if (_mainMenuWindow.activeInHierarchy)
                if (Input.GetKeyDown(KeyCode.Escape))
                    _exitWindow.SetActive(true);
        }

        public void Exit()
        {
            Application.Quit();
        }
    }

}