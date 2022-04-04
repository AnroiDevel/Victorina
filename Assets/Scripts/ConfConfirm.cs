using UnityEngine;


namespace Victorina
{
    public class ConfConfirm : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;
        public void Confirm()
        {
            _playerData.IsNewVersionApp = false;
        }
    }

}