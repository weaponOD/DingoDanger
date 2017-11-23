using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentBase : MonoBehaviour
{
    [Header("Base Stat")]
    [SerializeField]
    protected float maxHealth;

    [SerializeField]
    protected float healthWhenBroken = 0.5f;

    [SerializeField]
    protected Mesh brokenMesh = null;

    [SerializeField]
    protected float healthWhenSmoking = 0.25f;

    [SerializeField]
    protected string smokeEffect = "attachment Smoke here";

    protected MeshFilter filter = null;

    protected float currentHealth;

    protected bool broken = false;

    protected bool smoking = false;

    protected LivingEntity entity;

    protected Pool smokePool;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;

        filter = GetComponentInChildren<MeshFilter>();

        entity = transform.root.GetComponent<LivingEntity>();
    }

    protected virtual void Start()
    {
        smokePool = ResourceManager.instance.getPool(smokeEffect);
    }

    public virtual void TakeDamage(float _damage)
    {
        currentHealth -= _damage;

        // switch to broken mesh
        if (currentHealth < (maxHealth * healthWhenBroken))
        {
            if (brokenMesh != null && !broken)
            {
                filter.mesh = brokenMesh;
                broken = true;
            }
        }

        //if (currentHealth < (maxHealth * healthWhenSmoking) && !smoking)
        //{
        //    if(smokePool != null)
        //    {
        //        Transform smokeEffect = smokePool.getPooledObject().transform;

        //        if (smokeEffect != null)
        //        {
        //            smokeEffect.position = transform.position;
        //            smokeEffect.rotation = Quaternion.identity;

        //            smoking = true;
        //            smokeEffect.parent = transform;
        //            smokeEffect.gameObject.SetActive(true);
        //        }
        //    }
        //}

        // destroyed 
        if (currentHealth <= 0)
        {
            transform.parent = null;

            entity.UpdateParts();

            if (!GetComponent<Rigidbody>())
            {
                gameObject.AddComponent<Rigidbody>();
                gameObject.GetComponent<Rigidbody>().AddExplosionForce(100, transform.position, 0.1f);
            }

            Destroy(gameObject, 3f);
        }
    }

    public void RestoreHealth()
    {
        broken = false;
        smoking = false;
        currentHealth = maxHealth;
    }
}