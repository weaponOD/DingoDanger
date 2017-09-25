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

    private int gold = 900;

    private bool buildMode = false;

    private void Awake()
    {
        movement = GetComponent<ShipMovement>();

        GameObject cameraPivot = GameObject.FindGameObjectWithTag("CameraPivot");

        combat = GetComponent<ShipCombat>();

        bobbing = GetComponentInChildren<buoyancy>();

        attachmentPoints = GetComponentsInChildren<AttachmentPoint>();

        cameraMovement = cameraPivot.GetComponentInChildren<CameraMovement>();
        cameraOrbit = cameraPivot.GetComponentInChildren<CameraOrbit>();

        foreach (AttachmentPoint point in attachmentPoints)
        {
            point.PartOne = this.transform;
        }
    }

    private void Start()
    {
        SetBuildMode(buildMode);
    }

    public void SetBuildMode(bool isBuildMode)
    {
        buildMode = isBuildMode;

        if (buildMode)
        {
            movement.enabled = false;
            bobbing.WaveHeight = 0.1f;
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
            bobbing.ResetWaveHeight();

            foreach (AttachmentPoint point in attachmentPoints)
            {
                point.gameObject.SetActive(false);
            }
        }
    }

    private void UpdateSails()
    {

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