using UnityEngine;


namespace Victorina
{
    public class NotDestroy : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }

}