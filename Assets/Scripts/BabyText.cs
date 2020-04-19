using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyText : MonoBehaviour
{
    public GameObject visuals;

    public SpriteRenderer coldSprite, hungrySprite, ouchSprite;

    public float duration = 3;


    bool visible = false;
    float visibleTimer = 0;

    Vector3 targetPosition;

    public float distanceFromPlayer = 6;

    private void Start()
    {
        targetPosition = transform.position;
        visuals.SetActive(false);
    }

    public void ShowText(BabyTextValue value)
    {
        coldSprite.enabled = hungrySprite.enabled = ouchSprite.enabled = false;
        switch (value)
        {
            case BabyTextValue.Cold:
                coldSprite.enabled = true;
                break;
            case BabyTextValue.Hungry:
                hungrySprite.enabled = true;
                break;
            case BabyTextValue.Ouch:
                ouchSprite.enabled = true;
                break;
            default:
                break;
        }

        visuals.SetActive(true);
        visible = true;
        visibleTimer = duration;
    }

    private void Update()
    {
        if (!visible)
            return;

        transform.position = targetPosition;
        var directionToPlayer = GameManager.Instance.PlayerPosition - (Vector2)targetPosition;

        if (directionToPlayer.magnitude > distanceFromPlayer)
        {
            transform.position = GameManager.Instance.PlayerPosition - directionToPlayer.normalized * distanceFromPlayer;
        }

        visibleTimer -= Time.deltaTime;
        if(visibleTimer < 0)
        {
            visuals.SetActive(false);
            visible = false;
        }
    }
}

public enum BabyTextValue
{
    Cold, Hungry, Ouch
}
