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

        playCam = GetComponentInChildren<PlayModeCam>();
        buildCam = GetComponentInChildren<BuildModeCam>();
    }

    private void SetBuildMode(bool isBuildMode)
    {
        playCamGO.SetActive(!isBuildMode);
        buildCamGO.SetActive(isBuildMode);
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