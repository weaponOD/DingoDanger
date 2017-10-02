using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] islandPrefabs;

    private List<Transform> activeIslands;

    private Transform player;

    [SerializeField]
    private int maxIslands;

    [SerializeField]
    private float spawnRange;

    [SerializeField]
    private float deSpawnRange = 200f;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        activeIslands = new List<Transform>(maxIslands);

        StartCoroutine(CheckIslands());
    }

    IEnumerator CheckIslands()
    {
        DeSpawnIslands();

        if (activeIslands.Count < maxIslands)
        {
            SpawnIsland();
            Debug.Log("Spaned Island");
        }

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(CheckIslands());
    }
    private void SpawnIsland()
    {
        Vector3 spawnPos = OutOfSightPos();

        if ((Physics2D.OverlapCircle(spawnPos, 100f, 0, 0, 0)) == null)
        {
            Transform newIsland = Instantiate(islandPrefabs[Random.Range(0, islandPrefabs.Length)], spawnPos, Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f)).transform;

            activeIslands.Add(newIsland);
        }
    }

    private void DeSpawnIslands()
    {
        foreach (Transform island in activeIslands)
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
        activeIslands.Remove(_island);
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