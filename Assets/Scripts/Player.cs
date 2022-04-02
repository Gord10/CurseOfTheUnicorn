using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float baseHorizontalSpeed = 3f;
    public float flySpeed = 11f;
    public float maxY = 1f; //Don't allow the player to move above this Y value
    public Transform bulletSpawnPoint;
    public float maxHealth = 10;
    public SpriteRenderer harmedUnicornSpriteRenderer;

    private float health = 0; //maxHealth will be assigned to here at Awake


    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    private Vector2 desiredMovementDirection = new Vector2();
    private bool isTouchingEnemy = false;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = maxHealth;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Read the player input
        desiredMovementDirection.x = Input.GetAxis("Horizontal");
        desiredMovementDirection.y = Input.GetAxis("Vertical");

        if(desiredMovementDirection.y < 0)
        {
            desiredMovementDirection.y = 0;
        }

        //Flip the sprite renderer according to the X direction that the player wants to go
        if(desiredMovementDirection.x > 0)
        {
            transform.rotation = Quaternion.identity;
        }
        else if(desiredMovementDirection.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        //We don't want to player to move too fast when they are moving diagonally
        //desiredMovementDirection = Vector2.ClampMagnitude(desiredMovementDirection, 1f);
    }

    private void FixedUpdate()
    {
        harmedUnicornSpriteRenderer.enabled = false;

        Vector2 velocity = new Vector2();
        velocity.x = desiredMovementDirection.x * baseHorizontalSpeed;

        velocity.y = desiredMovementDirection.y * flySpeed;
        velocity.y += Physics2D.gravity.y * 0.1f; //Apply gravity to the velocity

        //Don't allow the player to move above the camera frame
        //Vector3 rigidbodyPos = transform.position;
        if (transform.position.y > maxY && velocity.y > 0)
        {
            velocity.y = 0;
        }

        //Move the player along the desired direction
        rigidbody.velocity = velocity;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();
            health -= enemy.harmPerSecond * Time.fixedDeltaTime;
            GameUi.instance.FillHealthBar(health / maxHealth);
            harmedUnicornSpriteRenderer.enabled = true;

            if(health <= 0)
            {
                GameManager.RestartScene();
            }
        }
    }
}
