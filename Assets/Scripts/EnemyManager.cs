using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    public Death deathPrefab;
    public float minSpawnX = -10, maxSpawnX = 10;
    public int enemyPoolLength = 30;
    public Enemy enemyPrefab;
    public float spawnInterval = 0.5f;
    public float spawnIntervalDecreasePerSecond = 0.001f; //To make the game harder, we will make the spawning faster as time passes by
    public float minSpawnInterval = 0.1f; //The hardest spawning interval
    public float timeLimitForBeingDifficult = 240; //If the game time exceeds this seconds, we will use maxHealthHardDifficulty as enemy health

    private Player player;
    private Enemy[] enemies;
    private bool isSpawningEnemies = true;
    private bool didWeSpawnDeath = false;

    private void Awake()
    {
        instance = this;

        player = FindObjectOfType<Player>();

        //Destroy the previous enemies in the scene
        Enemy[] previousEnemiesInScene = FindObjectsOfType<Enemy>();
        int i;

        for(i = 0; i < previousEnemiesInScene.Length; i++)
        {
            Destroy(previousEnemiesInScene[i].gameObject);
        }

        //Instantiate the enemies
        enemies = new Enemy[enemyPoolLength];

        for(i = 0; i < enemyPoolLength; i++)
        {
            enemies[i] = Instantiate(enemyPrefab, transform);
            enemies[i].gameObject.SetActive(false);
        }

        StartCoroutine(SpawnEnemies());
    }

    private Enemy GetEnemyFromPool()
    {
        int i;
        for(i = 0; i < enemies.Length; i++)
        {
            if(!enemies[i].gameObject.activeSelf)
            {
                return enemies[i];
            }
        }

        return null;
    }

    //Gets the closest enemy who is both alive and rendered
    public Enemy GetClosestEnemyAsTarget(Vector3 playerWeaponPosition)
    {
        float minDistance = float.MaxValue;
        Enemy closestEnemy = null;

        //Don't return anything if the array is not initialized yet
        if(enemies == null)
        {
            return null;
        }

        int i;
        for(i = 0; i < enemies.Length; i++)
        {
            Enemy enemy = enemies[i];
            //Check if the enemy is alive, active and rendered
            if (enemy.gameObject.activeSelf && enemy.IsVisible() && enemy.IsAlive())
            {
                float distance = Vector3.Distance(playerWeaponPosition, enemy.transform.position);
                if(distance < minDistance)
                {
                    closestEnemy = enemy;
                    minDistance = distance;
                }
            }
        }

        return closestEnemy;
    }

    //We do this check, because we don't want to spawn an enemy that's too close to the player
    public bool IsXPointTooCloseToPlayer(float x)
    {
        float minDistance = 3.5f;
        return Mathf.Abs(player.transform.position.x - x) < minDistance;
    }

    private IEnumerator SpawnEnemies()
    {
        while(isSpawningEnemies)
        {
            Enemy enemy = GetEnemyFromPool();
            if(enemy)
            {
                float x = Random.Range(minSpawnX, maxSpawnX);

                if(!IsXPointTooCloseToPlayer(x)) //Don't spawn the enemy if x point too close to the player
                {
                    Vector3 spawnPoint = new Vector3(x, 0, 0);
                    enemy.Spawn(spawnPoint);
                }
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void SpawnDeath()
    {
        didWeSpawnDeath = true;
        isSpawningEnemies = false;
        //Kill the existing enemies in the scene
        int i;
        for(i = 0; i < enemies.Length; i++)
        {
            enemies[i].Die();
        }

        //Spawn Death at a random point
        Death death = Instantiate(deathPrefab);
        float x = Random.Range(minSpawnX, maxSpawnX);

        //Make sure we find a point that's not too close to the player
        while(IsXPointTooCloseToPlayer(x))
        {
            x = Random.Range(minSpawnX, maxSpawnX);
        }

        Vector3 spawnPoint = new Vector3(x, 0, 0);
        death.Spawn(spawnPoint);
    }

    public void Update()
    {
        //Make the game harder gradually
        if (spawnInterval > minSpawnInterval)
        {
            spawnInterval -= spawnIntervalDecreasePerSecond * Time.deltaTime;
        }

#if UNITY_EDITOR || true
        if (Input.GetKeyDown(KeyCode.L) && Input.GetKey(KeyCode.LeftShift))
        {
            SpawnDeath();
        }
#endif
    }
}
