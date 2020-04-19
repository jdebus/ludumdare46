using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Sprite fullHeart;
    public Sprite emptyHeart;

    [SerializeField]
    Image[] playerImages;

    [SerializeField]
    Image[] nestImages;


    private void Start()
    {
        GameManager.Instance.Player.PlayerHealthChanged += Player_HealthChanged;
        GameManager.Instance.Nest.Egg.HealthChanged += Egg_HealthChanged;
        GameManager.Instance.Nest.Baby.HealthChanged += Egg_HealthChanged;
    }

    private void Player_HealthChanged(int health)
    {
        for (int i = 0; i < playerImages.Length; i++)
        {
            playerImages[i].sprite = health > i ? fullHeart : emptyHeart;
        }
    }

    private void Egg_HealthChanged(int health)
    {
        for (int i = 0; i < nestImages.Length; i++)
        {
            nestImages[i].sprite = health > i ? fullHeart : emptyHeart;
        }
    }
}
