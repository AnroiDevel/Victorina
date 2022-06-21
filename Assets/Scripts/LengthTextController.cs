using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class LengthTextController : MonoBehaviour
    {
        [SerializeField] private Text _text;

        private InputField _inputField;

        private void Start()
        {
            _inputField = GetComponent<InputField>();
        }

        public void LengthUpdate()
        {
            _text.text = _inputField.text.Length.ToString();
        }
    }
}