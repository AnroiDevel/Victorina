using UnityEngine;
using UnityEngine.SceneManagement;


namespace Victorina
{
    internal class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }

        public void LoadGameScene(string scena)
        {
            SceneManager.LoadScene(scena);
        }

    }
}

