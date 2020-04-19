using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverText : MonoBehaviour
{
    public Sprite eatSprite;
    public Sprite feedSprite;
    public Sprite placeSprite;
    public Sprite makeFireSprite;


    [SerializeField]
    SpriteRenderer textRenderer;
    

    public void Hide()
    {
        textRenderer.enabled = false;
    }

    public void Show()
    {
        textRenderer.enabled = true;
    }

    public void SetSprite(HoverTextText text)
    {
        switch (text)
        {
            case HoverTextText.Eat:
                textRenderer.sprite = eatSprite;
                break;
            case HoverTextText.Feed:
                textRenderer.sprite = feedSprite;
                break;
            case HoverTextText.Place:
                textRenderer.sprite = placeSprite;
                break;
            case HoverTextText.MakeFire:
                textRenderer.sprite = makeFireSprite;
                break;
            default:
                textRenderer.sprite = null;
                break;
        }
    }

    public enum HoverTextText
    {
        Eat, Feed, Place, MakeFire
    }
}
