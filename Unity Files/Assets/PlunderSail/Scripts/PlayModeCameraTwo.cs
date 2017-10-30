using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayModeCameraTwo : MonoBehaviour
{
    [Header("Right Stick Rotation")]
    [SerializeField]
    [Tooltip("How much the camera will move when the mouse moves")]
    [Range(1, 20)]
    private float mouseSensitivity = 2.5f;

    [SerializeField]
    [Tooltip("Highest angle the camera can go from the horizon")]
    private float MaxClamp = 60f;

    [SerializeField]
    [Tooltip("Lowest angle the camera can go to the horizon")]
    private float minclamp = 5f;

    [SerializeField]
    [Tooltip("How long it takes for the camera to reach it's destination")]
    private float orbitDampening = 10f;

    [SerializeField]
    private bool invertedY = false;

    private Transform target;

    [SerializeField]
    private Vector3 offset;

    private Transform pivot;

    private Vector3 localRotation;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        pivot = transform.parent;

        localRotation = new Vector3(pivot.rotation.eulerAngles.y, pivot.rotation.eulerAngles.x, 0f);
    }

    // Use this for initialization
    void Start ()
    {
        localRotation = new Vector3(pivot.rotation.eulerAngles.y, pivot.rotation.eulerAngles.x, 0f);
    }

    private void Update()
    {
        // Rotate the camera based on right thumb stick input
        if (Input.GetAxis("Mouse_X") != 0 || Input.GetAxis("Mouse_Y") != 0)
        {
            if (invertedY)
            {
                localRotation.y += Input.GetAxis("Mouse_Y") * mouseSensitivity;
            }
            else
            {
                localRotation.y -= Input.GetAxis("Mouse_Y") * mouseSensitivity;
            }

            localRotation.x += Input.GetAxis("Mouse_X") * mouseSensitivity;

            // Clamp the Y rotation to horizon and not flipping it over at the top
            localRotation.y = Mathf.Clamp(localRotation.y, minclamp, MaxClamp);
        }

        if (Input.GetButtonDown("Right_Stick_Click"))
        {
            invertedY = !invertedY;
        }
    }

    private void LateUpdate()
    { 
        pivot.position = target.position + target.forward * offset.z + target.right * offset.x + target.up * offset.y;


        // Camera Rotation using the right thumb stick
        Quaternion targetRotation = Quaternion.Euler(localRotation.y, localRotation.x, 0f);
        pivot.rotation = Quaternion.Lerp(pivot.rotation, targetRotation, Time.deltaTime * orbitDampening);
    }
}