using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    public int maxHealth = 5;
    int currentHealth;

    public float speed = 3.0f;
    public float timeInvincible = 2.0f;

    public int gethealth { get { return currentHealth; } }
    //public int sethealth { set { return currentHealth; } }

    bool isInvincible;
    float invincibleTimer;

    Animator animator;
    Vector2 lookdirection = new Vector2(1,0);
    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    public GameObject projectilePrefab;

    public ParticleSystem hitParticle;
    public ParticleSystem healParticle;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
            {
            lookdirection.Set(move.x, move.y);
            lookdirection.Normalize();
        }

        animator.SetFloat("Look X", lookdirection.x);
        animator.SetFloat("Look Y", lookdirection.y);
        animator.SetFloat("Speed", move.magnitude);
 
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
            { isInvincible = false; }
            
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookdirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                JambiController jambi = hit.collider.GetComponent<JambiController>();
                if (jambi != null)
                {

                    jambi.DisplayDialog();
                    //Debug.Log("Raycast hit " + hit.collider.gameObject);
                }
            }
        }
    }

    void FixedUpdate() 
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }
    
    public void ChangeHealth(int amount)
    {
        if (amount <= -1)
        {
            if (amount < 0)
            {
                if (isInvincible)
                { return; }

                isInvincible = true;
                invincibleTimer = timeInvincible;
            }
            animator.SetTrigger("Hit");
            Instantiate(hitParticle, rigidbody2d.position + Vector2.up * .5f, Quaternion.identity);
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
            UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
        }
        else
        {
            if (amount < 0)
            {
                if (isInvincible)
                { return; }

                isInvincible = true;
                invincibleTimer = timeInvincible;
            }
            Instantiate(healParticle, rigidbody2d.position + Vector2.up * .5f, Quaternion.identity);
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
            UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
        }
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookdirection, 300);

        animator.SetTrigger("Launch");
    }
}
