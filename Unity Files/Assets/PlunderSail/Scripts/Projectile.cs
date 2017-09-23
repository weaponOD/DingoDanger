using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private Rigidbody rb;
    private float initialForce = 8;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        initialForce += Random.Range(4, 8);

        rb.AddForce(transform.forward * initialForce, ForceMode.Impulse);
    }


    private void Update()
    {
        if(transform.position.y < -5)
        {
            GameManager.Destroy(gameObject);
        }
    }
}