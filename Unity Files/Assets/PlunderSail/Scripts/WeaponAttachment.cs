using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttachment : AttachmentBase
{
    [SerializeField]
    protected float minFireTime = 0;

    [SerializeField]
    protected float maxFireTime = 0;

    [SerializeField]
    private string ammoType = "";

    [SerializeField]
    protected float damage;

    [SerializeField]
    protected float projectileForce;

    [SerializeField]
    protected int numberOfFirePoints = 0;

    [SerializeField]
    protected bool canAim = false;

    [Header("Effects resources")]
    [SerializeField]
    protected string shootSound;

    [SerializeField]
    protected string fireEffect = "";

    [SerializeField]
    protected Transform[] firePoints;

    protected Transform[] effectPoints;

    protected bool facingLeft;

    protected Vector3 shipVelocity;

    protected int pointCount = 0;

    private Pool projectilePool = null;

    private Pool fireEffectPool = null;

    protected override void Awake()
    {
        base.Awake();

        projectilePool = ResourceManager.instance.getPool(ammoType);

        fireEffectPool = ResourceManager.instance.getPool(fireEffect);

        firePoints = new Transform[numberOfFirePoints];

        foreach (Transform child in transform.GetChild(0))
        {
            if (child.CompareTag("FirePoint"))
            {
                firePoints[pointCount] = child;
                pointCount++;
            }
        }
    }

    public void Aim(Vector3 _target)
    {
        if (!canAim)
            return;

        foreach (Transform firePoint in firePoints)
        {
            firePoint.LookAt(new Vector3(_target.x, _target.y + 1, _target.z));
            firePoint.localEulerAngles = new Vector3(firePoint.localEulerAngles.x, 270f, 0f);
        }
    }

    public bool FacingLeft
    {
        get
        {
            facingLeft = (transform.localEulerAngles.y < 95f);
            return facingLeft;
        }
        set { facingLeft = value; }
    }

    public void FireWeapon()
    {
        foreach (Transform firePoint in firePoints)
        {
            StartCoroutine(Fire(firePoint));
        }
    }

    protected virtual IEnumerator Fire(Transform _firePoint)
    {
        yield return new WaitForSeconds(Random.Range(minFireTime, maxFireTime));

        shipVelocity = entity.Velocity;

        GameObject projectile = projectilePool.getPooledObject();

        if (projectile != null)
        {
            projectile.transform.position = _firePoint.position;
            projectile.transform.rotation = _firePoint.rotation;

            projectile.SetActive(true);

            Projectile shot = projectile.GetComponent<Projectile>();
            shot.Damage = damage;
            shot.FireProjectile(shipVelocity, projectileForce);

            GameObject effect = fireEffectPool.getPooledObject();

            if(effect != null)
            {
                effect.transform.position = _firePoint.position;
                effect.transform.rotation = _firePoint.rotation;

                effect.SetActive(true);

                ResourceManager.instance.DelayedDestroy(effect, effect.GetComponent<ParticleSystem>().main.startLifetime.constant);
            }
            else
            {
                Debug.LogError("fire effect from resource manager was null");
            }

            AudioManager.instance.PlaySound(shootSound);
        }
        else
        {
            Debug.LogError("Projectile from resource manager was null");
        }
    }
}