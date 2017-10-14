using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComponentManager : MonoBehaviour
{
    private AttachmentSail[] sails;

    private WeaponAttachment[] leftWeapons;
    private WeaponAttachment[] rightWeapons;

    private void Awake()
    {
        // Subscribe to game state
        GameState.buildModeChanged += UpdateParts;

        sails = new AttachmentSail[0];

        UpdateParts(false);
    }

    public WeaponAttachment[] GetAttachedLeftWeapons()
    {
        UpdateWeapons();

        return leftWeapons;
    }

    public WeaponAttachment[] GetAttachedRightWeapons()
    {
        UpdateWeapons();

        return rightWeapons;
    }

    public AttachmentSail[] GetAttachedSails()
    {
        return sails;
    }

    public void LowerSails()
    {
        UpdateSails();

        foreach(AttachmentSail sail in sails)
        {
            sail.Lower();
        }
    }

    public void RaiseSails()
    {
        UpdateSails();

        foreach (AttachmentSail sail in sails)
        {
            sail.Raise();
        }
    }

    // returns total bonus from all sails.
    public int getSpeedBonus()
    {
        UpdateSails();
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
        WeaponAttachment[] weapons = GetComponentsInChildren<WeaponAttachment>();

        int weaponsRightCount = 0;
        int weaponsLeftCount = 0;

        foreach (WeaponAttachment weapon in weapons)
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

        leftWeapons = new WeaponAttachment[weaponsLeftCount];
        rightWeapons = new WeaponAttachment[weaponsRightCount];

        weaponsRightCount = 0;
        weaponsLeftCount = 0;

        foreach (WeaponAttachment weapon in weapons)
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

    private void OnDestroy()
    {
        // Unsubscribe to game state
        GameState.buildModeChanged -= UpdateParts;
    }
}