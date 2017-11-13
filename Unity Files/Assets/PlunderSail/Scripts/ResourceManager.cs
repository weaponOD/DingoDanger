using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    [SerializeField]
    private enum PoolTypes {};

    public static ResourceManager instance;

    [SerializeField]
    private Pool[] objectPools = null;

    // Which pool is currently being initialized
    private int currentPool = 0;

    private bool AllPoolsReady = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(InitializePools());
    }


    // Runs over several frames to instantiate the game objects needed for each pool.
    private IEnumerator InitializePools()
    {
        for(int i = 0; i < objectPools.Length; i++)
        {
            objectPools[currentPool].objects = new List<GameObject>(objectPools[currentPool].poolSize);
        }

        // Loop through array of pools and fill them up
        while(currentPool < objectPools.Length)
        {
            // Loop through the current pool and fill it 
            while (objectPools[currentPool].objects.Count < objectPools[currentPool].poolSize)
            {
                GameObject obj = (GameObject)Instantiate(objectPools[currentPool].pooledPrefab);
                obj.SetActive(false);
                objectPools[currentPool].objects.Add(obj);

                yield return new WaitForEndOfFrame();
            }

            currentPool++;
        }

        AllPoolsReady = true;
    }

    public GameObject getPooledObject(string _objectType)
    {
        int neededPool = -1;

        // First find out which pool the needed object belongs to.

        for(int i = 0; i < objectPools.Length; i++)
        {
            if(objectPools[i].objectName.Equals(_objectType))
            {
                neededPool = i;
                break;
            }
        }
        
        // return null if a pool can't be matched to the object type
        if(neededPool == -1)
        {
            return null;
        }

        // loop through the correct pool until an inactive object can be found
        for (int i = 0; i < objectPools[neededPool].objects.Count; i++)
        {
            if (!objectPools[neededPool].objects[i].activeInHierarchy)
            {
                return objectPools[neededPool].objects[i];
            }
        }

        // If an inactive object can't be found check if they pool can be expanded with a new gameobject
        if (objectPools[neededPool].canExpand)
        {
            GameObject obj = (GameObject)Instantiate(objectPools[neededPool].pooledPrefab);
            objectPools[neededPool].objects.Add(obj);

            return obj;
        }

        return null;
    }

    // Once all the pools have their instantiated gameobjects they are considered ready
    public bool PoolsReady
    {
        get { return AllPoolsReady; }
    }
}


[System.Serializable]
public class Pool
{
    public string objectName = "";

    public GameObject pooledPrefab = null;

    public int poolSize = 0;

    [Tooltip("Can the pool expand it's size as more objects are needed. If can't expand, will return null when no inactive GameObjects are left.")]
    public bool canExpand = false;

    public List<GameObject> objects = null;
}