using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildModeCam : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Highest angle the camera can go from the horizon")]
    float MaxClamp;

    [SerializeField]
    [Tooltip("Lowest angle the camera can go to the horizon")]
    float minclamp;

    [SerializeField]
    [Tooltip("How much the camera will move when the mouse moves")]
    [Range(1, 20)]
    private float mouseSensitivity = 2.5f;

    [SerializeField]
    [Tooltip("How long it takes for the camera to reach it's destination")]
    private float orbitDampening = 10f;

    private Transform target;

    private Vector3 localRotation;

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
    }

    private void LateUpdate()
    {
        Quaternion targetRotation = Quaternion.Euler(localRotation.y, localRotation.x, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * orbitDampening);
    }

    public void SetTarget(Transform _target)
    {
        target = _target;
    }
}