using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // References
    private CameraMovement cameraMovement;

    private CameraOrbit cameraOrbit;

    private ShipMovement movement;

    private ShipCombat combat;

    private buoyancy bobbing;

    private AttachmentPoint[] attachmentPoints;

    private AttachmentSail[] sails;

    private int gold = 900;

    private void Awake()
    {
        movement = GetComponent<ShipMovement>();

        GameObject cameraPivot = GameObject.FindGameObjectWithTag("CameraPivot");

        combat = GetComponent<ShipCombat>();

        bobbing = GetComponentInChildren<buoyancy>();

        attachmentPoints = GetComponentsInChildren<AttachmentPoint>();

        cameraMovement = cameraPivot.GetComponentInChildren<CameraMovement>();
        cameraOrbit = cameraPivot.GetComponentInChildren<CameraOrbit>();

        // Subscribe to game state
        GameState.buildModeChanged += SetBuildMode;

        foreach (AttachmentPoint point in attachmentPoints)
        {
            point.PartOne = this.transform;
        }
    }

    private void SetBuildMode(bool isBuildMode)
    {
        if (isBuildMode)
        {
            movement.enabled = false;
            bobbing.enabled = false;
            cameraOrbit.BuildMode = true;
            cameraMovement.BuildMode = true;

            foreach (AttachmentPoint point in attachmentPoints)
            {
                point.gameObject.SetActive(true);
            }
        }
        else
        {
            attachmentPoints = GetComponentsInChildren<AttachmentPoint>();

            combat.updateWeapons();

            UpdateSails();
            movement.enabled = true;
            cameraOrbit.BuildMode = false;
            cameraMovement.BuildMode = false;
            bobbing.enabled = true;

            foreach (AttachmentPoint point in attachmentPoints)
            {
                point.gameObject.SetActive(false);
            }
        }
    }

    private void UpdateSails()
    {
        sails = GetComponentsInChildren<AttachmentSail>();

        movement.ResetMoveSpeed();

        if (sails.Length > 5)
        {
            movement.MoveSpeed = 5;

            movement.MoveSpeed += (sails.Length - 5) / 2;
        }
        else
        {
            movement.MoveSpeed += sails.Length;
        }
    }

    public int Gold
    {
        get { return gold; }
    }


    public void DeductGold(int _amount)
    {
        gold -= _amount;
    }

    public void GiveGold(int _amount)
    {
        gold += _amount;
    }
}