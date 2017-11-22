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
    protected string ammoType = "";

    [SerializeField]
    protected string shootSound;

    protected Player player = null;

    protected Transform[] firePoints = null;

    protected bool canFire = true;

    protected int currentCannon = 0;

    protected Pool projectilePool = null;

    protected virtual void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    protected override void Start()
    {
        base.Start();
        projectilePool = ResourceManager.instance.getPool(ammoType);
    }

    protected virtual void Fire()
    {
        
    }

    public override void TakeDamage(float damgage)
    {
        currentHealth -= damgage;

        if (currentHealth <= 0 && !dead)
        {
            Die();
        }
    }

    protected void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.75F);
        Gizmos.DrawWireSphere(transform.position, awarenessRange);

        Gizmos.color = new Color(0, 0, 0, 1);
        Gizmos.DrawWireSphere(transform.position, minimumRange);
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