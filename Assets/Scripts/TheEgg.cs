using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheEgg : MonoBehaviour
{
    [SerializeField]
    int health = 4;

    public event Action TheEggDied;

    public event Action<int> HealthChanged;

    float animationTimer = 0;
    bool broken = false;

    private void Start()
    {
        HealthChanged?.Invoke(health);
        Debug.Log("Egg started: " + health);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Egg takes damage: " + damage + " health: " + health);
        HealthChanged?.Invoke(health);
        if (health <= 0)
            Die();
    }

    private void Update()
    {
        if (broken)
            return;

        animationTimer -= Time.deltaTime;

        if(animationTimer < 0)
        {
            int idleAnim = UnityEngine.Random.Range(1, 2 + 1);
            string triggerName = "idle_" + idleAnim.ToString();
            GetComponent<Animator>().SetTrigger(triggerName);
            animationTimer = UnityEngine.Random.Range(1, 4);
        }
    }

    public void Break()
    {
        broken = true;
        //GameManager.Instance.mainCam.Follow = this.transform;
        GetComponent<Animator>().SetTrigger("break");
    }

    public void EggBroken()
    {
        GameManager.Instance.Nest.ActivateBaby();
        //GameManager.Instance.mainCam.Follow = GameManager.Instance.Player.transform;
    }

    void Die()
    {
        if(!GameManager.Instance.IsGameOver)
            TheEggDied?.Invoke();
    }

}
