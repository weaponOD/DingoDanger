using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script component that will smoothly rotate the camera around camera Pivot
/// </summary>
/// 
public class CameraOrbit : MonoBehaviour
{
    // Varibles

    private Transform cameraT;
    
    private Transform pivotT;

    private Transform player;

    private float startX;

    private Vector3 localRotation;

    private float cameraDistance = 10f;

    // How much the camera will move when the mouse moves
    [SerializeField]
    [Tooltip("How much the camera will move when the mouse moves")]
    [Range(1, 20)]
    private float mouseSensitivity = 2.5f;

    [SerializeField]
    [Tooltip("How much the camera will zoom when the mouse moves")]
    [Range(0, 20)]
    private float zoomSensitivity = 0.2f;

    [SerializeField]
    private float minZoomDistance = 4f;

    [SerializeField]
    private float maxZoomDistance = 25f;

    // How long it takes for the camera to reach it's destination.
    [SerializeField]
    [Tooltip("How long it takes for the camera to reach it's destination")]
    private float orbitDampening = 10f;

    [SerializeField]
    [Tooltip("How long it takes for the camera to reach it's destination")]
    private float BuildorbitDampening = 10f;

    [SerializeField]
    [Tooltip("How long it takes for the camera to reach it's zoom level")]
    private float zoomDampening = 12f;

    [SerializeField]
    [Tooltip("Highest angle the camera can go from the horizon")]
    float MaxClamp;

    [SerializeField]
    [Tooltip("Lowest angle the camera can go to the horizon")]
    float minclamp;

    [SerializeField]
    [Tooltip("angle the camera can rotate around the player while sailing")]
    float xAxisClamp;

    [SerializeField]
    private bool buildMode = false;

    // Alignment and snap Varibles
    private bool snapIsDelayed = true;

    private float timeBeforeSnap = 1.5f;

    private float timeToSnapBack;

    private void Start()
    {
        cameraT = this.transform;
        pivotT = this.transform.parent;

        startX = pivotT.rotation.eulerAngles.x;

        localRotation = new Vector3(pivotT.rotation.eulerAngles.y, pivotT.rotation.eulerAngles.x, 0f);

        cameraDistance = cameraT.localPosition.z * -1f;

        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Subscribe to game state
        GameState.buildModeChanged += SetBuildMode;
    }

    private void Update()
    {
        // Rotate the camera based on right thumb stick input
        if (Input.GetAxis("Mouse_X") != 0 || Input.GetAxis("Mouse_Y") != 0)
        {
            localRotation.x -= Input.GetAxis("Mouse_X") * mouseSensitivity;
            localRotation.y += Input.GetAxis("Mouse_Y") * mouseSensitivity;


            // Clamp the Y rotation to horizon and not flipping it over at the top
            localRotation.y = Mathf.Clamp(localRotation.y, minclamp, MaxClamp);
        }

        // Zoom input from our mouse scroll wheel
        if (Input.GetAxis("Right_Bumper") != 0f)
        {
            float zoomAmount = Input.GetAxis("Right_Bumper") * -1f * zoomSensitivity;

            // Makes camera zoom faster the further away it is from the target
            zoomAmount *= (cameraDistance * 0.3f);

            cameraDistance += zoomAmount * -1f;

            // Zoom no closer than 1.5 units and no futher than 100 units away
            cameraDistance = Mathf.Clamp(cameraDistance, minZoomDistance, maxZoomDistance);
        }

        // Zoom input from our mouse scroll wheel
        if (Input.GetAxis("Left_Bumper") != 0f)
        {
            float zoomAmount = Input.GetAxis("Left_Bumper") * zoomSensitivity;

            // Makes camera zoom faster the further away it is from the target
            zoomAmount *= (cameraDistance * 0.3f);

            cameraDistance += zoomAmount * -1f;

            // Zoom no closer than 1.5 units and no futher than 100 units away
            cameraDistance = Mathf.Clamp(cameraDistance, minZoomDistance, maxZoomDistance);
        }
    }

    private void LateUpdate()
    {
        if(buildMode)
        {
            Quaternion targetRotation = Quaternion.Euler(localRotation.y, localRotation.x, 0f);
            pivotT.rotation = Quaternion.Lerp(pivotT.rotation, targetRotation, Time.deltaTime * BuildorbitDampening);
        }
        else
        {
            Vector2 stickForce = new Vector2(Input.GetAxis("Mouse_X"), Input.GetAxis("Mouse_Y"));

            // If player uses right stick break free from alignment
            if (stickForce.magnitude > 0.2f)
            {
                snapIsDelayed = false;

                localRotation.x = Mathf.Clamp(localRotation.x, player.transform.rotation.eulerAngles.y - xAxisClamp, player.transform.rotation.eulerAngles.y + xAxisClamp);

                Quaternion targetRotation = Quaternion.Euler(startX, localRotation.x, 0f);
                pivotT.rotation = Quaternion.Lerp(pivotT.rotation, targetRotation, Time.deltaTime * orbitDampening);
            }
            else
            {
                // Align to player rotation unless that rotate.

                Quaternion targetRotation = Quaternion.Euler(startX, player.transform.rotation.eulerAngles.y, 0f);

                float difference = Mathf.Abs(pivotT.rotation.eulerAngles.y - targetRotation.eulerAngles.y);

                if (difference > 1f)
                {
                    pivotT.rotation = Quaternion.Slerp(pivotT.rotation, targetRotation, Time.deltaTime * orbitDampening);
                }
            }
        }

        if (cameraT.localPosition.z != cameraDistance * -1f)
        {
            cameraT.localPosition = new Vector3(0f, 0f, Mathf.Lerp(cameraT.localPosition.z, cameraDistance * -1f, Time.deltaTime * zoomDampening));
        }
    }

    public void SetBuildMode(bool isBuildMode)
    {
        buildMode = isBuildMode;
    }

    private void OnDestroy()
    {
        // Unsubscribe to game state
        GameState.buildModeChanged -= SetBuildMode;
    }
}