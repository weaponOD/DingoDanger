using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CameraMovement cameraMovement;

    private CameraOrbit cameraOrbit;

    private ShipMovement movement;

    private buoyancy bobbing;

    [SerializeField]
    private bool buildMode = true;

    private void Awake()
    {
        movement = GetComponent<ShipMovement>();

        GameObject cameraPivot = GameObject.FindGameObjectWithTag("CameraPivot");

        bobbing = GetComponentInChildren<buoyancy>();

        cameraMovement = cameraPivot.GetComponentInChildren<CameraMovement>();
        cameraOrbit = cameraPivot.GetComponentInChildren<CameraOrbit>();
    }
	
	void Update ()
    {
		if(Input.GetButtonDown("Y_Button"))
        {
            SetBuildMode(!buildMode);
        }
	}

    public void SetBuildMode(bool isBuildMode)
    {
        if(isBuildMode != buildMode)
        {
            buildMode = isBuildMode;

            if(buildMode)
            {
                movement.enabled = false;
                bobbing.WaveHeight = 0.1f;
                cameraOrbit.BuildMode = true;
                cameraMovement.BuildMode = true;
            }
            else
            {
                movement.enabled = true;
                cameraOrbit.BuildMode = false;
                cameraMovement.BuildMode = false;
                bobbing.ResetWaveHeight();
            }
        } 
    }
}