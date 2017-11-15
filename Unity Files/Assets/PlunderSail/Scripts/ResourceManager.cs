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

    // Set the specified object to inactive after t seconds
    public void DelayedDestroy(GameObject _object, float _time)
    {
        StartCoroutine(Destroy(_object, _time));
    }


    private IEnumerator Destroy(GameObject _object, float _time)
    {
        yield return new WaitForSeconds(_time);

        _object.SetActive(false);
    }

    // Returns a reference to a pool of objects matching the specified object type
    public Pool getPool(string _objectType)
    {
        for(int i = 0; i < objectPools.Length; i++)
        {
            if(objectPools[i].objectName.Equals(_objectType))
            {
                return objectPools[i];
            }
        }

        return null;
    }

    // Once all the pools have their instantiated gameobjects they are considered ready
    public bool PoolsReady
    {
        get { return AllPoolsReady; }
    }

    public GameObject AddToPool(GameObject _prefab)
    {
        return (GameObject)Instantiate(_prefab);
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

    public GameObject getPooledObject()
    {
        // loop through the correct pool until an inactive object can be found
        for (int i = 0; i < objects.Count; i++)
        {
            if (!objects[i].activeInHierarchy)
            {
                return objects[i];
            }
        }

        // If an inactive object can't be found check if they pool can be expanded with a new gameobject
        if (canExpand)
        {
            GameObject obj = ResourceManager.instance.AddToPool(pooledPrefab);
            objects.Add(obj);

            return obj;
        }

        return null;
    }
}