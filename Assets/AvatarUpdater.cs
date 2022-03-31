using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class AvatarUpdater : MonoBehaviour
    {
        [SerializeField] private PlayerData _playerData;
        private Image _avatar;

        private void Start()
        {
            _avatar = GetComponent<Image>();
        }

        //private void OnEnable()
        //{
        //    StartCoroutine(AvatarUpdate());
        //}

        //private IEnumerator AvatarUpdate()
        //{
        //    yield return new WaitForSeconds(2);
        //    _avatar.sprite = _playerData.Avatar;
        //}
    }

}