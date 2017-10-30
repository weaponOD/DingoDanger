using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField]
    private float initialForce = 15;

    private AudioSource audioSource;

    private float damage;

    [Header("Effects")]
    [SerializeField]
    private AudioClip splashSound;

    [SerializeField]
    private ParticleSystem splashEffect;

    [SerializeField]
    private ParticleSystem hitEffect;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    public void FireProjectile(Vector3 _shipVelocity)
    {
        initialForce += Random.Range(initialForce * 0.5f, initialForce * 0.7f);

        rb.velocity = _shipVelocity;

        rb.AddForce(transform.forward * initialForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision _collision)
    {
        if (_collision.collider.gameObject.GetComponent<AttachmentBase>() != null)
        {
            if(_collision.collider.gameObject.GetComponent<ArmourAttachment>() != null)
            {
                rb.AddForce(-transform.forward * 2f, ForceMode.Impulse);
                _collision.collider.gameObject.GetComponent<AttachmentBase>().TakeDamage(damage);
            }
            else
            {
                _collision.collider.gameObject.GetComponent<AttachmentBase>().TakeDamage(damage);
                Destroy(Instantiate(hitEffect.gameObject, transform.position, Quaternion.LookRotation(-transform.rotation.eulerAngles)) as GameObject, hitEffect.main.startLifetime.constant);
                GameObject.Destroy(gameObject);
            }
        }

        if (_collision.collider.gameObject.GetComponent<LivingEntity>() != null)
        {
            _collision.collider.gameObject.GetComponent<LivingEntity>().TakeDamage(damage);
            Destroy(Instantiate(hitEffect.gameObject, transform.position, Quaternion.LookRotation(-transform.rotation.eulerAngles)) as GameObject, hitEffect.main.startLifetime.constant);
            GameObject.Destroy(gameObject);
        }
    }

    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }

    private void Update()
    {
        if (transform.position.y < 0)
        {
            audioSource.PlayOneShot(splashSound, 5f);
            Destroy(Instantiate(splashEffect.gameObject, transform.position, Quaternion.identity) as GameObject, splashEffect.main.startLifetime.constant);
            GameObject.Destroy(gameObject);
        }
    }
}