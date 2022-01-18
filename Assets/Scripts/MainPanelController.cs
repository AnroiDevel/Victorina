using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class MainPanelController : MonoBehaviour
    {
        [SerializeField] Text _nameUser;

        private void OnEnable()
        {
            if(PlayerPrefs.HasKey("Name"))
            _nameUser.text = PlayerPrefs.GetString("Name");
        }
    }

}