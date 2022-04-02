using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    public float minSpawnX = -10, maxSpawnX = 10;
    public int enemyPoolLength = 30;
    public Enemy enemyPrefab;
    public float spawnInterval = 0.5f;
    public float spawnIntervalDecreasePerSecond = 0.001f; //To make the game harder, we will make the spawning faster as time passes by
    public float minSpawnInterval = 0.1f; //The hardest spawning interval
    public float timeLimitForBeingDifficult = 240; //If the game time exceeds this seconds, we will use maxHealthHardDifficulty as enemy health

    private Player player;
    private Enemy[] enemies;


    private void Awake()
    {
        instance = this;

        player = FindObjectOfType<Player>();

        Enemy[] previousEnemiesInScene = FindObjectsOfType<Enemy>();
        int i;

        for(i = 0; i < previousEnemiesInScene.Length; i++)
        {
            Destroy(previousEnemiesInScene[i].gameObject);
        }

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

    public Enemy GetClosestEnemyAsTarget(Vector3 playerPosition)
    {
        float minDistance = float.MaxValue;
        Enemy closestEnemy = null;

        if(enemies == null)
        {
            return null;
        }

        int i;
        for(i = 0; i < enemies.Length; i++)
        {
            Enemy enemy = enemies[i];
            if(enemy.gameObject.activeSelf && enemy.IsVisible() && enemy.IsAlive())
            {
                float distance = Vector3.Distance(playerPosition, enemy.transform.position);
                if(distance < minDistance)
                {
                    closestEnemy = enemy;
                    minDistance = distance;
                }
            }
        }

        return closestEnemy;
    }

    public bool IsXPointTooCloseToPlayer(float x)
    {
        return Mathf.Abs(player.transform.position.x - x) < 1.8f;
    }

    private IEnumerator SpawnEnemies()
    {
        while(true)
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

    public void Update()
    {
        if (spawnInterval > minSpawnInterval)
        {
            spawnInterval -= spawnIntervalDecreasePerSecond * Time.deltaTime;
        }        
    }
}
