using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerDirectional : TowerBase
{
    [Header("Sides to fire")]
    [SerializeField]
    private bool front = true;

    [SerializeField]
    private bool right = true;

    [SerializeField]
    private bool back = true;

    [SerializeField]
    private bool left = true;

    [Header("Fire points per side")]

    [SerializeField]
    private Transform[] frontWeapons;

    [SerializeField]
    private Transform[] rightWeapons;

    [SerializeField]
    private Transform[] backWeapons;

    [SerializeField]
    private Transform[] leftWeapons;

    [SerializeField]
    private float angle;

    protected override void Awake()
    {
        base.Awake();

        // Warn designer if direction is set to true but no weapins are assigned.

        if (front && frontWeapons.Length == 0)
        {
            Debug.LogError("No front weapons assigned on this tower");
        }

        if (right && rightWeapons.Length == 0)
        {
            Debug.LogError("No right weapons assigned on this tower");
        }

        if (back && backWeapons.Length == 0)
        {
            Debug.LogError("No back weapons assigned on this tower");
        }

        if (left && leftWeapons.Length == 0)
        {
            Debug.LogError("No left weapons assigned on this tower");
        }
    }

    private void Update()
    {
        float distToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distToPlayer < awarenessRange && distToPlayer > minimumRange)
        {
            CalculateDirToPlayer();
        }
    }

    private void CalculateDirToPlayer()
    {
        if (canFire)
        {
            angle = CalculatePerspectiveAngle();

            // fire infront
            if (front && angle == 0)
            {
                FireFront();

                return;
            }

            // fire behind
            if (back && angle == 180)
            {
                FireBack();

                return;
            }

            // fire right
            if (left && angle == 90)
            {
                FireLeft();

                return;
            }

            // fire right
            if (right && angle == 270)
            {
                FireRight();

                return;
            }
        }
    }

    // convert raw angle of camera to the nearest cardinonal point
    private float CalculatePerspectiveAngle()
    {
        Vector3 vecBetween = player.transform.position - transform.position;

        Quaternion relative = Quaternion.Inverse(Quaternion.LookRotation(vecBetween));

        float rawAngle = relative.eulerAngles.y;

        float convertedAngle = Mathf.Round(rawAngle / 90) * 90;

        // when rouding to nearest 90 degrees 360 and 0 should be considered the same thing.
        if (convertedAngle >= 360)
        {
            convertedAngle = 0;
        }

        return convertedAngle;
    }

    protected void FireFront()
    {
        // Debug.Log("Firing Front");

        for (int i = 0; i < 3; i++)
        {
            frontWeapons[i].LookAt(new Vector3(player.transform.position.x, player.transform.position.y + yAxisOffset, player.transform.position.z));

            GameObject projectile = projectilePool.getPooledObject();

            if (projectile != null)
            {
                projectile.transform.position = frontWeapons[i].position;
                projectile.transform.rotation = frontWeapons[i].rotation;

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
        }

        canFire = false;
        Invoke("Reload", reloadTime);
    }

    protected void FireRight()
    {
        Debug.Log("Firing Right");

        for (int i = 0; i < 3; i++)
        {
            rightWeapons[i].LookAt(new Vector3(player.transform.position.x, player.transform.position.y + yAxisOffset, player.transform.position.z));

            GameObject projectile = projectilePool.getPooledObject();

            if (projectile != null)
            {
                projectile.transform.position = rightWeapons[i].position;
                projectile.transform.rotation = rightWeapons[i].rotation;

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
        }

        canFire = false;
        Invoke("Reload", reloadTime);
    }

    protected void FireBack()
    {
        Debug.Log("Firing Back");

        for (int i = 0; i < 3; i++)
        {
            backWeapons[i].LookAt(new Vector3(player.transform.position.x, player.transform.position.y + yAxisOffset, player.transform.position.z));

            GameObject projectile = projectilePool.getPooledObject();

            if (projectile != null)
            {
                projectile.transform.position = backWeapons[i].position;
                projectile.transform.rotation = backWeapons[i].rotation;

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
        }

        canFire = false;
        Invoke("Reload", reloadTime);
    }

    protected void FireLeft()
    {
        Debug.Log("Firing Left");

        for (int i = 0; i < 3; i++)
        {
            leftWeapons[i].LookAt(new Vector3(player.transform.position.x, player.transform.position.y + yAxisOffset, player.transform.position.z));

            GameObject projectile = projectilePool.getPooledObject();

            if (projectile != null)
            {
                projectile.transform.position = leftWeapons[i].position;
                projectile.transform.rotation = leftWeapons[i].rotation;

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
        }

        canFire = false;
        Invoke("Reload", reloadTime);
    }
}