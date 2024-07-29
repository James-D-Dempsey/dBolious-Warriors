using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBullet : MonoBehaviour
{
    public float speed = 20f;
    public float Damage = 5f;
    public Rigidbody2D rb;
    public GameObject bullet;

    void Start()
    {
        rb.velocity = transform.right * speed;

        Destroy(gameObject, 2f); //ensures bullet is destroyed 
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        playerHealth percent = hitInfo.GetComponent<playerHealth>();
        CPUhealth percent2 = hitInfo.GetComponent<CPUhealth>();

        if (percent != null) {
            percent.takeDamage(Damage, bullet);
        }
        if (percent2 != null)
        {
            percent2.takeDamage(Damage, bullet);
        }

        Destroy(gameObject); //ensures bullet is destroyed when colliding with player
    }
}
