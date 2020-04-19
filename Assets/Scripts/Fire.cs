using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Fire : MonoBehaviour
{
    public float lifeTime = 60;

    public Sprite offSprite;

    CircleCollider2D cc;
    [SerializeField]
    AudioClip startSound;

    public Light2D light;
    private void Start()
    {
        cc = GetComponent<CircleCollider2D>();
        if (startSound != null)
            AudioSource.PlayClipAtPoint(startSound, Camera.main.transform.position);
    }

    void Update()
    {
        if (lifeTime < 0)
            return;

        var nestDistance = (GameManager.Instance.NestPosition - (Vector2)transform.position).magnitude;
        if(nestDistance < cc.radius)
        {
            GameManager.Instance.Nest.Baby.Cold = 0;
        }

        lifeTime -= Time.deltaTime;
        if(lifeTime <= 0)
        {
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().sprite = offSprite;
            var particles = GetComponentsInChildren<ParticleSystem>();
            foreach (var ps in particles)
            {
                var emission = ps.emission;
                emission.enabled = false;
            }
         
            if(light != null)
                light.enabled = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Nest") && GameManager.Instance.Nest.State == NestState.BabyInside)
        {
            GameManager.Instance.Nest.Baby.Cold = 0;
        }
    }
}
