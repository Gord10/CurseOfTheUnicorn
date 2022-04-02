using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public Bullet bulletPrefab;
    public int bulletAmountInPool = 20;
    public float fireInterval = 1f;

    private Bullet[] bulletPool;
    private Player player;
    private bool isSpawning = true;
    private EnemyManager enemyManager;

    private void Awake()
    {
        //Create the bullets in pool
        bulletPool = new Bullet[bulletAmountInPool];
        int i;
        for(i = 0; i < bulletAmountInPool; i++)
        {
            bulletPool[i] = Instantiate(bulletPrefab, transform);
            bulletPool[i].gameObject.SetActive(false);
        }

        player = FindObjectOfType<Player>();
        enemyManager = FindObjectOfType<EnemyManager>();

        StartCoroutine(SpawnBullets());
    }

    private IEnumerator SpawnBullets()
    {
        while(isSpawning)
        {
            //Retrieve a bullet from pool and make it fired
            Bullet bullet = GetBulletFromPool();
            if(bullet)
            {
                Vector3 spawnPoint = player.bulletSpawnPoint.position;
                Enemy closestEnemy = enemyManager.GetClosestEnemyAsTarget(player.transform.position);

                if(closestEnemy)
                {
                    Vector3 targetPoint = closestEnemy.transform.position;
                    bullet.GetFired(spawnPoint, targetPoint);
                    yield return new WaitForSeconds(fireInterval);
                }
                else
                {
                    yield return new WaitForEndOfFrame();
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }

    //Returns the first disabled bullet from the pool
    private Bullet GetBulletFromPool()
    {
        int i;
        for(i = 0; i < bulletPool.Length; i++)
        {
            if(!bulletPool[i].gameObject.activeSelf)
            {
                return bulletPool[i];
            }
        }

        return null;
    }
}
