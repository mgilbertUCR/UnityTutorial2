using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    Rigidbody2D rigidbody2d;

    //public AudioClip launchClip;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, float force)
        {
        //RubyController controller = GetComponent<RubyController>();
        rigidbody2d.AddForce(direction * force);
            //controller.PlaySound(launchClip);
    }

    void OnCollisionEnter2D(Collision2D other)
        {
            EnemyController e = other.collider.GetComponent<EnemyController>();
            
            if (e != null)
        {
            e.Fix();
        }
            Debug.Log("Projectile hit " + other.gameObject);
            Destroy(gameObject);
        }
      

    // Update is called once per frame
    void Update()
    {
        if(transform.position.magnitude > 1000f)
        {
            Destroy(gameObject);
        }
    }
}
