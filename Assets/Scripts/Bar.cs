using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bar : MonoBehaviour
{
    public Image barImage;


    [Range(0,1)]
    public float percentage;

    private void OnValidate()
    {
        barImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 200 * percentage);
    }

    private void Update()
    {
        barImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 200 * percentage);
    }

}
