using System.Collections;
using UnityEngine;


namespace Victorina
{
    public class Logo : MonoBehaviour
    {
        [SerializeField] private GameObject _firstText;
        [SerializeField] private GameObject _logoText;
        [SerializeField] private float _time;

        private void Start()
        {
            StartCoroutine(LogoPlay());
        }

        private IEnumerator LogoPlay()
        {
            yield return new WaitForSeconds(_time);
            //_firstText.SetActive(true);
            //yield return new WaitForSeconds(_time);
            //_firstText.SetActive(false);
            //_logoText.SetActive(true);
            //yield return new WaitForSeconds(1);

            GetComponent<SceneLoader>().LoadGameScene("Autorization");

        }

    }

}