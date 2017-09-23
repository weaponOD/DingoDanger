using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // References
    private CameraMovement cameraMovement;

    private CameraOrbit cameraOrbit;

    private ShipMovement movement;

    private buoyancy bobbing;

    private AttachmentPoint[] attachmentPoints;

    private int gold = 300;

    private bool buildMode = false;

    private void Awake()
    {
        movement = GetComponent<ShipMovement>();

        GameObject cameraPivot = GameObject.FindGameObjectWithTag("CameraPivot");

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