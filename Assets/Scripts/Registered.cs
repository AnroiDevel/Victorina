using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


namespace Victorina
{
    public class Registered : MonoBehaviour
    {
        #region Fields

        [SerializeField] InputField _name;
        [SerializeField] InputField _passwd;

        private const string _urlIndex = "https://coxcombic-eliminato.000webhostapp.com/Test/";
        private WWWForm _form;

        #endregion


        #region Methods

        public void PostText() => StartCoroutine(Post(_urlIndex,
        (string error) => Debug.Log("Ошибка: " + error),
        (string text) => Debug.Log("Текст отправлен" + text)));

        private IEnumerator Post(string url, Action<string> onError, Action<string> onSucces)
        {
            _form = new WWWForm();
            var str = _name.text;
            _form.AddField("regName", str);

            using UnityWebRequest unityWebRequest = UnityWebRequest.Post(url, _form);
            yield return unityWebRequest.SendWebRequest();
            if (unityWebRequest.result == UnityWebRequest.Result.ConnectionError || unityWebRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                onError(unityWebRequest.error);
                print("Ошибка отправки");
            }
            else
            {
                onSucces(unityWebRequest.downloadHandler.text);
                print("Отправлено");
            }
        }

        #endregion  
    }
}