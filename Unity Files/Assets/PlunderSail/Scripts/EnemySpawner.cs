using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    private Transform player;

    private int maxEnemies = 1;

    private int enemyCount = 0;

    [SerializeField]
    private GameObject enemyPrefab;

    private List<GameObject> enemyList;

    private float enemyDistanceLimit = 150f;

    float distanceToPlayer;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Start()
    {
        enemyList = new List<GameObject>(10);
        InvokeRepeating("SpawnEnemy", 0f, 5f);
    }

    void Update()
    {
        //foreach(GameObject enemy in enemyList)
        //{
        //    // work out distance from enemy to player
        //    float distanceToPlayer = Vector3.Distance(enemy.transform.position, player.position);

        //    if(distanceToPlayer > enemyDistanceLimit)
        //    {
        //        enemy.SetActive(false);
        //        enemyCount--;
        //    }
        //}

        if (enemyList.Count > 0)
        {
            //work out distance from enemy to player
            distanceToPlayer = Vector3.Distance(enemyList[0].transform.position, player.position);

            if (distanceToPlayer > enemyDistanceLimit)
            {
                GameObject enemy = enemyList[0];
                enemyList.Remove(enemyList[0]);
                GameManager.Destroy(enemy);
                enemyCount--;
            }
        }
    }

    private void SpawnEnemy()
    {
        if (enemyCount < maxEnemies)
        {
            Vector3 spawnPoint = player.position - player.forward * 10f;

            GameObject newEnemy = Instantiate(enemyPrefab, spawnPoint, Quaternion.identity);
            newEnemy.name = "Enemy: " + enemyCount;

            enemyList.Add(newEnemy);
            enemyCount++;
        }
    }
}