using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class AvatarManager : MonoBehaviour
    {
        public static AvatarManager Instance;

        #region Fields

        [SerializeField] private PlayerData _playerData;
        [SerializeField] private Image _avatarImg;
        [SerializeField] private Sprite _defaultAvatarSprite;
        [SerializeField] private Text _loadLable;
        [SerializeField] private Renderer _avatarUIRenderer;

        private SpriteCreator _spriteCreator = new SpriteCreator();
        private Character _player;
        private GameData _gameData;

        #endregion


        #region UnityMethods

        private void Awake()
        {
            Instance = this;
            _spriteCreator.SpriteComplete += OnSpriteCompete;
        }

        private void Start()
        {
            _gameData = GameData.GetInstance();
            _player = _gameData.Player;
            SetProfileAvatar();
        }

        private void SetDefaultAvatar()
        {
            //_player.Avatar = _defaultAvatarSprite.texture;
        }

        private void OnEnable()
        {
            _spriteCreator.SpriteComplete += OnSpriteCompete;
        }

        private void OnDisable()
        {
            _spriteCreator.SpriteComplete -= OnSpriteCompete;
        }

        #endregion


        #region Methods

        private void OnSpriteCompete()
        {
            _player.ScaleAvatarCoef = _spriteCreator.ScaleCoef;

            _loadLable.gameObject.SetActive(false);

            SetProfileAvatar();
        }

        private void SetProfileAvatar()
        {
            var player = _gameData.Player;
            _avatarImg.transform.localScale = Vector3.one * player.ScaleAvatarCoef;
        }

        private void OnReloadAvatar()
        {
            //_avatarImg.texture = _player.Avatar;
            _avatarImg.transform.localScale = Vector3.one * _player.ScaleAvatarCoef;
        }

        public void PickImage(int maxSize)
        {
            NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
            {
                //Debug.Log("Image path: " + path);
                if (path != null)
                {
                    Texture2D texture = NativeGallery.LoadImageAtPath(path, -1);
                    if (texture == null)
                    {
                        Debug.Log("Couldn't load texture from " + path);
                        return;
                    }

                    //GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    //quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
                    ////quad.transform.forward = Camera.main.transform.forward;
                    //quad.transform.localScale = new Vector3(1f, texture.height / (float)texture.width, 1f);
                    //Material material = quad.GetComponent<Renderer>().material;
                    //material.shader = Shader.Find("UI/Default");
                    //material.mainTexture = texture;
                    //Destroy(quad, 5.0f);
                    var player = _gameData.Player;

                    var avatar = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    var scale = texture.width > texture.height ? (float)texture.width / texture.height : texture.height / (float)texture.width;
                    avatar.name = "SpriteForAvatar";

                    player.Avatar = avatar;
                    player.ScaleAvatarCoef = scale;

                    //s_avatarImg.GetComponent<CanvasRenderer>().SetTexture(material.mainTexture);
                    _avatarImg.sprite = avatar;
                    _avatarImg.transform.localScale = Vector3.one * player.ScaleAvatarCoef;

                    PlayerPrefs.SetString("AvatarUrl", path);


                    //if (!material.shader.isSupported) // happens when Standard shader is not included in the build
                    //    material.shader = Shader.Find("Legacy Shaders/Diffuse");
                    //else

                    //Destroy(texture, 5.0f);

                    //_loadLable.gameObject.SetActive(true);
                    //StartCoroutine(_spriteCreator.CreateSprite(path));

                }
                //_playerData.PathFileAvatar = path;


            });
        }

        #endregion    
    }
}