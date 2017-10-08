using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private Rigidbody rb;
    private float initialForce = 10;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        initialForce += Random.Range(6, 8);

        rb.AddForce(transform.forward * initialForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision _collision)
    {
        Debug.Log("Collide!");

        if(_collision.collider.gameObject.GetComponent<AttachmentBase>() != null)
        {
           _collision.collider.gameObject.GetComponent<AttachmentBase>().TakeDamage(100);
        }

        //GameObject.Destroy(gameObject);
    }

    private void Update()
    {
        if(transform.position.y < -5)
        {
            GameObject.Destroy(gameObject);
        }
    }
}