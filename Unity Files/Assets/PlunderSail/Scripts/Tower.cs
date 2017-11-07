using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : LivingEntity
{
    [SerializeField]
    protected float awarenessRange = 0;

    [SerializeField]
    protected float damage = 0;

    [SerializeField]
    protected float projectileForce = 0;

    [SerializeField]
    protected Transform wheel = null;

    [SerializeField]
    protected GameObject projectilePrefab = null;

    protected Player player = null;

    protected Transform[] firePoints = null;

    protected bool canFire = true;

    protected int currentCannon = 0;

    protected void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        firePoints = new Transform[4];

        firePoints[0] = transform.GetChild(0).transform.GetChild(1).GetChild(0).transform;
        firePoints[1] = transform.GetChild(0).transform.GetChild(1).GetChild(1).transform;
        firePoints[1] = transform.GetChild(0).transform.GetChild(1).GetChild(2).transform;
        firePoints[1] = transform.GetChild(0).transform.GetChild(1).GetChild(3).transform;
    }

    private void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < awarenessRange)
        {
            wheel.LookAt(player.transform.position);

            firePoints[0].LookAt(player.transform.position);

            if (canFire)
            {
                Fire();
            }
        }
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

        Invoke("Reload", leadReloadTime);
    }

    public override void TakeDamage(float damgage)
    {
        Debug.Log("Tower Took  " + damgage + "Damage");

        currentHealth -= damgage;

        if (currentHealth <= 0 && !dead)
        {
            Die();
        }
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