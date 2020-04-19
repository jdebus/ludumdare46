using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BabyGrownUp : MonoBehaviour
{
    Animator animator;

    bool moving;

    private void Start()
    {
        Move();
    }

    public void Move()
    {
        animator = GetComponent<Animator>();
        animator.SetFloat("speed", 0.3f);
        moving = true;
    }

    private void Update()
    {
        if(moving)
        {
            transform.Translate(-Vector2.right * 0.5f * Time.deltaTime);
        }
    }



}
