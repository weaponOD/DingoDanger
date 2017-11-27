using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGenerator : MonoBehaviour
{
    [SerializeField]
    private Transform waterPrefab;

    [SerializeField]
    private Vector2 mapSize;

    [SerializeField]
    private float buffer = 0.5f;

    [SerializeField]
    private GameObject tempPlane;


    private void Start()
    {
        tempPlane.SetActive(false);
        StartCoroutine(GenerateMap());
    }

    public IEnumerator GenerateMap()
    {
        //Store Tiles into an empty called Water Tiles

        string holderName = "Water Tiles";

        if (transform.Find(holderName))
        {
            DestroyImmediate(transform.Find(holderName).gameObject);
        }

        Transform mapHolder = new GameObject(holderName).transform;

        mapHolder.parent = transform;

        //Generate Map

        for (int x = 0; x < mapSize.x; x++)
        {
            for (int y = 0; y < mapSize.y; y++)
            {

                //Prepare where to place tile
                //Vector3 waterPos = new Vector3(-mapSize.y / 2 + buffer * y, 0f, -mapSize.x / 2 + buffer * x);

                Vector3 waterPos = new Vector3(692 + y * buffer, 0f, 692 + x * buffer);

                //Create the new tile using caluclated position
                Transform newWater = Instantiate(waterPrefab, waterPos, Quaternion.identity) as Transform;

                newWater.parent = mapHolder;
            }

            yield return new WaitForEndOfFrame();
        }
    }

}