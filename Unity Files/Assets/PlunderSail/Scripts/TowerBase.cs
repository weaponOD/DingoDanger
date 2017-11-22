using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : LivingEntity
{

    [Header("Tower Stats")]

    [Header("Range Values")]
    [SerializeField]
    protected float awarenessRange = 0;

    [SerializeField]
    protected float minimumRange = 0;

    [Header("Combat stats")]

    [SerializeField]
    protected float damage = 0;

    [SerializeField]
    protected float reloadTime = 0.0f;

    [SerializeField]
    protected float projectileForce = 0;

    [SerializeField]
    protected int numFirePoints = 0;

    [SerializeField]
    protected GameObject projectilePrefab = null;

    protected Player player = null;

    protected Transform[] firePoints = null;

    protected bool canFire = true;

    protected int currentCannon = 0;

    protected virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    protected void Fire()
    {
        canFire = false;

        if (currentCannon < 3)
        {
            currentCannon++;
        }
        else
        {
            currentCannon = 0;
        }

        Projectile shot = Instantiate(projectilePrefab, firePoints[0].position, firePoints[0].rotation).GetComponent<Projectile>();
        shot.Damage = damage;
        shot.FireProjectile(new Vector3(), projectileForce);

        Invoke("Reload", reloadTime);
    }

    public override void TakeDamage(float damgage)
    {
        currentHealth -= damgage;

        if (currentHealth <= 0 && !dead)
        {
            Die();
        }
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.75F);
        Gizmos.DrawWireSphere(transform.position, awarenessRange);
    }

    [ContextMenu("Self Destruct")]
    protected override void Die()
    {
        base.Die();

        Destroy(gameObject);
    }

    protected void Reload()
    {
        canFire = true;
    }
}