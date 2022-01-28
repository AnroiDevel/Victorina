using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Victorina
{
    public class DoubleClick : MonoBehaviour
    {
        //���������� ����� ����� ������� �����
        public float _timePassed = 0.0f;
        //��� ��������� ������ ����
        public bool clicked = false;
        public bool doubleClick = false;
        private Image _image;

        private void Start()
        {
            _image = GetComponent<Image>();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _image.raycastTarget = false;
                _timePassed = 0.0f; 
            }

            _timePassed += Time.deltaTime;

            if (_timePassed > 0.3f)
                _image.raycastTarget = true;

        }

    }

}
