using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 3;
    public Sprite greenSprite;
    public int maxHealthNormalDifficulty = 2;
    public int maxHealthHardDifficulty = 3;
    
    public float harmPerSecond = 2f;
    public float yToGetDisabled = -1.5f; //If he falls below this point, he will get disabled
 
    private int health = 3;

    private Rigidbody2D rigidbody;
    private SpriteRenderer spriteRenderer;
    private Collider2D collider;
    private float timeWhenSpawned = 0f;
    private bool isShowingHarmedFeedback = false;

    private Player player;

    private void Awake()
    {
        player = FindObjectOfType<Player>();
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        collider = GetComponentInChildren<Collider2D>();
    }

    public bool IsVisible()
    {
        return spriteRenderer.isVisible;
    }

    public bool IsAlive()
    {
        return health > 0;
    }

    private void FixedUpdate()
    {
        if(player == null)
        {
            player = FindObjectOfType<Player>();
        }

        if(health > 0)
        {
            //Fly towards the player
            Vector3 direction = player.transform.position - transform.position;
            direction.z = 0;
            direction.Normalize();
            rigidbody.velocity = direction * speed;

            //Let the enemy look like as if he's looking at the player
            if(direction.x < 0)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
            else
            {
                transform.rotation = Quaternion.identity;
            }
        }

        if(transform.position.y < yToGetDisabled)
        {
            gameObject.SetActive(false);
        }
    }

    public virtual void GetHarmed()
    {
        health--;
        if(health <= 0)
        {
            Die();
            GameManager.instance.ReportEnemyDeath();
        }

        if(!isShowingHarmedFeedback)
        {
            StartCoroutine(ShowHarmedFeedback());
        }
    }

    public void Die()
    {
        collider.enabled = false;
        rigidbody.gravityScale = 1f;
        health = 0;
    }
        
    public void Spawn(Vector3 spawnPoint)
    {
        health = maxHealthNormalDifficulty;

        //Make the enemy hard
        if(Time.timeSinceLevelLoad >= EnemyManager.instance.timeLimitForBeingDifficult && this is not Death)
        {
            health = maxHealthHardDifficulty;
            spriteRenderer.sprite = greenSprite;
        }

        transform.position = spawnPoint;
        gameObject.SetActive(true);
        timeWhenSpawned = Time.time;
        collider.enabled = true;
        rigidbody.gravityScale = 0;
    }

    //Enemy will go red, then to normal color again
    public IEnumerator ShowHarmedFeedback()
    {
        isShowingHarmedFeedback = true;
        float gettingRedSpeed = 10f;
        float gettingWhiteSpeed = 3f;

        float t = 0;
        //Turn into red
        while(t < 1)
        {   
            t = Mathf.MoveTowards(t, 1f, Time.deltaTime * gettingRedSpeed);
            spriteRenderer.color = Color.Lerp(Color.white, Color.red, t);
            yield return new WaitForEndOfFrame();
        }

        //Turn into white
        while(t > 0)
        {
            t = Mathf.MoveTowards(t, 0f, Time.deltaTime * gettingWhiteSpeed);
            spriteRenderer.color = Color.Lerp(Color.white, Color.red, t);
            yield return new WaitForEndOfFrame();
        }

        isShowingHarmedFeedback = false;
    }

    public void Update()
    {
        //Sway movement with code, we will use Sin function
        float speed = 5f;
        float movementRange = 0.05f;
        float spriteY = Mathf.Sin((Time.time - timeWhenSpawned) * speed) * movementRange;
        spriteRenderer.transform.localPosition = new Vector3(0, spriteY, 0);
    }
}
