using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private Rigidbody rb;

    private float damage;

    [Header("Effects")]
    [SerializeField]
    private AudioClip splashSound;

    [SerializeField]
    private ParticleSystem splashEffect;

    [SerializeField]
    private ParticleSystem hitEffect;

    private bool hasSplashed = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        rb.velocity = Vector3.zero;
    }

    public void FireProjectile(Vector3 _shipVelocity, float _initialForce)
    {
        rb.velocity = _shipVelocity;

        rb.AddForce(transform.forward * _initialForce, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision _collision)
    {
        if (_collision.collider.gameObject.GetComponent<AttachmentBase>() != null)
        {
            if (_collision.collider.gameObject.GetComponent<ArmourAttachment>() != null)
            {
                rb.AddForce(-transform.forward * 2f, ForceMode.Impulse);
                _collision.collider.gameObject.GetComponent<AttachmentBase>().TakeDamage(damage);
            }
            else
            {
                _collision.collider.gameObject.GetComponent<AttachmentBase>().TakeDamage(damage);
                Destroy(Instantiate(hitEffect.gameObject, transform.position, Quaternion.LookRotation(-transform.rotation.eulerAngles)) as GameObject, hitEffect.main.startLifetime.constant);
                Destroy();
            }
        }
        else if (_collision.collider.gameObject.GetComponent<LivingEntity>() != null)
        {
            _collision.collider.gameObject.GetComponent<LivingEntity>().TakeDamage(damage);
            Destroy(Instantiate(hitEffect.gameObject, transform.position, Quaternion.LookRotation(-transform.rotation.eulerAngles)) as GameObject, hitEffect.main.startLifetime.constant);
            Destroy();
        }
        else
        {
            Destroy();
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
            if (!hasSplashed)
            {
                AudioManager.instance.PlaySound("ImpactWater");
                Destroy(Instantiate(splashEffect.gameObject, transform.position, Quaternion.identity) as GameObject, splashEffect.main.startLifetime.constant);
                Invoke("Destroy", 3f);

                hasSplashed = true;
            }
        }
    }

    private void Destroy()
    {
        hasSplashed = false;
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }
}