using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Island : MonoBehaviour
{
    private float sizeIncrement = 0.01f;

    private bool shrink = false;

    private MapManager manager;

    private void Awake()
    {
        transform.localScale = Vector3.zero;

        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MapManager>();
    }

    private void Update()
    {
        if(shrink)
        {
            if (transform.localScale.x > 0)
            {
                transform.localScale -= new Vector3(sizeIncrement, sizeIncrement, sizeIncrement);
            }

            if(transform.localScale.x <= 0)
            {
                //manager.RemoveMe(transform);

                Destroy(gameObject);
            }
        }
        else
        {
            if (transform.localScale.x < 1f)
            {
                transform.localScale += new Vector3(sizeIncrement, sizeIncrement, sizeIncrement);
            }
        }
    }

    public void DeSpawn()
    {
        shrink = true;
    }
}