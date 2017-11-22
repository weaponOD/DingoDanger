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

    protected LaunchMesh arc;

    protected Vector3 shipVelocity;

    protected int pointCount = 0;

    private Pool projectilePool = null;

    private Pool fireEffectPool = null;

    protected override void Awake()
    {
        base.Awake();

        firePoints = new Transform[numberOfFirePoints];

        if (canAim)
        {
            arc = GetComponentInChildren<LaunchMesh>();

            arc.gameObject.SetActive(false);
        }

        foreach (Transform child in transform.GetChild(0))
        {
            if (child.CompareTag("FirePoint"))
            {
                firePoints[pointCount] = child;
                pointCount++;
            }
        }
    }

    protected override void Start()
    {
        base.Start();

        projectilePool = ResourceManager.instance.getPool(ammoType);

        fireEffectPool = ResourceManager.instance.getPool(fireEffect);
    }

    public void UpdateRange(float _velocity)
    {
        if (!canAim)
            return;

        arc.Velocity = _velocity;
    }

    public void Aim(float _velocity)
    {
        if (!canAim)
            return;

        arc.gameObject.SetActive(true);
        arc.Velocity = _velocity;
    }

    public void CancelAim()
    {
        if (!canAim)
            return;

        arc.gameObject.SetActive(false);
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
            //projectile.transform.rotation = _firePoint.rotation;

            projectile.SetActive(true);

            Projectile shot = projectile.GetComponent<Projectile>();
            shot.Damage = damage;

            if (canAim)
            {
                Vector3 shotVelocity = (transform.GetChild(1).forward * projectileForce + Vector3.up * 0.7f) * arc.Velocity;

                shot.FireProjectile(shotVelocity);
            }
            else
            {
                shot.FireProjectile(shipVelocity, projectileForce);
            }

            GameObject effect = null; //fireEffectPool.getPooledObject();

            if (effect != null)
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