using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildModeCam : MonoBehaviour
{

    // Adjustable Variables
    [Header("Right Stick Rotation")]

    [SerializeField]
    [Tooltip("How much the camera will move when the mouse moves")]
    [Range(1, 20)]
    private float mouseSensitivity = 2.5f;

    [SerializeField]
    [Tooltip("How long it takes for the camera to reach it's destination")]
    private float orbitDampening = 10f;

    [SerializeField]
    [Tooltip("Highest angle the camera can go from the horizon")]
    private float MaxClamp = 80f;

    [SerializeField]
    [Tooltip("Lowest angle the camera can go to the horizon")]
    private float minclamp = 5f;

    [SerializeField]
    [Tooltip("How much the camera will zoom when the mouse moves")]
    [Range(0, 20)]
    private float zoomSensitivity = 0.2f;

    [SerializeField]
    [Tooltip("How long it takes for the camera to reach it's zoom level")]
    private float zoomDampening = 12f;

    [SerializeField]
    private float minZoomDistance = 4f;

    [SerializeField]
    private float maxZoomDistance = 25f;

    private bool invertedX = false;

    private bool invertedY = false;

    // Camera Rotation Variables
    private float cameraDistance;

    private Vector3 localRotation;

    // Camera Pivot Point
    private Transform pivot;

    private void Awake()
    {
        pivot = transform.parent;
    }

    private void Start()
    {
        localRotation = new Vector3(pivot.rotation.eulerAngles.y, pivot.rotation.eulerAngles.x, 0f);

        cameraDistance = transform.localPosition.z * -1f;
    }

    private void Update()
    {
        // Rotate the camera based on right thumb stick input
        if (Input.GetAxis("Mouse_X") != 0 || Input.GetAxis("Mouse_Y") != 0)
        {
            if (invertedX)
            {
                localRotation.x += Input.GetAxis("Mouse_X") * mouseSensitivity; 
            }
            else
            {
                localRotation.x -= Input.GetAxis("Mouse_X") * mouseSensitivity;
            }

            if (invertedY)
            {
                localRotation.y += Input.GetAxis("Mouse_Y") * mouseSensitivity;
            }
            else
            {
                localRotation.y -= Input.GetAxis("Mouse_Y") * mouseSensitivity;
            }

            // Clamp the Y rotation to horizon and not flipping it over at the top
            localRotation.y = Mathf.Clamp(localRotation.y, minclamp, MaxClamp);
        }

        // Zoom input from our mouse scroll wheel
        if (Input.GetAxis("Right_Trigger") != 0f)
        {
            float zoomAmount = Input.GetAxis("Right_Trigger") * -1f * zoomSensitivity;

            // Makes camera zoom faster the further away it is from the target
            zoomAmount *= (cameraDistance * 0.3f);

            cameraDistance += zoomAmount * -1f;

            // Zoom no closer than 1.5 units and no futher than 100 units away
            cameraDistance = Mathf.Clamp(cameraDistance, minZoomDistance, maxZoomDistance);
        }

        // Zoom input from our mouse scroll wheel
        if (Input.GetAxis("Left_Trigger") != 0f)
        {
            float zoomAmount = Input.GetAxis("Left_Trigger") * zoomSensitivity;

            // Makes camera zoom faster the further away it is from the target
            zoomAmount *= (cameraDistance * 0.3f);

            cameraDistance += zoomAmount * -1f;

            // Zoom no closer than 1.5 units and no futher than 100 units away
            cameraDistance = Mathf.Clamp(cameraDistance, minZoomDistance, maxZoomDistance);
        }
    }

    private void LateUpdate()
    {
        // Camera Rotation using the right thumb stick
        Quaternion targetRotation = Quaternion.Euler(localRotation.y, localRotation.x, 0f);
        pivot.rotation = Quaternion.Lerp(pivot.rotation, targetRotation, Time.deltaTime * orbitDampening);

        if (transform.localPosition.z != cameraDistance * -1f)
        {
            transform.localPosition = new Vector3(0f, 0f, Mathf.Lerp(transform.localPosition.z, cameraDistance * -1f, Time.deltaTime * zoomDampening));
        }
    }

    public bool InvertX
    {
        get { return invertedX; }
        set { invertedX = value; }
    }

    public bool InvertY
    {
        get { return invertedY; }
        set { invertedY = value; }
    }

    public void MoveToPoint(Transform _target)
    {
        pivot.position = _target.position;
    }
}