using System.Collections;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;



namespace Victorina
{
    public class AvatarManager : MonoBehaviour
    {
        public static AvatarManager Instance;

        [SerializeField] private PlayerData _playerData;

        [SerializeField] private GameObject _fileListPan;
        [SerializeField] private GameObject _filesContent;
        [SerializeField] private GameObject _filePrefab;

        [SerializeField] private Image _avatarImg;
        [SerializeField] private Image _avatarImgTest;


        [SerializeField] private Text _errorInfo;
        [SerializeField] private Image _testImage;

        private string _pathToUserAvatar;

        private DirectoryInfo _dirInfo;
        private FileInfo[] _files;
        [SerializeField] private FileScript[] _placeToImage;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            SetAvatar();
        }

        private void OnEnable()
        {
            LoadAvatarList();
        }


        public async void LoadAvatarList()
        {
            string[] dirs = {
               "/mnt/sdcard/Pictures",
               "/mnt/sdcard/Download",
               "/mnt/sdcard/DCIM"
            };

            string res = "Доступные пути\n";
            foreach (string s in dirs)
            {
                if (Directory.Exists(s))
                {
                    res += s + "\n";
                }
            }
            _errorInfo.text = res;



            //dir = Path.GetDirectoryName(dir);
            var dir = "/mnt/sdcard/Download";

            if (!Directory.Exists(dir))
            {
                dir = Directory.GetCurrentDirectory();
                if (!Directory.Exists(dir))
                {
                    _errorInfo.text = "Путь не найден";
                    return;
                }

            }

            _dirInfo = new DirectoryInfo(dir);

            _errorInfo.text = "Загрузка";

            await Task.Run(() => FileListCreated());

            StartCoroutine(LoadTextures());

        }

        private Task FileListCreated()
        {

            _files = new string[] { "*.png", "*.jpg" }.SelectMany(ext => _dirInfo.GetFiles(ext, SearchOption.AllDirectories)).ToArray();

            return Task.CompletedTask;
        }

        private IEnumerator LoadTextures()
        {
            var cnt = 0;
            var maxCount = _placeToImage.Length;
            var currentIndex = 0;
            while (cnt < _files.Length && currentIndex < maxCount)
            {
                FileInfo f = _files[cnt];

                //if (f.Length < 10000)
                //{
                //    cnt++;
                //    print(f.Name + "  " + f.Length);
                //    continue;
                //}

                var request = UnityWebRequestTexture.GetTexture("file://" + _files[cnt].FullName);

                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    _placeToImage[currentIndex].Index = cnt;
                    _placeToImage[currentIndex].Image.texture = DownloadHandlerTexture.GetContent(request);
                    _placeToImage[currentIndex].gameObject.SetActive(true);
                    currentIndex++;
                }
                else
                    _errorInfo.text = request.error;

                request.Dispose();
                if (cnt > 40) yield break;
                cnt++;

            }
            _errorInfo.text = "Загружено";
        }

        public void SelectAvatar(int index)
        {
            WWW www = new WWW("file://" + _files[index].FullName);
            _fileListPan.SetActive(false);

            Texture2D texture2D = www.texture;
            Sprite sprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));



            _playerData.Avatar = sprite;
            _avatarImg.sprite = _playerData.Avatar;
            _pathToUserAvatar = _files[index].FullName;
            _playerData.PathFileAvatar = _pathToUserAvatar;
            PlayerPrefs.SetString("AvatarUrl", _pathToUserAvatar);

            DestroyTempFiles();
        }

        public void SelectAvatar(string pathFile)
        {
            _errorInfo.text += "Установка аватара из " + pathFile;
            StartCoroutine(UpdateAvatar(pathFile));
        }

        private IEnumerator UpdateAvatar(string pathFile)
        {
            yield return new WaitForSeconds(0.1f);

            WWW www = new WWW("file://" + pathFile);
            Texture2D texture2D = www.texture;
            Sprite sprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
            _playerData.Avatar = sprite;
            _avatarImg.sprite = sprite;
            _pathToUserAvatar = pathFile;
            _playerData.PathFileAvatar = _pathToUserAvatar;
            PlayerPrefs.SetString("AvatarUrl", _pathToUserAvatar);

            _errorInfo.text += "Аватар установлен ";
            //_avatarImgTest.sprite = sprite;
        }

        public void SetAvatar()
        {
            _playerData.PathFileAvatar = PlayerPrefs.GetString("AvatarUrl");
            WWW www = new WWW("file://" + _playerData.PathFileAvatar);
            Texture2D texture2D = www.texture;
            Sprite sprite = Sprite.Create(texture2D, new Rect(0.0f, 0.0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
            _playerData.Avatar = sprite;
            _avatarImg.sprite = sprite;
        }

        public void DestroyTempFiles()
        {
            if (_placeToImage.Length > 0)
                foreach (var obj in _placeToImage)
                    obj.gameObject.SetActive(false);
        }


    }

}