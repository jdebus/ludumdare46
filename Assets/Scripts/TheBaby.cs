using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheBaby : MonoBehaviour
{
    [SerializeField]
    int health = 4;

    public event Action BabyDied;

    public event Action<int> HealthChanged;

    public float Hunger = 0;
    public float Cold = 0;

    public float maxColdTime = 10;
    public float maxHungerTime = 15;

    float coldDamageTime = 5;
    float hungerDamageTime = 7;

    float coldTimer = 0;
    float hungerTimer = 0;

    public Bar hungerBar;
    public Bar coldBar;
    public BabyText babyText;

    public AudioClip eatSound;

    Animator animator;

    void PlayClip(AudioClip clip)
    {
        if (clip != null)
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
    }

    public void Activate()
    {
        health = 4;
        HealthChanged?.Invoke(health);
        animator.SetTrigger("activate");
    }

    public void ActivationCompleted()
    {
        GameManager.Instance.Nest.babyCam.Priority = 0;
        GameManager.Instance.TemporaryPaused = false;
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        GameManager.Instance.OnWin += () => enabled = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        babyText.ShowText(BabyTextValue.Ouch);
        HealthChanged?.Invoke(health);
        if (health <= 0)
            Die();
    }

    private void Update()
    {
        Hunger += Time.deltaTime;

        if (GameManager.Instance.DayNight.IsNight)
            Cold += Time.deltaTime;
        else
            Cold = 0;


        if(Hunger > maxHungerTime * 0.75f)
            babyText.ShowText(BabyTextValue.Hungry);

        if (Cold > maxColdTime * 0.5f)
            babyText.ShowText(BabyTextValue.Cold);

        if(Cold >= maxColdTime)
        {
            Cold = maxColdTime;
            coldTimer -= Time.deltaTime;
            if(coldTimer < 0)
            {
                TakeDamage(1);
                coldTimer = coldDamageTime;
            }
        }

        if(Hunger >= maxHungerTime)
        {
            Hunger = maxHungerTime;
            hungerTimer -= Time.deltaTime;
            if(hungerTimer < 0)
            {
                TakeDamage(1);
                hungerTimer = hungerDamageTime;
            }
        }

        coldBar.percentage = Cold / maxColdTime;
        hungerBar.percentage = Hunger / maxHungerTime;
    }

    void Die()
    {
        if(!GameManager.Instance.IsGameOver)
            BabyDied?.Invoke();
    }

    internal void MouseDown()
    {
        if (InventoryUI.Instance.SelectedItem != null)
        {
            var item = InventoryUI.Instance.SelectedItem;

            if (item.type == ItemType.Food)
            {
                Debug.Log("Baby eats food");
                Hunger = 0;
                PlayClip(eatSound);
                if(health < 4)
                    health++;
                HealthChanged?.Invoke(health);
                InventoryUI.Instance.ItemUsed(item);
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("baby stays in trigger: " + collision.name);

    }
}
