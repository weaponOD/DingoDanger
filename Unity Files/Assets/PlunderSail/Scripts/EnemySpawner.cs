using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] enemyPrefabs;

    private List<Transform> activeEnemies;

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
        activeEnemies = new List<Transform>(maxEnemies);

        StartCoroutine(CheckEnemies());
    }

    IEnumerator CheckEnemies()
    {
        DeSpawnEnemies();

        if (activeEnemies.Count < maxEnemies)
        {
            SpawnEnemy();
            Debug.Log("Spaned Island");
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(CheckEnemies());
    }
    private void SpawnEnemy()
    {
        Vector3 spawnPos = OutOfSightPos();

        if ((Physics2D.OverlapCircle(spawnPos, 100f, 0, 0, 0)) == null)
        {
            Transform newIsland = Instantiate(enemyPrefabs[Random.Range(0, enemyPrefabs.Length)], spawnPos, Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f)).transform;

            activeEnemies.Add(newIsland);
        }
        else
        {
            SpawnEnemy();
        }
    }

    private void DeSpawnEnemies()
    {
        foreach (Transform island in activeEnemies)
        {
            float distanceFromPlayer = Vector3.Distance(island.position, player.position);

            if (distanceFromPlayer > deSpawnRange)
            {
                island.gameObject.GetComponent<Island>().DeSpawn();

                Debug.Log("DeSpawn");
            }
        }
    }

    public void RemoveMe(Transform _island)
    {
        activeEnemies.Remove(_island);
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