using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Victorina
{
    public class AchivHelper : MonoBehaviour
    {
        [SerializeField] private Image _infoPanelImage;
        [SerializeField] private GameObject _infoPanel;
        private GameObject[] _achivGOs;
        private Text _infoText;

        private void OnEnable()
        {
            if (_achivGOs != null) return;
            _infoText = _infoPanel.GetComponentInChildren<Text>();
            _achivGOs = GameObject.FindGameObjectsWithTag("Achivka");
            for (int i = 0; i < _achivGOs.Length; i++)
            {
                EventTrigger trigger = _achivGOs[i].AddComponent<EventTrigger>();
                EventTrigger.Entry entry = new EventTrigger.Entry();
                entry.eventID = EventTriggerType.PointerClick;
                entry.callback.AddListener((result) => { SwitchImage(trigger); });
                trigger.triggers.Add(entry);
            }
        }

        private void SwitchImage(EventTrigger trigger)
        {
            _infoPanel.SetActive(true);

            var targetImg = trigger.GetComponentsInChildren<Image>()[3];
            _infoPanelImage.sprite = targetImg.sprite;

            var targetText = trigger.GetComponentInChildren<Text>();
            if (targetText != null)
                _infoText.text = targetText.text;
            else _infoText.text = "Это что то значит";
        }
    }

}