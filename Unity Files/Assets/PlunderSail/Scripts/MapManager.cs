using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private Transform player;

    private Zone[,] map;

    [SerializeField]
    private GameObject[] zones;

    [SerializeField]
    private GameObject waterTile;

    private Vector3[] SpawnPoints;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Start()
    {
        map = new Zone[5, 5];
    }

    private void InitMap()
    {
        foreach (Zone zone in map)
        {
            zone.Init(waterTile, zones[Random.Range(0, zones.Length)]);
        }
    }
}

public class Zone : MonoBehaviour
{
    private GameObject waterTile;
    private GameObject islandsPrefab;

    private Vector3 pos;

    public void Init(GameObject _water, GameObject _islands)
    {
        waterTile = _water;
        islandsPrefab = _islands;
    }

    public void Spawn(Vector3 _spawnPos)
    {
        Transform water = Instantiate(waterTile, _spawnPos, Quaternion.identity).transform;
        water.parent = transform;

        Transform islands = Instantiate(waterTile, _spawnPos, Quaternion.identity).transform;
        islands.parent = transform;
    }

    public Vector3 Position
    {
        get { return pos; }
        set { pos = value; }
    }
}