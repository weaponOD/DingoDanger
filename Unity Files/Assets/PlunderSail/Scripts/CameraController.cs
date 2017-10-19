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

    private PlayModeCam playCam;

    private Transform playerCentre;

    private void Awake()
    {
        playCam = playCamGO.GetComponentInChildren<PlayModeCam>();
        buildCam = buildCamGO.GetComponentInChildren<BuildModeCam>();
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
        if(_pier)
        {
            buildCamGO.transform.position = _pier.position;
        }
    }

    public Transform PlayerCentre
    {
        set { playerCentre = value; }
    }

    public BuildModeCam BuildCam
    {
        get { return buildCam; }
    }
}