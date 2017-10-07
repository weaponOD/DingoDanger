using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentManager : MonoBehaviour
{
    private AttachmentSail[] sails;

    private AttachmentWeapon[] leftWeapons;
    private AttachmentWeapon[] rightWeapons;

    private void Awake()
    {
        // Subscribe to game state
        GameState.buildModeChanged += UpdateParts;
    }

    public AttachmentWeapon[] GetAttachedLeftWeapons()
    {
        return leftWeapons;
    }

    public AttachmentWeapon[] GetAttachedRightWeapons()
    {
        return rightWeapons;
    }

    public AttachmentSail[] GetAttachedSails()
    {
        return sails;
    }

    // returns total bonus from all sails.
    public int getSpeedBonus()
    {
        return sails.Length;
    }

    private void UpdateParts(bool isBuildMode)
    {
        if (!isBuildMode)
        {
            UpdateWeapons();
            UpdateSails();
        }
    }

    private void UpdateSails()
    {
        sails = GetComponentsInChildren<AttachmentSail>();
    }

    private void UpdateWeapons()
    {
        AttachmentWeapon[] weapons = GetComponentsInChildren<AttachmentWeapon>();

        int weaponsRightCount = 0;
        int weaponsLeftCount = 0;

        foreach (AttachmentWeapon weapon in weapons)
        {
            if (weapon.FacingLeft)
            {
                weaponsLeftCount++;
            }
            else
            {
                weaponsRightCount++;
            }
        }

        leftWeapons = new AttachmentWeapon[weaponsLeftCount];
        rightWeapons = new AttachmentWeapon[weaponsRightCount];

        weaponsRightCount = 0;
        weaponsLeftCount = 0;

        foreach (AttachmentWeapon weapon in weapons)
        {
            if (weapon.FacingLeft)
            {
                leftWeapons[weaponsLeftCount] = weapon;
                weaponsLeftCount++;
            }
            else
            {
                rightWeapons[weaponsRightCount] = weapon;
                weaponsRightCount++;
            }
        }
    }
}