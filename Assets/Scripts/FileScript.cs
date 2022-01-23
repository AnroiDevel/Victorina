using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class FileScript : MonoBehaviour
    {
        public Text FileNameText;
        public RawImage Image;

        [HideInInspector]
        public int Index;

        public void OnCklick()
        {
            AvatarManager.Instance.SelectAvatar(Index);
        }
    }

}