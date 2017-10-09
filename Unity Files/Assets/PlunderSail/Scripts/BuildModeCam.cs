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

    [Header("Left Stick Movement")]

    [SerializeField]
    [Tooltip("How much the camera will move when the mouse moves")]
    [Range(1, 20)]
    private float MoveSensitivity = 2.5f;

    [SerializeField]
    [Tooltip("How long it takes for the camera to reach it's destination")]
    private float movementDampening = 10f;

    [SerializeField]
    [Tooltip("How much movement is needed to freely move the camera.")]
    [Range(0, 1)]
    float breakForce = 0.2f;

    [SerializeField]
    [Tooltip("Time in seconds of player inactivity before the camera snaps back to the target block.")]
    [Range(0, 20)]
    private float timeBeforeSnap = 1.5f;

    [SerializeField]
    private bool snapEnabled = false;

    // Camera Rotation Variables
    private float cameraDistance = 10f;

    private Transform targetBlock;

    private Transform anchor;

    private Vector3 localRotation;

    // Camera Pivot Point
    private Transform pivot;

    // if true the a timer has been set for the camera to snap back
    private bool snapIsDelayed = false;

    // The time that the camera will snap to the last block
    private float timeToSnapBack;

    // Anchor Variables

    // target location to smoothly move towards
    private Vector3 targetPosRight;
    private Vector3 targetPosForward;
    private Vector3 targetPosUp;

    private void Start()
    {
        anchor = transform.parent.GetChild(1);

        pivot = transform.parent;

        localRotation = new Vector3(pivot.rotation.eulerAngles.y, pivot.rotation.eulerAngles.x, 0f);

        cameraDistance = transform.localPosition.z * -1f;
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

        // Move anchor using left thumb stick
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {

            targetPosRight += anchor.transform.right * Input.GetAxis("Horizontal") * MoveSensitivity;

            targetPosForward += anchor.transform.forward * Input.GetAxis("Vertical") * MoveSensitivity;
        }

        // Move along y axis using Triggers
        if (Input.GetAxis("Left_Trigger") != 0 || Input.GetAxis("Right_Trigger") != 0)
        {
            targetPosUp += anchor.transform.up * Input.GetAxis("Left_Trigger") * mouseSensitivity;

            targetPosUp -= anchor.transform.up * Input.GetAxis("Right_Trigger") * mouseSensitivity;
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

        // Anchor movement
        Vector3 euler = pivot.rotation.eulerAngles;
        Quaternion rot = Quaternion.Euler(0f, euler.y, 0f);
        anchor.rotation = rot;

        Vector2 stickForce = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        bool verticalMovement = (Input.GetAxis("Left_Trigger") == 1 || Input.GetAxis("Right_Trigger") == 1);

        if (stickForce.magnitude > breakForce || verticalMovement)
        {
            snapIsDelayed = false;

            pivot.position = Vector3.Lerp(pivot.position, targetPosRight, Time.deltaTime * movementDampening);
            pivot.position = Vector3.Lerp(pivot.position, targetPosForward, Time.deltaTime * movementDampening);
            pivot.position = Vector3.Lerp(pivot.position, targetPosUp, Time.deltaTime * movementDampening);
        }
        else
        {
            if (targetBlock == null)
                return;

            if (!snapIsDelayed)
            {
                timeToSnapBack = Time.time + timeBeforeSnap;
                snapIsDelayed = true;
            }
            if (snapEnabled)
            {
                if (Time.time > timeToSnapBack)
                {
                    if (Vector3.Distance(anchor.position, targetBlock.position) > 0.1f)
                    {
                        Vector3 targetBlockPos;

                        targetBlockPos = targetBlock.position;

                        anchor.position = Vector3.Lerp(anchor.position, targetBlockPos, Time.deltaTime * 3f);
                        pivot.position = Vector3.Lerp(pivot.position, targetBlockPos, Time.deltaTime * 3f);

                        targetPosRight = anchor.position;
                        targetPosForward = anchor.position;
                        targetPosUp = anchor.position;
                    }
                }
            }
        }
    }

    public void UpdatePos()
    {
        if(anchor)
        {
            targetPosRight = anchor.position;
            targetPosForward = anchor.position;
            targetPosUp = anchor.position;
        }
    }

    public Transform Target
    {
        set
        {
            targetBlock = value;
            snapIsDelayed = true;
            timeToSnapBack = Time.time;
        }
    }
}