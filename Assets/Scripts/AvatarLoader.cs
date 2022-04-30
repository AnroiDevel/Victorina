using System.IO;
using UnityEngine;


namespace Victorina
{
    internal class AvatarLoader : MonoBehaviour
    {
        private SpriteCreator _spriteCreator = new SpriteCreator();

        [SerializeField] private PlayerData _playerData;
        [SerializeField] private Sprite _defaultAvatarTexture;


        //private void Awake()
        //{
        //    _spriteCreator.SpriteComplete += OnSpriteComplete;
        //}

        //private void OnDisable()
        //{
        //    _spriteCreator.SpriteComplete -= OnSpriteComplete;  
        //}

        //private void OnSpriteComplete()
        //{
        //    _playerData.Avatar = _spriteCreator.SpriteForAvatar;
        //    _playerData.ScaleImageAvatarCoef = _spriteCreator.ScaleCoef;
        //}

        private void Start()
        {
            if (PlayerPrefs.HasKey("AvatarUrl"))
            {
                var path = PlayerPrefs.GetString("AvatarUrl");
                if (File.Exists(path))
                {
                    Texture2D texture = NativeGallery.LoadImageAtPath(path, -1);
                    var avatar = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    avatar.name = "SpriteForAvatar";

                    _playerData.Avatar = avatar;
                    _playerData.ScaleImageAvatarCoef = texture.width > texture.height ? (float)texture.width / texture.height : texture.height / (float)texture.width;

                }
                else
                {
                    _playerData.Avatar = _defaultAvatarTexture;
                }
            }
            else
                _playerData.Avatar = _defaultAvatarTexture;
        }

    }
}