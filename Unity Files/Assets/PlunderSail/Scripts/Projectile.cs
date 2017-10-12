using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private Rigidbody rb;
    private float initialForce = 10;

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip splashSound;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
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
           _collision.collider.gameObject.GetComponent<AttachmentBase>().TakeDamage(25);
        }

        if (_collision.collider.gameObject.GetComponent<LivingEntity>() != null)
        {
            _collision.collider.gameObject.GetComponent<LivingEntity>().TakeDamage(25);
        }

        GameObject.Destroy(gameObject);
    }

    private void Update()
    {
        if(transform.position.y < 0)
        {
            audioSource.PlayOneShot(splashSound, 5f);
            GameObject.Destroy(gameObject);
        }
    }
}