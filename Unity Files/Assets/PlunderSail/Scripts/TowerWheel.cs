using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerWheel : TowerBase
{
    [SerializeField]
    protected Transform wheel = null;

    protected override void Awake()
    {
        base.Awake();

        firePoints = new Transform[4];

        for (int point = 0; point < 4; point++)
        {
            firePoints[point] = wheel.GetChild(point).transform;
        }
    }

    protected override void Fire()
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

        GameObject projectile = projectilePool.getPooledObject();

        if (projectile != null)
        {
            projectile.transform.position = firePoints[0].position;
            projectile.transform.rotation = firePoints[0].rotation;

            projectile.SetActive(true);

            Projectile shot = projectile.GetComponent<Projectile>();
            shot.Damage = damage;

            shot.FireProjectile(new Vector3(), projectileForce);

            AudioManager.instance.PlaySound(shootSound);
        }
        else
        {
            Debug.LogError("Projectile from resource manager was null");
        }

        Invoke("Reload", reloadTime);
    }


    private void Update()
    {
        float distToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distToPlayer < awarenessRange && distToPlayer > minimumRange)
        {
            wheel.LookAt(player.transform.position);

            wheel.localEulerAngles = new Vector3(0f, wheel.localEulerAngles.y, 0f);

            firePoints[0].LookAt(new Vector3(player.transform.position.x, player.transform.position.y + yAxisOffset, player.transform.position.z));

            if (canFire)
            {
                Fire();
            }
        }
    }
}