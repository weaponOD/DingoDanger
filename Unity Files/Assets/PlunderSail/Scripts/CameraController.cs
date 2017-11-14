using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private GameObject buildCamGO;

    [SerializeField]
    private GameObject playCamGO;

    private BuildModeCam buildCam;

    private PlayModeCameraTwo playCam;

    private Transform playerCentre;

    private bool inverted = false;

    private bool invertedX = true;

    private bool invertedY = true;

    private bool aiming = false;

    private bool aimLeft = false;

    private void Awake()
    {
        playCam = playCamGO.GetComponentInChildren<PlayModeCameraTwo>();
        buildCam = buildCamGO.GetComponentInChildren<BuildModeCam>();
    }

    private void Start()
    {
        MovePlayCameraToPlayer();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            inverted = !inverted;

            InvertX();
            InvertY();
        }

        if (aiming)
        {
            if (aimLeft)
            {
                AimLeft();
            }
            else
            {
                AimRight();
            }
        }
    }

    public void SwitchToBuildMode()
    {
        playCamGO.SetActive(false);
        buildCamGO.SetActive(true);
        buildCam.MoveToPoint(playerCentre);
    }

    public void SwitchToPlayMode()
    {
        playCamGO.SetActive(true);
        buildCamGO.SetActive(false);
    }

    public void MoveBuildCameraToPier(Transform _pier)
    {
        if (_pier)
        {
            buildCamGO.transform.position = _pier.position;
        }
    }

    public void InvertX()
    {
        invertedX = !invertedX;

        playCam.InvertX = invertedX;
        buildCam.InvertX = invertedX;
    }

    public void InvertY()
    {
        invertedY = !invertedY;

        playCam.InvertY = invertedY;
        buildCam.InvertY = invertedY;
    }

    private void AimRight()
    {
        playCam.AimRight();
    }

    private void AimLeft()
    {
        playCam.AimLeft();
    }

    public void CancelAim()
    {
        aiming = false;
        playCam.CancelAim();
    }

    public string TryAim()
    {
        Debug.Log("Trying to aim");

        float angle = playCam.CalculatePerspectiveAngle();

        if (angle > 12 && angle < 168)
        {
            AimRight();
            aiming = true;
            aimLeft = false;
            return "right";
        }

        if (angle > 192 && angle < 348)
        {
            AimLeft();
            aiming = true;
            aimLeft = true;
            return "left";
        }

        aiming = false;

        return "no";
    }

    public void EnableFastMode()
    {
        playCam.FastMode(true);
    }

    public void DisableFastMode()
    {
        playCam.FastMode(false);
    }

    private void MovePlayCameraToPlayer()
    {
        //buildCamGO.transform.position = playerCentre.position;
    }

    public Transform PlayerCentre
    {
        set { playerCentre = value; }
    }

    public BuildModeCam BuildCam
    {
        get { return buildCam; }
    }

    public PlayModeCameraTwo PlayCam
    {
        get { return playCam; }
    }
}