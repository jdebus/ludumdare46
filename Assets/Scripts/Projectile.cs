using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    float speed = 6;

    [SerializeField]
    int damage = 1;

    [SerializeField]
    float lifetime = 5;

    Rigidbody2D body;

    private void Start()
    {
        
        Destroy(gameObject, lifetime);
    }


    internal void Fire(Vector2 direction)
    {
        body = GetComponent<Rigidbody2D>();
        body.bodyType = RigidbodyType2D.Dynamic;
        body.velocity = direction.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody == null)
            return;

        if (collision.CompareTag("Nest"))
            return;

        if (collision.attachedRigidbody.CompareTag("Enemy"))
        {
            var enemy = collision.attachedRigidbody.GetComponent<Enemy>();
            enemy.TakeDamage(transform.position, damage);
            Destroy(gameObject);
        }

        if (collision.attachedRigidbody == null)
            Destroy(gameObject, 0.1f);
    }
}
