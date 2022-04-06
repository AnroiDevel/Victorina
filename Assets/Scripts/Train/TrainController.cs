using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Victorina
{
    public class TrainController : MonoBehaviour
    {
        [SerializeField] PlayerData _playerData;

        private void Start()
        {
            _playerData.IsTrain = true;
        }
    }

}