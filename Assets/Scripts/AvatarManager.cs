using System.Collections;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;



namespace Victorina
{
    public class AvatarManager : MonoBehaviour
    {
        public static AvatarManager Instance;

        [SerializeField] private GameObject _fileListPan;
        [SerializeField] private GameObject _filesContent;
        [SerializeField] private GameObject _filePrefab;

        [SerializeField] private RawImage _avatarImg;

        private string _pathToUserAvatar;

        private DirectoryInfo _dirInfo;
        private FileInfo[] _files;
        private GameObject[] _spawnsGameObjects;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if(PlayerPrefs.HasKey("AvatarUrl"))
                _pathToUserAvatar = PlayerPrefs.GetString("AvatarUrl");
            WWW www = new WWW(_pathToUserAvatar);
            _avatarImg.texture = www.texture;
        }

        public void LoadAvatarList()
        {
            var dir = Directory.GetCurrentDirectory();
            //dir = Path.GetDirectoryName(dir);
            if (File.Exists(dir)) return;

            _dirInfo = new DirectoryInfo(dir);
            _fileListPan.SetActive(true);
            FileListCreated();
        }

        private void FileListCreated()
        {
            _files = _dirInfo.GetFiles("*.png", SearchOption.AllDirectories);

            _spawnsGameObjects = new GameObject[_files.Length];

            for (int i = 0; i < _files.Length; i++)
            {
                StartCoroutine(FileCreated(i));
                if (i > 10) return;
            }
        }

        private IEnumerator FileCreated(int i)
        {
            OneFileCreated(i);
            yield return null;
        }

        private void OneFileCreated(int i)
        {
            FileInfo f = _files[i];
            FileScript file = Instantiate(_filePrefab, _filesContent.transform).GetComponent<FileScript>();
            file.FileNameText.text = f.Name;
            file.Index = i;
            _spawnsGameObjects[i] = file.gameObject;

            WWW www = new WWW("file://" + _files[i].FullName);
            file.Image.texture = www.texture;
        }

        public void SelectAvatar(int index)
        {
            WWW www = new WWW("file://" + _files[index].FullName);
            _avatarImg.texture = www.texture;
            _fileListPan.SetActive(false);

            _pathToUserAvatar = www.url;
            PlayerPrefs.SetString("AvatarUrl",_pathToUserAvatar);

            DestroyTempFiles();
        }

        public void DestroyTempFiles()
        {
            if (_spawnsGameObjects.Length > 0)
                foreach (GameObject obj in _spawnsGameObjects)
                    Destroy(obj);
        }
    }

}