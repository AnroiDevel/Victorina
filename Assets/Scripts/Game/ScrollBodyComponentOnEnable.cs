using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScrollBodyComponentOnEnable : MonoBehaviour
{
    private void OnEnable()
    {
        var rect = GetComponent<RectTransform>();
        rect.anchorMin = Vector3.zero;

        transform.localPosition = Vector3.zero;
        rect.position = Vector3.zero;
    }
}
