using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed;
    public bool vertical;
    public float changeTime = 3.0f;

    Animator animator;
    Rigidbody2D rigidbody2d;
    float timer;
    int direction = 1;

    bool broken = true;
    public ParticleSystem smokeEffect;
    public ParticleSystem healEffect;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!broken){
            return;
        }
        
        timer -= Time.deltaTime;

        if (timer < 0)
            {
            direction = -direction;
            timer = changeTime;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;

        if (!broken)
        {
            return;
        }

        if (vertical)
        {
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
            position.y = position.y + Time.deltaTime * speed * direction;
        }
        else
        {
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", direction);
            position.x = position.x + speed * Time.deltaTime * direction;
        }
        
        rigidbody2d.MovePosition(position);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        RubyController player = other.gameObject.GetComponent<RubyController>();

        if (player != null)
            {
            player.ChangeHealth(-1);
        }
    }

    public void Fix()
    {
        broken = false;
        rigidbody2d.simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
        Instantiate(healEffect, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
    }
}
