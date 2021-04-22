using UnityEngine;
using UnityEngine.SceneManagement;


namespace Victorina
{
    internal class SceneLoader : MonoBehaviour
    {
        public void LoadGameScene(string scena)
        {
            SceneManager.LoadScene(scena);
        }

    }
}

