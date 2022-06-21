using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class AvatarManager : MonoBehaviour
    {
        public static AvatarManager Instance;

        #region Fields

        [SerializeField] private Image _avatarImg;

        private Character _player;
        private GameData _gameData;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            Instance = this;
            _gameData = GameData.GetInstance();
            _player = _gameData.Player;
        }

        #endregion


        #region Methods

        public void PickImage()
        {
            NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
            {
                if (path != null)
                {
                    Texture2D texture = NativeGallery.LoadImageAtPath(path, -1);
                    if (texture == null)
                    {
                        Debug.Log("Couldn't load texture from " + path);
                        return;
                    }

                    var avatar = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    var scale = texture.width > texture.height ? (float)texture.width / texture.height : texture.height / (float)texture.width;
                    avatar.name = "SpriteForAvatar";

                    _player.Avatar = avatar;
                    _player.ScaleAvatarCoef = scale;

                    _avatarImg.sprite = avatar;
                    _avatarImg.transform.localScale = Vector3.one * scale;

                    PlayerPrefs.SetString("AvatarUrl", path);
                }
            });
        }

        #endregion    
    }
}