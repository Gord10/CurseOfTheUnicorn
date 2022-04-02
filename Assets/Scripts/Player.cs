using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float baseSpeed = 3f;

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
        desiredMovementDirection = Vector2.ClampMagnitude(desiredMovementDirection, 1f);
    }

    private void FixedUpdate()
    {
        //Move the player along the desired direction
        rigidbody.velocity = desiredMovementDirection * baseSpeed;
    }
}
