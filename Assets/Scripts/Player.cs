using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float baseHorizontalSpeed = 3f;
    public float flySpeed = 11f;
    public float maxY = 1f; //Don't allow the player to move above this Y value
    public Transform bulletSpawnPoint;

    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    private Vector2 desiredMovementDirection = new Vector2();

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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
            spriteRenderer.flipX = false;
        }
        else if(desiredMovementDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }

        //We don't want to player to move too fast when they are moving diagonally
        //desiredMovementDirection = Vector2.ClampMagnitude(desiredMovementDirection, 1f);
    }

    private void FixedUpdate()
    {
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
}
