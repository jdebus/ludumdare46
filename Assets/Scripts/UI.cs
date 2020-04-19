using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI : MonoBehaviour
{
    Animator animator;

    [SerializeField]
    TMPro.TextMeshProUGUI dayText;


    [SerializeField]
    TMPro.TextMeshProUGUI gameOverText;

    private void Start()
    {
        animator = GetComponent<Animator>();

        GameManager.Instance.Nest.Egg.TheEggDied += () => gameOverText.text = "Your baby died! It wasn't even born..";
        GameManager.Instance.Nest.Baby.BabyDied += () => gameOverText.text = "Your baby died! Who is the real monster!?";
        GameManager.Instance.Player.PlayerDied += () => gameOverText.text = "You died! You Baby is defenseless";

        GameManager.Instance.OnGameOver += Instance_OnGameOver;
        GameManager.Instance.OnWin += Instance_OnWin;

        GameManager.Instance.DayNight.OnDayStarted += DayNight_OnDayStarted;
    }

    private void Instance_OnWin()
    {
        animator.SetTrigger("win");
    }

    private void DayNight_OnDayStarted(int obj)
    {
        dayText.text = obj.ToString();
        animator.SetTrigger("next_day");
    }

    private void Instance_OnGameOver()
    {
        animator.SetTrigger("game_over");
    }
}
