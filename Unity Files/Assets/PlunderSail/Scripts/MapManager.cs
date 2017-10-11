using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    private Zone[,] map;

    [SerializeField]
    private GameObject[] zonePrefabs;

    private Vector3[] SpawnPoints;

    private void Start()
    {
        map = new Zone[9, 9];

        //InitMap();
    }

    // Loops through the 2D array of Zones and instantiates the prefabs, sets to null if outside of the diamond map shape.
    private void InitMap()
    {
        // Used to tapper off the top and bottom of the 2D grid to create a diamon shape within it.
        int killZone = 3;

        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                if (x > killZone && x <= map.GetLength(0) - killZone)
                {
                    map[x, y] = Instantiate(zonePrefabs[Random.Range(0, zonePrefabs.Length)], new Vector3(x, 0, y), Quaternion.identity).GetComponent<Zone>();
                    killZone--;
                }
                else
                {
                    map[x, y] = null;
                }
            }
        }

    }
}