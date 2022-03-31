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
        [SerializeField] private GameObject _fileList;
        [SerializeField] private RawImage _cameraImg;


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
            Debug.Log(Application.persistentDataPath + "/photo.png");

            //_image.texture = _playerData.Avatar.texture;

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

        public void SetActiveCamera(WebCamTexture cameraToUse)
        {
            if (_activeCameraTexture != null)
            {
                _activeCameraTexture.Stop();
            }

            _activeCameraTexture = cameraToUse;
            _activeCameraDevice = WebCamTexture.devices.FirstOrDefault(device =>
                device.name == cameraToUse.deviceName);

            _cameraImg.texture = _activeCameraTexture;
            //_cameraImg.material.mainTexture = _activeCameraTexture;

            if (_activeCameraTexture != null)
                _activeCameraTexture.Play();
        }

        public void LoadImageFromDir()
        {

            Application.OpenURL(Application.persistentDataPath);
            _files = _directory.GetFiles("*.jpg", SearchOption.TopDirectoryOnly);
        }


        public void SetAvatarFromCamera()
        {
            StartCoroutine(TakeSnapshot());

        }

        WaitForSeconds waitTime = new WaitForSeconds(0.1F);
        WaitForEndOfFrame frameEnd = new WaitForEndOfFrame();

        public IEnumerator TakeSnapshot()
        {
            yield return waitTime;
            yield return frameEnd;

            var width = (int)_cameraImg.rectTransform.rect.width;
            var height = (int)_cameraImg.rectTransform.rect.height;

            Texture2D texture = new Texture2D(width, height, TextureFormat.RGB24, true);
            texture.ReadPixels(new Rect((Screen.width - width) / 2, Screen.height - height, width, height), 0, 0);
            texture.LoadRawTextureData(texture.GetRawTextureData());
            texture.Apply();
            SendTexture(texture);

            // gameObject.renderer.material.mainTexture = TakeSnapshot;
        }

        private void SendTexture(Texture2D texture)
        {
            WriteFoto(texture);
            _cameraImg.transform.Rotate(new Vector3(0, 0, -90));
            _cameraImg.gameObject.SetActive(false);
            _activeCameraTexture.Stop();


        }

        private void WriteFoto(Texture2D texture)
        {
            byte[] bytes = texture.EncodeToPNG();
            string path = "/mnt/sdcard/Download" + "/foto.png";
            //path = Application.persistentDataPath + "/foto.png";
            File.WriteAllBytes(path, bytes);
            PlayerPrefs.SetString("AvatarUrl", path);
            _playerData.PathFileAvatar = path;
            _playerData.SetAvatar();
            AvatarManager.Instance.SelectAvatar(path);
            StartCoroutine(DeactiveFileList());
        }

        private IEnumerator DeactiveFileList()
        {
            yield return new WaitForSeconds(1);
            _fileList.SetActive(false);
        }

        public void FotoFromCamera()
        {

            //_isActiveCamera = true;

            //if (_isActiveCamera)
            //{


            _cameraImg.gameObject.SetActive(true);

            SetActiveCamera(_frontCameraTexture);

            _cameraImg.transform.Rotate(new Vector3(0, 0, 90));

            //}
            //else
            //{
            //    if (_activeCameraTexture.isPlaying)
            //    {
            //        Texture2D photo = new Texture2D(_frontCameraTexture.width, _frontCameraTexture.height);
            //        photo.SetPixels(_frontCameraTexture.GetPixels());
            //        photo.Apply();
            //        _avatar.texture = photo;

            //        _activeCameraTexture.Stop();

            //        byte[] bytes = photo.EncodeToPNG();
            //        var path = Application.persistentDataPath;

            //        _playerData.PathFileAvatar = path;
            //        Sprite sprite = Sprite.Create(photo, new Rect(0.0f, 0.0f, photo.width, photo.height), new Vector2(0.5f, 0.5f));
            //        _playerData.Avatar = sprite;
            //        File.WriteAllBytes(path + "/photo.png", bytes);

            //    }
            //}

            //_isActiveCamera = false;
        }
    }
}