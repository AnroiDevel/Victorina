using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class NameCreator : MonoBehaviour
    {
        [SerializeField] private Text _name;
        [SerializeField] private PlayerData _playerData;

        public void CreateName()
        {
            _playerData.Name = _name.text;
        }
    }

}