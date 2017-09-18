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
    [Tooltip("How long it takes for the camera to reach it's zoom level")]
    private float zoomDampening = 12f;

    private bool buildMode = false;

    private void Start()
    {
        cameraT = this.transform;
        pivotT = this.transform.parent;
    }

    private void Update()
    {
        if (Input.GetAxis("Mouse_X") != 0 || Input.GetAxis("Mouse_Y") != 0)
        {
            localRotation.x -= Input.GetAxis("Mouse_X") * mouseSensitivity;
            localRotation.y += Input.GetAxis("Mouse_Y") * mouseSensitivity;


            // Clamp the Y rotation to horizon and not flipping it over at the top
            localRotation.y = Mathf.Clamp(localRotation.y, 0f, 90f);
        }

        // Zoom input from our mouse scroll wheel
        if (Input.GetAxis("Left_Bumper") != 0f)
        {
            float zoomAmount = Input.GetAxis("Left_Bumper") * -1f * zoomSensitivity;

            // Makes camera zoom faster the further away it is from the target
            zoomAmount *= (cameraDistance * 0.3f);

            cameraDistance += zoomAmount * -1f;

            // Zoom no closer than 1.5 units and no futher than 100 units away
            cameraDistance = Mathf.Clamp(cameraDistance, minZoomDistance, maxZoomDistance);
        }

        // Zoom input from our mouse scroll wheel
        if (Input.GetAxis("Right_Bumper") != 0f)
        {
            float zoomAmount = Input.GetAxis("Right_Bumper") * zoomSensitivity;

            // Makes camera zoom faster the further away it is from the target
            zoomAmount *= (cameraDistance * 0.3f);

            cameraDistance += zoomAmount * -1f;

            // Zoom no closer than 1.5 units and no futher than 100 units away
            cameraDistance = Mathf.Clamp(cameraDistance, minZoomDistance, maxZoomDistance);
        }
    }

    private void LateUpdate()
    {
        Quaternion targetRotation = Quaternion.Euler(localRotation.y, localRotation.x, 0f);
        pivotT.rotation = Quaternion.Lerp(pivotT.rotation, targetRotation, Time.deltaTime * orbitDampening);


        if (cameraT.localPosition.z != cameraDistance * -1f)
        {
            cameraT.localPosition = new Vector3(0f, 0f, Mathf.Lerp(cameraT.localPosition.z, cameraDistance * -1f, Time.deltaTime * zoomDampening));
        }
    }
}