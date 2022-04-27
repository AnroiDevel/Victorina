using System.Collections;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


namespace Victorina
{
    public class LoadAvatar : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;
        [SerializeField] private RawImage _cameraImg;


        public RawImage _image;
        // Device cameras
        //WebCamDevice _frontCameraDevice;
        //WebCamDevice _backCameraDevice;
        //WebCamDevice _activeCameraDevice;

        //WebCamTexture _frontCameraTexture;
        //WebCamTexture _backCameraTexture;
        //WebCamTexture _activeCameraTexture;

        [SerializeField] private RawImage _avatar;
        private Sprite _sprite;

        private DirectoryInfo _directory = new DirectoryInfo("C:");
        private FileInfo[] _files;
        private bool _isActiveCamera;

        private void Start()
        {
            //Debug.Log(Application.persistentDataPath + "/photo.png");

            ////_image.texture = _playerData.Avatar.texture;

            //if (WebCamTexture.devices.Length == 0)
            //{
            //    Debug.Log("No devices cameras found");
            //    return;
            //}

            //_frontCameraDevice = WebCamTexture.devices.Last();
            //_backCameraDevice = WebCamTexture.devices.First();

            //_frontCameraTexture = new WebCamTexture(_frontCameraDevice.name);
            //_backCameraTexture = new WebCamTexture(_backCameraDevice.name);

            // Set camera filter modes for a smoother looking image
            //_frontCameraTexture.filterMode = FilterMode.Trilinear;
            //_backCameraTexture.filterMode = FilterMode.Trilinear;

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
            var _pathForUserImage = "/mnt/sdcard/Download" + "/foto.png";
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
    }
}