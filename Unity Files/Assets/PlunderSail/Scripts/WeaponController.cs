using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    private float nextReloadTime = 0;

    private bool canShoot = true;

    [SerializeField]
    private string FireShoutSound = "";

    [SerializeField]
    private float reloadTime = 0.0f;

    private WeaponAttachment[] leftWeapons = null;

    private WeaponAttachment[] rightWeapons = null;

    public WeaponAttachment[] LeftWeapons
    {
        get { return leftWeapons; }

        set { leftWeapons = value; }
    }

    public WeaponAttachment[] RightWeapons
    {
        get { return rightWeapons; }

        set { rightWeapons = value; }
    }

    public bool HasWeapons
    {
        get { return (rightWeapons.Length > 0 && leftWeapons.Length > 0); }
    }

    private void Update()
    {
        if (!GameState.BuildMode)
        {
            if (Time.time > nextReloadTime)
            {
                canShoot = true;
            }

            if (Time.time > nextReloadTime)
            {
                canShoot = true;
            }
        }
    }

    public void FireWeaponsRight()
    {
        if (canShoot)
        {
            if (rightWeapons.Length > 0)
            {
                AudioManager.instance.PlaySound(FireShoutSound);

                foreach (WeaponAttachment weapon in rightWeapons)
                {
                    if (weapon != null)
                    {
                        weapon.FireWeapon();
                    }
                }

                canShoot = false;
                nextReloadTime = Time.time + reloadTime;
            }
        }
    }

    public void FireWeaponsLeft()
    {
        if (canShoot)
        {
            if (leftWeapons.Length > 0)
            {
                AudioManager.instance.PlaySound(FireShoutSound);

                foreach (WeaponAttachment weapon in leftWeapons)
                {
                    if (weapon != null)
                    {
                        weapon.FireWeapon();
                    }
                }

                canShoot = false;
                nextReloadTime = Time.time + reloadTime;
            }
        }
    }
}