using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public static Player instance;

    public float baseHorizontalSpeed = 3f;
    public float flySpeed = 11f;
    public float maxY = 1f; //Don't allow the player to move above this Y value
    public Transform bulletSpawnPoint;
    public float maxHealth = 10;
    public SpriteRenderer harmedUnicornSpriteRenderer;
    public float healthIncreaseOnHeal = 2f;

    private float health = 0; //maxHealth will be assigned to here at Awake


    private Rigidbody2D rigidbody;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Vector2 desiredMovementDirection = new Vector2();
    private bool isTouchingEnemy = false;

    private void Awake()
    {
        instance = this;

        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.IsGameOver())
        {
            harmedUnicornSpriteRenderer.enabled = true;
            return;
        }

        //Read the player input
        desiredMovementDirection.x = Input.GetAxis("Horizontal");
        desiredMovementDirection.y = Input.GetAxis("Vertical");

        if(desiredMovementDirection.y < 0)
        {
            desiredMovementDirection.y = 0;
        }

        animator.SetBool("flying", desiredMovementDirection.y > 0);

        //Flip the sprite renderer according to the X direction that the player wants to go
        if(desiredMovementDirection.x > 0)
        {
            transform.rotation = Quaternion.identity;
        }
        else if(desiredMovementDirection.x < 0)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    private void FixedUpdate()
    {
        GameManager gameManager = GameManager.instance;
        if(gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        if(gameManager.IsGameOver())
        {
            harmedUnicornSpriteRenderer.enabled = true;
            return;
        }
        harmedUnicornSpriteRenderer.enabled = false;

        Vector2 velocity = new Vector2();
        velocity.x = desiredMovementDirection.x * baseHorizontalSpeed;

        velocity.y = desiredMovementDirection.y * flySpeed;
        float gravityFactor = 0.1f; //If we don't multiply gravity with this value, player falls to the ground too hard/fast
        velocity.y += Physics2D.gravity.y * gravityFactor; //Apply gravity to the velocity

        //Don't allow the player to move above the camera frame
        if (transform.position.y > maxY && velocity.y > 0)
        {
            velocity.y = 0;
        }

        //Move the player along the desired direction
        rigidbody.velocity = velocity;
        VibrationManager.StopVibration();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if(GameManager.instance.IsGameOver())
        {
            return;
        }

        if(collision.gameObject.CompareTag("Enemy")) //Player will be hurt
        {
            Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();
            health -= enemy.harmPerSecond * Time.fixedDeltaTime;
            GameUi.instance.FillHealthBar(health / maxHealth);
            harmedUnicornSpriteRenderer.enabled = true;
            GameCamera.instance.Shake();

            if(health <= 0)
            {
                GameManager.instance.ReportGameOver();
            }
            else
            {
                SfxManager.instance.PlayHurtSound();
                VibrationManager.Vibrate();
            }
        }
    }

    //Player has chosen a heal upgrade
    public void IncreaseHealth()
    {
        health += healthIncreaseOnHeal;
        health = Mathf.Clamp(health, 0, maxHealth);
        GameUi.instance.FillHealthBar(health / maxHealth);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if(collision.gameObject.GetComponent<Death>())
            {
                GameManager.instance.ReportTouchOfDeath();
            }
            else
            {
                //VibrationManager.Vibrate();
            }
        }
    }
}
