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

        private SpriteCreator _spriteCreator = new SpriteCreator();

        #endregion


        #region UnityMethods

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            //SetAvatar();

        }

        private void OnEnable()
        {
            //LoadAvatarList();
            _spriteCreator.SpriteComplete += OnSpriteCompete;
        }

        private void OnDisable()
        {
            _spriteCreator.SpriteComplete -= OnSpriteCompete;
        }

        #endregion


        #region Methods
        private void OnSpriteCompete(Sprite spr)
        {
            _playerData.Avatar = spr;
            _avatarImg.sprite = spr;
        }

        public void PickImage(int maxSize)
        {
            NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
            {
                Debug.Log("Image path: " + path);
                if (path != null)
                {
                    Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);
                    if (texture == null)
                    {
                        Debug.Log("Couldn't load texture from " + path);
                        return;
                    }

                    GameObject quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
                    quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 10002.5f;
                    quad.transform.forward = Camera.main.transform.forward;
                    quad.transform.localScale = new Vector3(1f, texture.height / (float)texture.width, 1f);

                    Material material = quad.GetComponent<Renderer>().material;
                    if (!material.shader.isSupported) // happens when Standard shader is not included in the build
                        material.shader = Shader.Find("Legacy Shaders/Diffuse");

                    material.mainTexture = texture;

                    Destroy(quad, 5.0f);
                    Destroy(texture, 5.0f);
                }
                _playerData.PathFileAvatar = path;
                PlayerPrefs.SetString("AvatarUrl", path);

                StartCoroutine(_spriteCreator.CreateSprite(path));

            });
        }

        #endregion    
    }
}