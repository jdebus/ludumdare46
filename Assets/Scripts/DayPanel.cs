using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayPanel : MonoBehaviour
{
    [SerializeField]
    Sprite[] numberSprites;

    [SerializeField]
    Image dayImage;

    private void Start()
    {
        GameManager.Instance.DayNight.OnDayStarted += DayNight_OnDayStarted;
    }

    private void DayNight_OnDayStarted(int obj)
    {
        dayImage.sprite = numberSprites[obj - 1];
    }
}
