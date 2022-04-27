using UnityEngine;


namespace Victorina
{
    public class TrainController : MonoBehaviour
    {
        [SerializeField] PlayerData _playerData;

        private void Awake()
        {
            _playerData.IsTrain = true;
        }
    }

}