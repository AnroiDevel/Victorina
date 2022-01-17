using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class Renamer : MonoBehaviour
    {
        [SerializeField] private Button _renameBtn;
        [SerializeField] private Text _currentName;
        [SerializeField] private Text _newName;
        [SerializeField] private InputField _inputField;

        private void Start()
        {
            _renameBtn.onClick.AddListener(Switcher);
        }

        private void Switcher()
        {
            if (!_inputField.isActiveAndEnabled)
            {
                _inputField.gameObject.SetActive(true);
            }
            else
            {
                _inputField.gameObject.SetActive(false);
                _currentName.text = _newName.text;
            }
        }

    }


}