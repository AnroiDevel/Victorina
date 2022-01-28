using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class QuestionLoader : MonoBehaviour
    {
        [SerializeField] private Text _question;
        [SerializeField] private Text[] _answers; 


        public void LoadOneQuestion() 
        {
            StartCoroutine(GetQuestion());
        }

        private IEnumerator GetQuestion()
        {
            throw new NotImplementedException();
        }
    }

}