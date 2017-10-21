using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pier : MonoBehaviour
{
    [SerializeField]
    private Transform dockingPos;

    private GameManager GM;

    private void Awake()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            GM.setPier(dockingPos);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //GM.setPier(null);
        }
    }
}