using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class ConfidencialThema : MonoBehaviour
    {
        [SerializeField] private Image _backConf;

        private void Start()
        {
            _backConf.sprite = ThemaConrtoller.ActiveThema?.Confidential;
        }
    }

}