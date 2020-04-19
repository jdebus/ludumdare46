using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public int Health { get; set; } = 3;

    [SerializeField]
    float speed = 2f;

    [SerializeField]
    Transform projectileSpawnPoint;

    [SerializeField]
    Projectile projectilePrefab;

    Rigidbody2D body;
    Animator animator;

    [SerializeField]
    bool atHome = false;

    public event Action<int> PlayerHealthChanged;
    public event Action PlayerDied;

    public bool IsAlive => Health > 0;


    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        PlayerHealthChanged?.Invoke(Health);

        GameManager.Instance.OnWin += () => enabled = false;
    }

    void Update()
    {
        body.simulated = !GameManager.Instance.TemporaryPaused && !GameManager.Instance.IsGameOver;
        if(!body.simulated)
        {
            animator.SetFloat("speed", 0);
            body.velocity = Vector2.zero;
            return;
        }


        if (!IsAlive)
            return;

        Vector2 input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        body.velocity = input * speed;

        animator.SetFloat("speed", input.magnitude);

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            animator.SetTrigger("throw");

        CheckFlipping();
    }

    internal void Disable()
    {
        enabled = false;
        body.velocity = Vector2.zero;
    }

    void CheckFlipping()
    {
        if (body.velocity.x > 0.01f)
            transform.rotation = Quaternion.Euler(0, 180, 0);
        else if (body.velocity.x < -0.01f)
            transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Nest"))
        {
            atHome = true;
        }

        if(collision.CompareTag("Item") && collision.gameObject.activeInHierarchy && InventoryUI.Instance.SelectedItem == null)
        {
            var item = collision.GetComponent<Item>();
            if (item != null && item.canBePickedUp)
            {
                if (InventoryUI.Instance.PickupItem(item))
                {
                    Debug.Log("picked up " + collision.name);
                    collision.gameObject.SetActive(false);
                }
            }
                
        }
    }

    internal void TakeDamage(int v)
    {
        if (Health > 0)
        {
            Health -= v;
            PlayerHealthChanged?.Invoke(Health);
            if(Health <= 0)
            {
                Die();
            }
        }
    }

    public void OnDieAnimationComplete()
    {
        animator.enabled = false;
    }

    public void Die()
    {
        animator.SetTrigger("die");
        body.velocity = Vector2.zero;
        enabled = false;
        body.bodyType = RigidbodyType2D.Kinematic;
        
        if(!GameManager.Instance.IsGameOver)
            PlayerDied?.Invoke();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Nest"))
        {
            atHome = false;
        }
    }

    public void ThrowSomething()
    {
        if (GameManager.Instance.TemporaryPaused)
            return;

        Debug.Log("Throw");
        var projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

        var mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var direction = mouseWorld - transform.position;

        //projectile.Fire(body.velocity - (Vector2)transform.right);
        projectile.Fire((Vector2)direction);
    }

    public void OnMouseDown()
    {
        if (GameManager.Instance.TemporaryPaused)
            return;

        if (InventoryUI.Instance.SelectedItem != null)
        {
            var item = InventoryUI.Instance.SelectedItem;

            if(item.type == ItemType.Food && Health < 4)
            {
                Debug.Log("Player eats food");
                Health++;
                GetComponent<PlayerAudio>().OnEatSound();
                PlayerHealthChanged?.Invoke(Health);
                InventoryUI.Instance.ItemUsed(item);
            }
        }
    }
}
