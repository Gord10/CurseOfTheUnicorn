using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3;
    public int maxHealth = 3;
    public float harmPerSecond = 2f;
 
    private int health = 3;

    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;

    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public bool IsVisible()
    {
        return spriteRenderer.isVisible;
    }

    private void FixedUpdate()
    {
        //Fly towards the player
        Vector3 direction = player.transform.position - transform.position;
        direction.z = 0;
        direction.Normalize();
        rigidbody.velocity = direction * speed;

        //Let the enemy look like as if he's looking at the player
        spriteRenderer.flipX = direction.x < 0;
    }

    public void GetHarmed()
    {
        health--;
        if(health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
    
    public void Spawn(Vector3 spawnPoint)
    {
        health = maxHealth;
        transform.position = spawnPoint;
        gameObject.SetActive(true);
    }
}
