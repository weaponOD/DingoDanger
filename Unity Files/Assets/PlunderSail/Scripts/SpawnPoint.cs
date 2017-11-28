using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SphereCollider))]
public class SpawnPoint : MonoBehaviour
{
    public bool isOpen = true;

    private void Awake()
    {
        GetComponent<SphereCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        isOpen = false;
    }

    private void OnTriggerExit(Collider other)
    {
        isOpen = true;
    }
}