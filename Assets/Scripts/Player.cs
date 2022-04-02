using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float baseSpeed = 3f;

    private Rigidbody2D rigidbody;
    private Vector2 desiredMovementDirection = new Vector2();

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        desiredMovementDirection.x = Input.GetAxis("Horizontal");
        desiredMovementDirection.y = Input.GetAxis("Vertical");

        //We don't want to player to move too fast when they are moving diagonally
        desiredMovementDirection = Vector2.ClampMagnitude(desiredMovementDirection, 1f);
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = desiredMovementDirection * baseSpeed;
    }
}
