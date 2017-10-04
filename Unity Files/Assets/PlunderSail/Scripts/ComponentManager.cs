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

    private void UpdateParts(bool isBuildMode)
    {
        if(!isBuildMode)
        {
            UpdateSails();
        }
    }

    private void UpdateSails()
    {
        sails = GetComponentsInChildren<AttachmentSail>();
    }

    private void UpdateWeapons()
    {
        
    }
}