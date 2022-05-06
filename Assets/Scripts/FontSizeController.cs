using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace Victorina
{
    public class FontSizeController : MonoBehaviour
    {
        [SerializeField] private Text _presetText;
        [SerializeField] private Text[] _postsetText;

        private void Start()
        {
            StartCoroutine(SetOptimalFontSize());
        }

        private IEnumerator SetOptimalFontSize()
        {
            yield return new WaitForEndOfFrame();
            var fontSize = _presetText.cachedTextGenerator.fontSizeUsedForBestFit;

            foreach (var text in _postsetText)
            {
                text.fontSize = fontSize;
                text.resizeTextMaxSize = fontSize;
            }
        }
    }
}