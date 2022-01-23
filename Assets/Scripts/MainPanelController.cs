using System.IO;
using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class MainPanelController : MonoBehaviour
    {
        [SerializeField] Text _nameUser;
        [SerializeField] RawImage _userImage;

        private string _pathForUserImage;

        private void OnEnable()
        {
            if (PlayerPrefs.HasKey("Name"))
                _nameUser.text = PlayerPrefs.GetString("Name");

            _pathForUserImage = Application.persistentDataPath + "/photo.png";
            if (File.Exists(_pathForUserImage))
            {
                var imageBytes = File.ReadAllBytes(_pathForUserImage);
                var firstLeftCC = new Texture2D(10, 10);
                firstLeftCC.LoadImage(imageBytes);
                _userImage.texture = firstLeftCC;
            }
            else
            {
                if (PlayerPrefs.HasKey("AvatarUrl"))
                    _pathForUserImage = PlayerPrefs.GetString("AvatarUrl");
                WWW www = new WWW(_pathForUserImage);
                _userImage.texture = www.texture;
            }


        }
    }

}