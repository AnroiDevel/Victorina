using UnityEngine;


namespace Victorina
{
    public class Options : MonoBehaviour
    {
        [SerializeField] private GameObject _rewievPanel;

        #region Methods

        public void SendMail()
        {
            Application.OpenURL("mailto:anroidevel@gmail.com");
        }

        public void GetMarkApp()
        {
            var mark = GameData.GetInstance().Player.MarkApp;
            if (mark < 6)
                _rewievPanel.SetActive(true);
        }

        #endregion   
    }
}