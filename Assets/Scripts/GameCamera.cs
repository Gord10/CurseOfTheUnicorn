using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCamera : MonoBehaviour
{
    public static GameCamera instance;

    public float minX = -6f, maxX = 6f; //These values are used to not to show beyond the colliders that limit the game world
    public float shakeTime = 0.1f; //The camera will shake for this duration if player touches enemy
    public float shakeMaxZ = 1f; //The Z rotation of the camera will be withing -shakeMaxZ and shakeMaxZ
    private bool isShaking = false;
    private float shakeCounter = 0;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Follow the player in X axis
        Vector3 newPosition = transform.position;
        newPosition.x = Player.instance.transform.position.x;
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX); //These clamping is used to not to show beyond the colliders that limit the game world
        transform.position = newPosition;

        //Try to shake the camera
        if(isShaking)
        {
            float randomZ = Random.Range(-shakeMaxZ, shakeMaxZ);
            transform.rotation = Quaternion.Euler(0, 0, randomZ);
            shakeCounter -= Time.unscaledDeltaTime;
            if(shakeCounter <= 0)
            {
                isShaking = false;
            }
        }
        else
        {
            transform.rotation = Quaternion.identity;
        }
    }

    public void Shake()
    {
        isShaking = true;
        shakeCounter = shakeTime;
    }
}
