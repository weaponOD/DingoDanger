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

    private void Awake()
    {
        // Subscribe to game state
        GameState.buildModeChanged += SetBuildMode;

        playCam = playCamGO.GetComponentInChildren<PlayModeCam>();
        buildCam = buildCamGO.GetComponentInChildren<BuildModeCam>();
    }

    private void SetBuildMode(bool isBuildMode)
    {
        playCamGO.SetActive(!isBuildMode);
        buildCamGO.SetActive(isBuildMode);
        buildCam.UpdatePos();
    }

    public void MoveBuildCameraToPier(Transform _pier)
    {
        buildCamGO.transform.position = _pier.position;
    }

    public BuildModeCam BuildCam
    {
        get { return buildCam; }
    }

    private void OnDestroy()
    {
        // Unsubscribe to game state
        GameState.buildModeChanged -= SetBuildMode;
    }
}