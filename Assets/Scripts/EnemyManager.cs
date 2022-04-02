using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public float minSpawnX = -10, maxSpawnX = 10;
    public int enemyPoolLength = 30;
    public Enemy enemyPrefab;
    public float spawnInterval = 0.5f;

    private Player player;
    private Enemy[] enemies;


    private void Awake()
    {
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

    public Enemy GetClosestVisibleEnemy(Vector3 playerPosition)
    {
        float minDistance = float.MaxValue;
        Enemy closestEnemy = null;

        int i;
        for(i = 0; i < enemies.Length; i++)
        {
            Enemy enemy = enemies[i];
            if(enemy.gameObject.activeSelf && enemy.IsVisible())
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

    private IEnumerator SpawnEnemies()
    {
        while(true)
        {
            Enemy enemy = GetEnemyFromPool();
            if(enemy)
            {
                float x = Random.Range(minSpawnX, maxSpawnX);
                Vector3 spawnPoint = new Vector3(x, 0, 0);
                enemy.Spawn(spawnPoint);
            }
            yield return new WaitForSeconds(spawnInterval);
        }
    }


}
