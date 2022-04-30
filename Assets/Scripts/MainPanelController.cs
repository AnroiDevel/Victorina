using System.IO;
using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class MainPanelController : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private Text _debugTest;

        private const string Avatar = "/photo.png";
        [SerializeField] Image _userImage;

        private string _pathForUserImage;
        private Texture _avatarTexture;

        private void Start()
        {
            _userImage.sprite=_playerData.Avatar;

            _userImage.transform.localScale = Vector3.one * _playerData.ScaleImageAvatarCoef;

            //if (_playerData.Avatar != null)
            //    _userImage.sprite = _playerData.Avatar;
            //else if (_playerData.PathFileAvatar != string.Empty)
            //{
            //    //_debugTest.text = _playerData.PathFileAvatar;
            //    _pathForUserImage = Application.persistentDataPath + Avatar;
            //    _playerData.PathFileAvatar = _pathForUserImage;
            //    _playerData.SetAvatar();
            //}
            //else _playerData.SetAvatar();



            //if (File.Exists(_pathForUserImage))
            //{
            //    var imageBytes = File.ReadAllBytes(_pathForUserImage);
            //    var firstLeftCC = new Texture2D(10, 10);
            //    firstLeftCC.LoadImage(imageBytes);
            //    _userImage.texture = firstLeftCC;
            //}
            //else
            //{
            //    if (PlayerPrefs.HasKey("AvatarUrl"))
            //        _pathForUserImage = PlayerPrefs.GetString("AvatarUrl");
            //    WWW www = new WWW(_pathForUserImage);
            //    _avatarTexture = www.texture;
            //    _userImage.texture = _avatarTexture;
            //}


        }

        private void OnEnable()
        {
            //var coef = _playerData.ScaleImageAvatarCoef;
            //_userImage.transform.localScale = Vector3.one * coef;

        }
    }

}