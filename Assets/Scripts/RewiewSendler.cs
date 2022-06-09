using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


namespace Victorina
{
    public class RewiewSendler : MonoBehaviour
    {
        private const string Uri = "http://a0669097.xsph.ru/Victorina/Question/SetRewiev.php";
        [SerializeField] private Text _textRewiev;

        public void SendRewievText()
        {
            StartCoroutine(SendRewievText(_textRewiev.text));
        }

        private IEnumerator SendRewievText(string text)
        {
            var form = new WWWForm();
            form.AddField("autor", GameData.GetInstance().Player.Name);
            form.AddField("textRewiv", text);
            using UnityWebRequest request = UnityWebRequest.Post(Uri, form);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
                Debug.Log("Ошибка");
        }
    }

}