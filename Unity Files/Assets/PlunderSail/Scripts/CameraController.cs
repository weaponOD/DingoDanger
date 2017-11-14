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

        InvertX(true);
        InvertY(true);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            inverted = !inverted;

            InvertX(inverted);
            InvertY(inverted);
        }

        if(aiming)
        {
            if(aimLeft)
            {
                Debug.Log("Aiming left");
                AimLeft();
            }
            else
            {
                Debug.Log("Aiming right");
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

    public void InvertX(bool _isInverted)
    {
        playCam.InvertX = _isInverted;
        buildCam.InvertX = _isInverted;
    }

    public void InvertY(bool _isInverted)
    {
        playCam.InvertY = _isInverted;
        buildCam.InvertY = _isInverted;
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

        if(angle > 12 && angle < 168)
        {
            AimRight();
            aiming = true;
            aimLeft = false;
            return "right";
        }

        if(angle > 192 && angle < 348)
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