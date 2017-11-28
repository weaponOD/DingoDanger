using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Projectile : MonoBehaviour
{
    private Rigidbody rb;

    private float damage;

    [Header("Sound")]

    [SerializeField]
    private string waterImpact = "CHANGE";

    [SerializeField]
    private string woodImpact = "CHANGE";

    [SerializeField]
    private string stoneImpact = "CHANGE";

    [Header("Effects")]

    [SerializeField]
    private string waterHitEffect;

    [SerializeField]
    private string woodHitEffect;

    [SerializeField]
    private string stoneHitEffect;

    [SerializeField]
    private bool floats = false;

    private bool hasSplashed = false;

    private Pool waterHitPool = null;

    private Pool woodHitPool = null;

    private Pool stoneHitPool = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        waterHitPool = ResourceManager.instance.getPool(waterHitEffect);
        woodHitPool = ResourceManager.instance.getPool(woodHitEffect);
        stoneHitPool = ResourceManager.instance.getPool(stoneHitEffect);
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

    public void FireProjectile(Vector3 _Velocity)
    {
        rb.AddForce(_Velocity, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision _collision)
    {
        // did we collide with an attachment
        if (_collision.collider.gameObject.GetComponent<AttachmentBase>() != null)
        {

            // deflect off armour pieces
            if (_collision.collider.gameObject.GetComponent<ArmourAttachment>() != null)
            {
                // Debug.Log("Hit armoured attachment");

                AudioManager.instance.PlaySound(stoneImpact);

                GameObject hitEffect = stoneHitPool.getPooledObject();

                if (hitEffect)
                {
                    hitEffect.transform.position = transform.position;
                    hitEffect.transform.rotation = Quaternion.LookRotation(-transform.rotation.eulerAngles);

                    hitEffect.SetActive(true);
                }

                ResourceManager.instance.DelayedDestroy(hitEffect, hitEffect.GetComponent<ParticleSystem>().main.startLifetime.constant);

                rb.AddForce(-transform.forward * 2f, ForceMode.Impulse);

                _collision.collider.gameObject.GetComponent<AttachmentBase>().TakeDamage(damage);
            }
            else
            {
                // Debug.Log("Hit non-armour attachment");
                // we've hit a non-armour attachment

                AudioManager.instance.PlaySound(woodImpact);
                _collision.collider.gameObject.GetComponent<AttachmentBase>().TakeDamage(damage);

                GameObject hitEffect = woodHitPool.getPooledObject();

                if (hitEffect)
                {
                    hitEffect.transform.position = transform.position;
                    hitEffect.transform.rotation = Quaternion.LookRotation(-transform.rotation.eulerAngles);

                    hitEffect.SetActive(true);
                }

                ResourceManager.instance.DelayedDestroy(hitEffect, hitEffect.GetComponent<ParticleSystem>().main.startLifetime.constant);

                Destroy();
            }
        }
        else if (_collision.collider.gameObject.GetComponent<LivingEntity>() != null)
        {
            // we've hit a tower
            if (_collision.collider.gameObject.GetComponent<Tower>() != null)
            {
                // Debug.Log("Hit Tower");

                AudioManager.instance.PlaySound(stoneImpact);

                _collision.collider.gameObject.GetComponent<LivingEntity>().TakeDamage(damage);

                GameObject hitEffect = stoneHitPool.getPooledObject();

                hitEffect.transform.position = transform.position;
                hitEffect.transform.rotation = Quaternion.LookRotation(-transform.rotation.eulerAngles);

                hitEffect.SetActive(true);

                ResourceManager.instance.DelayedDestroy(hitEffect, hitEffect.GetComponent<ParticleSystem>().main.startLifetime.constant);

                Destroy();
            }
            else if (_collision.collider.gameObject.GetComponent<Player>() != null)
            {
                // Debug.Log("Hit Player");

                AudioManager.instance.PlaySound(woodImpact);

                _collision.collider.gameObject.GetComponent<LivingEntity>().TakeDamage(damage);

                GameObject hitEffect = woodHitPool.getPooledObject();

                hitEffect.transform.position = transform.position;
                hitEffect.transform.rotation = Quaternion.LookRotation(-transform.rotation.eulerAngles);

                hitEffect.SetActive(true);

                ResourceManager.instance.DelayedDestroy(hitEffect, hitEffect.GetComponent<ParticleSystem>().main.startLifetime.constant);

                Destroy();
            }
            else
            {
                // Debug.Log("Hit hull of ship");

                AudioManager.instance.PlaySound(woodImpact);

                // we've hit the hull of a ship 
                _collision.collider.gameObject.GetComponent<LivingEntity>().TakeDamage(damage);

                GameObject hitEffect = woodHitPool.getPooledObject();

                hitEffect.transform.position = transform.position;
                hitEffect.transform.rotation = Quaternion.LookRotation(-transform.rotation.eulerAngles);

                hitEffect.SetActive(true);

                ResourceManager.instance.DelayedDestroy(hitEffect, hitEffect.GetComponent<ParticleSystem>().main.startLifetime.constant);

                Destroy();
            }
        }
        else
        {
            // hit the environment

            // Debug.Log("Hit environment");

            AudioManager.instance.PlaySound(stoneImpact);

            GameObject hitEffect = stoneHitPool.getPooledObject();

            if (hitEffect)
            {
                hitEffect.transform.position = transform.position;
                hitEffect.transform.rotation = Quaternion.LookRotation(-transform.rotation.eulerAngles);

                hitEffect.SetActive(true);
            }

            ResourceManager.instance.DelayedDestroy(hitEffect, hitEffect.GetComponent<ParticleSystem>().main.startLifetime.constant);

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
        if (transform.position.y < 0 && inMap())
        {
            if (!hasSplashed)
            {
                AudioManager.instance.PlaySound(waterImpact);

                GameObject splash = waterHitPool.getPooledObject();

                splash.transform.position = transform.position;
                splash.transform.rotation = Quaternion.identity;

                splash.SetActive(true);

                ResourceManager.instance.DelayedDestroy(splash, splash.GetComponent<ParticleSystem>().main.startLifetime.constant);

                if (floats)
                {
                    GetComponent<buoyancy>().enabled = true;
                    rb.mass = 0.1f;
                    rb.drag = 10;
                    rb.useGravity = false;
                    rb.useGravity = false;
                    GetComponent<SphereCollider>().radius = 1;
                }
                else
                {
                    Invoke("Destroy", 3f);
                }

                hasSplashed = true;
            }
        }
    }

    private bool inMap()
    {
        if (transform.position.x > 0 && transform.position.x < 5534)
        {
            if (transform.position.z > 0 && transform.position.z < 4150)
            {
                return true;
            }
        }

        return false;
    }

    // De-activates the gameObject
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