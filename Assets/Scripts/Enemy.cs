using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    Rigidbody2D body;
    Animator animator;

    [SerializeField]
    float speed = 2;
    [SerializeField]
    float aggroSpeed = 3;

    [SerializeField]
    int health = 3;

    [SerializeField]
    float aggroDistance = 2;

    [SerializeField]
    float attackRange = 0.5f;

    [SerializeField]
    float attackDelay = 2;

    [SerializeField]
    int damage = 1;

    [SerializeField]
    bool atNest = false;

    [SerializeField]
    Transform visual;

    [SerializeField]
    float attackTimer = 0;

    [SerializeField]
    GameObject[] droppableItems;

    [SerializeField, Range(0,1)]
    float dropChance = 0.5f;

    bool hasPlayerAggro = false;

    [SerializeField]
    AudioClip[] hitSounds;

    SpriteRenderer sr;


    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sr = visual.GetComponent<SpriteRenderer>();
        GameManager.Instance.Enemies.Add(this);
    }

    void Update()
    {
        body.simulated = !GameManager.Instance.TemporaryPaused && !GameManager.Instance.IsGameOver && health > 0;


        if (health <= 0 || GameManager.Instance.TemporaryPaused || GameManager.Instance.IsGameOver)
            return;

        Vector3 distanceToPlayer = GameManager.Instance.PlayerPosition - (Vector2)transform.position;
        hasPlayerAggro = distanceToPlayer.magnitude < aggroDistance;
        if (hasPlayerAggro && GameManager.Instance.Player.IsAlive)
        {
            // move to player
            body.velocity = distanceToPlayer.normalized * aggroSpeed;
            if (distanceToPlayer.magnitude < attackRange)
                DoAttack();
        }
        else
        {
            if (atNest)
            {
                DoAttack();
            }
            else
            {
                MoveToNest();
            }
        }

        attackTimer -= Time.deltaTime;
        animator.SetFloat("velocity", body.velocity.normalized.magnitude);

        //if (body.velocity.x > 0)
        //    visual.rotation = Quaternion.Euler(0, 180, 0);
        //else if (body.velocity.x < 0)
        //    visual.rotation = Quaternion.Euler(0, 0, 0);

        //if (body.velocity.x > 0)
        //    visual.localScale = new Vector3(-1, 1, 1);
        //else if (body.velocity.x < 0)
        //    visual.localScale = new Vector3(1, 1, 1);

        if (body.velocity.x > 0)
            sr.flipX = true;
        else if (body.velocity.x < 0)
            sr.flipX = false;
    }

    void DoAttack()
    {
        if (attackTimer <= 0)
        {
            animator.SetTrigger("attack");
            attackTimer = attackDelay;
            body.velocity = Vector3.zero;
        }
    }

    private void AttackPlayer()
    {
        if (atNest)
        {
            GameManager.Instance.Nest.TakeDamage(damage);
        }
        else if(hasPlayerAggro)
        {
            GameManager.Instance.Player.TakeDamage(damage);
        }

        
        attackTimer = attackDelay;
    }

    public void Die()
    {
        body.simulated = false;
        animator.SetTrigger("die");
        Destroy(gameObject, .7f);
    }

    private void OnDestroy()
    {
        GameManager.Instance.Enemies.Remove(this);
        DropItem();
    }

    void DropItem()
    {
        float thresh = 100 * dropChance;
        bool drop = UnityEngine.Random.Range(0, 101) <= thresh;
        if(drop)
            Instantiate(droppableItems[UnityEngine.Random.Range(0, droppableItems.Length)], transform.position, Quaternion.identity);
    }
 
    void AttackNest()
    {
        body.velocity = Vector2.zero;
    }

    public void TakeDamage(Vector2 position, int damage)
    {
        health -= damage;
        PlayHitSound();
        animator.SetTrigger("hit");
        if (health <= 0)
            Die();
    }

    void PlayHitSound()
    {
        AudioSource.PlayClipAtPoint(hitSounds[UnityEngine.Random.Range(0, hitSounds.Length)], Camera.main.transform.position + new Vector3(0, 0, 2));
    }

    void MoveToNest()
    {
        var direction = GameManager.Instance.NestPosition - (Vector2)transform.position;
        body.velocity = direction.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Nest"))
            atNest = true;

        if (collision.CompareTag("Item"))
        {
            var trap = collision.GetComponent<Trap>();
            if(trap != null && trap.isPlaced)
            {
                trap.isPlaced = false;
                trap.Snap();
                Die();
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Nest"))
            atNest = false;
    }

    private void OnDrawGizmos()
    {
        if(body != null)
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)body.velocity);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, aggroDistance);


    }




}
