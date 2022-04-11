using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class Renamer : MonoBehaviour
    {
        private GameObject _profilePanel;
        [SerializeField] private Button _renameBtn;
        [SerializeField] private Text _currentName;
        [SerializeField] private Text _newName;
        [SerializeField] private InputField _inputField;
        [SerializeField] private PlayerData _playerData;

        Button[] _othersBtns;

        private void Start()
        {
            _profilePanel = GameObject.Find("ProfilePanel");
            _renameBtn.onClick.AddListener(Switcher);
            _othersBtns = _profilePanel.GetComponentsInChildren<Button>();
            _currentName.text = _playerData.Name;
        }

        private void OnEnable()
        {
            if (PlayerPrefs.HasKey("Name"))
            {
                _currentName.text = PlayerPrefs.GetString("Name");
            }
        }

        public void Switcher()
        {
            if (!_inputField.isActiveAndEnabled)
            {
                _inputField.gameObject.SetActive(true);
                _inputField.text = _currentName.text;
                _newName.horizontalOverflow = HorizontalWrapMode.Wrap;
                _inputField.Select();
                _currentName.gameObject.SetActive(false);
                _renameBtn.GetComponent<Image>().color = Color.red;
                _profilePanel.GetComponent<Image>().color = Color.grey;
                ActivateButtons(false);
            }
            else
            {
                _inputField.gameObject.SetActive(false);
                if (_newName.text.Length > 2)
                {
                    PlayerPrefs.SetString("Name", _newName.text);
                    _currentName.text = _newName.text;
                }

                _currentName.gameObject.SetActive(true);
                _renameBtn.GetComponent<Image>().color = Color.white;
                _profilePanel.GetComponent<Image>().color = Color.white;
                ActivateButtons(true);
                _playerData.SetDisplayName(_newName.text);
            }
        }

        private void ActivateButtons(bool setActivate)
        {
            foreach (var btn in _othersBtns)
                if (btn != _renameBtn)
                    btn.interactable = setActivate;
        }

    }


}