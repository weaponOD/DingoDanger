using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterManager : MonoBehaviour
{
    [SerializeField]
    private int width = 5;

    [SerializeField]
    private int height = 5;

    [SerializeField]
    private int tileSize = 690;

    [SerializeField]
    private int cullDist = 2;

    [SerializeField]
    private GameObject waterPrefab = null;

    [SerializeField]
    private Transform player;

    [SerializeField]
    private Vector2 playerPos;

    private WaterTile[,] waterTiles = null;

    private void Start()
    {
        StartCoroutine(CreateWater());
    }

    private IEnumerator CheckPlayerPos()
    {
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (waterTiles[x, y].Contains(new Vector2(player.position.x, player.position.z)))
                {
                    playerPos.x = x;
                    playerPos.y = y;
                }
            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (Vector2.Distance(new Vector2(x,y), playerPos) > cullDist)
                {
                    waterTiles[x, y].tile.gameObject.SetActive(false);
                }
                else
                {
                    waterTiles[x, y].tile.gameObject.SetActive(true);
                }
            }
        }

        yield return new WaitForSeconds(0.1f);
        StartCoroutine(CheckPlayerPos());
    }

    private IEnumerator CreateWater()
    {
        Transform tiles = new GameObject("WaterTiles").transform;

        waterTiles = new WaterTile[width, height];
        int row = 0;
        int column = 0;

        for (int y = -(height / 2); y < (height / 2) + 1; y++)
        {
            for (int x = -(width / 2); x < (width / 2) + 1; x++)
            {
                Transform water = Instantiate(waterPrefab, new Vector3(x * tileSize, 0f, y * tileSize), Quaternion.identity).transform;

                water.parent = tiles;
                waterTiles[row, column] = new WaterTile(water, new Vector2(x * tileSize, y * tileSize), new Vector2(690 / 2, 690 / 2));

                row++;
                yield return new WaitForSeconds(0.1f);
            }
            column++;
            row = 0;
        }

        StartCoroutine(CheckPlayerPos());
    }
}

public class WaterTile
{
    public Transform tile;
    public Vector2 centre;
    public Vector2 bounds;

    public WaterTile(Transform _tile, Vector2 _centre, Vector2 _bounds)
    {
        tile = _tile;
        centre = _centre;
        bounds = _bounds;
    }

    public bool Contains(Vector2 pos)
    {
        if (pos.x > centre.x - bounds.x && pos.x < centre.x + bounds.x)
        {
            if (pos.y > centre.y - bounds.y && pos.y < centre.y + bounds.y)
            {
                return true;
            }
        }

        return false;
    }
}