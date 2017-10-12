using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] enemyPrefabs;

    private GameObject[] activeEnemies;

    private Transform player;

    [SerializeField]
    private int maxEnemies;

    [SerializeField]
    private float spawnRange;

    [SerializeField]
    private float deSpawnRange;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        activeEnemies = new GameObject[maxEnemies];

        StartCoroutine(CheckEnemies());
    }

    IEnumerator CheckEnemies()
    {
        activeEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (activeEnemies.Length < maxEnemies && !GameState.BuildMode)
        {
            SpawnEnemy();
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(CheckEnemies());
    }
    private void SpawnEnemy()
    {
        Vector3 spawnPos = OutOfSightPos();

        if ((Physics2D.OverlapCircle(spawnPos, 100f, 0, 0, 0)) == null)
        {
            Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPos, Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f));
        }
        else
        {
            SpawnEnemy();
        }
    }

    Vector3 OutOfSightPos()
    {
        Vector3 spawnPoint = player.position + RandomPointOnUnitCircle(spawnRange);

        spawnPoint.y = 0f;
        return spawnPoint;
    }

    private Vector3 RandomPointOnUnitCircle(float radius)
    {
        float angle = Random.Range(0f, Mathf.PI * 2);
        float x = Mathf.Sin(angle) * radius;
        float z = Mathf.Cos(angle) * radius;

        return new Vector3(x, 0f, z);
    }
}