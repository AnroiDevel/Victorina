using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class FileScript : MonoBehaviour
    {
        public Text FileNameText;
        public RawImage Image;

        [SerializeField] private GameObject _avatar;

        [HideInInspector]
        public int Index;

        public void SetAvatar()
        {
            AvatarManager.Instance.SelectAvatar(Index);
            _avatar.SetActive(true);    
        }
    }

}