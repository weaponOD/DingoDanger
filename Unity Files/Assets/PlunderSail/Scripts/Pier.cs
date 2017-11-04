using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pier : MonoBehaviour
{
    [SerializeField]
    private Transform dockingPos;

    private GameManager GM;

    [SerializeField]
    private bool open = false;

    [SerializeField]
    private float timeToRemovePier = 0;

    private void Awake()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    public bool isUnlocked
    {
        get { return open; }
        set { open = value; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (open)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                GM.setPier(dockingPos);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (open)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Invoke("RemovePier", timeToRemovePier);
            }
        }
    }

    private void RemovePier()
    {
        GM.setPier(null);
    }
}