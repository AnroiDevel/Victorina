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

        #endregion


        #region UnityMethods

        private void Awake()
        {
            Instance = this;
            _spriteCreator.SpriteComplete += OnSpriteCompete;
        }

        private void Start()
        {
            SetProfileAvatar();
        }

        private void SetDefaultAvatar()
        {
            _avatarImg.sprite = _playerData.Avatar;
            _playerData.Avatar = _defaultAvatarSprite;
        }

        private void OnEnable()
        {
            _spriteCreator.SpriteComplete += OnSpriteCompete;
            //_playerData.ReloadAvatar += OnReloadAvatar;

        }

        private void OnDisable()
        {
            _spriteCreator.SpriteComplete -= OnSpriteCompete;
            //_playerData.ReloadAvatar -= OnReloadAvatar;
        }

        #endregion


        #region Methods

        private void OnSpriteCompete()
        {
            _playerData.Avatar = _spriteCreator.SpriteForAvatar;
            _playerData.ScaleImageAvatarCoef = _spriteCreator.ScaleCoef;

            _loadLable.gameObject.SetActive(false);

            SetProfileAvatar();
        }

        private void SetProfileAvatar()
        {
            _avatarImg.sprite = _playerData.Avatar;
            _avatarImg.transform.localScale = Vector3.one;
            _avatarImg.transform.localScale *= _playerData.ScaleImageAvatarCoef;
        }

        private void OnReloadAvatar()
        {
            _avatarImg.sprite = _playerData.Avatar;
            _avatarImg.transform.localScale = Vector3.one * _playerData.ScaleImageAvatarCoef;
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

                    var avatar = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
                    var scale = texture.width > texture.height ? (float)texture.width / texture.height : texture.height / (float)texture.width;
                    avatar.name = "SpriteForAvatar";

                    _playerData.Avatar = avatar;
                    _playerData.ScaleImageAvatarCoef = scale;

                    //s_avatarImg.GetComponent<CanvasRenderer>().SetTexture(material.mainTexture);
                    _avatarImg.sprite = _playerData.Avatar;
                    _avatarImg.transform.localScale = Vector3.one * _playerData.ScaleImageAvatarCoef;

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