using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 3f;
    public float timeToGetDisabled = 3f; //The bullet will get disabled a few seconds after it's fired

    private float disableCounter = 0; //If this value reaches 0, the bullet will get disabled
    private Vector3 direction;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Move the bullet along the fire direction
        transform.position +=  speed * Time.deltaTime * direction;

        //Bullet will deactivate when disableCounter reaches 0
        disableCounter -= Time.deltaTime;
        if(disableCounter <= 0f)
        {
            GetDisabled();
        }
    }

    public void GetFired(Vector3 spawnPoint, Vector3 targetPoint)
    {
        transform.position = spawnPoint; //Spawn the bullet at the player's hand

        //Set the direction
        direction = targetPoint - spawnPoint;
        direction.y = 0; //As this is a 2D game, we don't want it to move in Y axis
        direction.Normalize();

        disableCounter = timeToGetDisabled; //Bullet will deactivate when disableCounter reaches 0

        gameObject.SetActive(true);
    }

    private void GetDisabled()
    {
        gameObject.SetActive(false);
    }
}
