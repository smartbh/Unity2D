using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public float speed = 10f;
    public float lifetime = 2f;
    public Vector2 direction;

    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction * speed;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) 
        {
            //collision.CompareTag("Floor")
            Debug.Log("Hit Player!");
            Destroy(gameObject);
            collision.GetComponent<BattleSystem>()?.OnDamage();
        }
    }
}
