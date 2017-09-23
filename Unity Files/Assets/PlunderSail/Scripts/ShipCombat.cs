using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCombat : MonoBehaviour
{
    private float reloadTime = 2f;
    private float nextReloadTimeRight = 0;
    private float nextReloadTimeLeft = 0;

    private bool canShootRight = true;
    private bool canShootLeft = true;

    private List<AttachmentWeapon> leftWeapons;
    private List<AttachmentWeapon> rightWeapons;

    private AttachmentWeapon[]  weapons;

    private void Awake()
    {
        leftWeapons = new List<AttachmentWeapon>();
        rightWeapons = new List<AttachmentWeapon>();
        updateWeapons();
    }

    public void updateWeapons()
    {
        weapons = GetComponentsInChildren<AttachmentWeapon>();

        foreach(AttachmentWeapon weapon in weapons)
        {
            if(!weapon.DoubleFacing)
            {
                if(weapon.FacingLeft)
                {
                    leftWeapons.Add(weapon);
                }
                else
                {
                    rightWeapons.Add(weapon);
                }
            }
            else
            {
                leftWeapons.Add(weapon);
                rightWeapons.Add(weapon);
            }
        }
    }

    private void Update()
    {
        if(Time.time > nextReloadTimeRight)
        {
            canShootRight = true;
        }

        if (Time.time > nextReloadTimeLeft)
        {
            canShootLeft = true;
        }

        if (Input.GetAxis("Left_Trigger") == 1)
        {
            Debug.Log("Trigger Down");

            if (canShootLeft)
            {
                foreach (AttachmentWeapon weapon in leftWeapons)
                {
                    weapon.FireLeft();
                }

                canShootLeft = false;
                nextReloadTimeLeft = Time.time + reloadTime;
            }
        }

        if (Input.GetAxis("Right_Trigger") == 1)
        {

            Debug.Log("Trigger Down");

            if (canShootRight)
            {
                foreach (AttachmentWeapon weapon in rightWeapons)
                {
                    weapon.FireRight();
                }

                canShootRight = false;
                nextReloadTimeRight = Time.time + reloadTime;
            }
        }
    }
}