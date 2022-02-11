using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


namespace Victorina
{
    public class LoadAvatar : MonoBehaviour
    {
        public RawImage _image;
        // Device cameras
        WebCamDevice _frontCameraDevice;
        WebCamDevice _backCameraDevice;
        WebCamDevice _activeCameraDevice;

        WebCamTexture _frontCameraTexture;
        WebCamTexture _backCameraTexture;
        WebCamTexture _activeCameraTexture;

        [SerializeField] private RawImage _avatar;
        private Sprite _sprite;

        private DirectoryInfo _directory = new DirectoryInfo("C:");
        private FileInfo[] _files;
        private bool _isActiveCamera;

        private void Start()
        {
            //_avatar = GetComponentInParent<RawImage>();
            LoadImageAvatar();

            if (WebCamTexture.devices.Length == 0)
            {
                Debug.Log("No devices cameras found");
                return;
            }

            _frontCameraDevice = WebCamTexture.devices.Last();
            _backCameraDevice = WebCamTexture.devices.First();

            _frontCameraTexture = new WebCamTexture(_frontCameraDevice.name);
            _backCameraTexture = new WebCamTexture(_backCameraDevice.name);

            // Set camera filter modes for a smoother looking image
            _frontCameraTexture.filterMode = FilterMode.Trilinear;
            _backCameraTexture.filterMode = FilterMode.Trilinear;

            // Set the camera to use by default
            /*
            devices = WebCamTexture.devices;
            frontCameraDevice = WebCamTexture.devices.Last();
            backCameraDevice = WebCamTexture.devices.First();
            wct = new WebCamTexture();
            if (devices.Length > 0)
            {
                wct.deviceName = devices[0].name;
                Debug.Log("Device 0");
            }
            rawimage.texture = wct;
            rawimage.material.mainTexture = wct;
            wct.Play();
            */
            // LoadSprite();

        }

        private void LoadImageAvatar()
        {
            var _pathForUserImage = Application.persistentDataPath + "/photo.png";
            if (File.Exists(_pathForUserImage))
            {
                var imageBytes = File.ReadAllBytes(_pathForUserImage);
                var firstLeftCC = new Texture2D(10, 10);
                firstLeftCC.LoadImage(imageBytes);
                _avatar.texture = firstLeftCC;
            }
            else
            {
                if (PlayerPrefs.HasKey("AvatarUrl"))
                    _pathForUserImage = PlayerPrefs.GetString("AvatarUrl");
                else return;
                WWW www = new WWW(_pathForUserImage);
                _avatar.texture = www.texture;


            }
            if (_avatar.texture != null)
                _avatar.gameObject.SetActive(true);
        }

        public void SetActiveCamera(WebCamTexture cameraToUse)
        {
            if (_activeCameraTexture != null)
            {
                _activeCameraTexture.Stop();
            }

            _activeCameraTexture = cameraToUse;
            _activeCameraDevice = WebCamTexture.devices.FirstOrDefault(device =>
                device.name == cameraToUse.deviceName);

            _image.texture = _activeCameraTexture;
            _image.material.mainTexture = _activeCameraTexture;

            _activeCameraTexture.Play();
        }

        public void LoadImageFromDir()
        {

            Application.OpenURL(Application.persistentDataPath);
            _files = _directory.GetFiles("*.jpg", SearchOption.TopDirectoryOnly);
        }


        public void FotoFromCamera()
        {

            _isActiveCamera = !_isActiveCamera;
            _image.gameObject.SetActive(_isActiveCamera);

#if !UNITY_EDITOR
            if (_isActiveCamera)
            {
                SetActiveCamera(_frontCameraTexture);
            }
            else
            {
                if (_activeCameraTexture.isPlaying)
                {
                    Texture2D photo = new Texture2D(_frontCameraTexture.width, _frontCameraTexture.height);
                    photo.SetPixels(_frontCameraTexture.GetPixels());
                    photo.Apply();
                    _avatar.texture = photo;

                    _activeCameraTexture.Stop();

                    byte[] bytes = photo.EncodeToPNG();
                    var path = Application.persistentDataPath;

                    File.WriteAllBytes(path + "/photo.png", bytes);
                }
            }
#endif
        }
    }
}