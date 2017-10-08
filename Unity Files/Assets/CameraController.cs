using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    GameObject buildCam;

    [SerializeField]
    GameObject playCam;

    private void Awake()
    {
        // Subscribe to game state
        GameState.buildModeChanged += SetBuildMode;

        //playCam = GetComponent<PlayModeCam>();
        //buildCam = GetComponent<BuildModeCam>();
    }

    private void SetBuildMode(bool isBuildMode)
    {
        playCam.SetActive(!isBuildMode);
        buildCam.SetActive(isBuildMode);
    }

    private void OnDestroy()
    {
        // Unsubscribe to game state
        GameState.buildModeChanged -= SetBuildMode;
    }
}