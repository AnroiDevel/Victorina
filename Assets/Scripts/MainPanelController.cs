using System.IO;
using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class MainPanelController : MonoBehaviour
    {
        private const string Avatar = "/photo.png";
        [SerializeField] Text _nameUser;
        [SerializeField] RawImage _userImage;

        private string _pathForUserImage;
        private Texture _avatarTexture;

        private void Start()
        {
            _pathForUserImage = Application.persistentDataPath + Avatar;

            if (PlayerPrefs.HasKey("Name"))
                _nameUser.text = PlayerPrefs.GetString("Name");

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
                _avatarTexture = www.texture;
                _userImage.texture = _avatarTexture;
            }


        }
    }

}