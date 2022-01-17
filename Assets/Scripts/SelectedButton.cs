using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Victorina
{
    public class SelectedButton : MonoBehaviour
    {
        private Button _button;
        private void Start()
        {
            _button = GetComponent<Button>();
            _button.Select();
        }

    }

}