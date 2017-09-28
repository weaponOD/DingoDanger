using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  Script component that will Move the Camera's anchor point along the x and z axis.
/// </summary>

public class CameraMovement : MonoBehaviour
{
    // Camera Pivot Point
    private Transform pivot;

    private Transform player;

    [Header("Movement Attributes")]
    [SerializeField]
    [Tooltip("How much the camera will move when the mouse moves.")]
    private float mouseSensitivity = 0.2f;

    // target location to smoothly move towards
    private Vector3 targetPosRight;
    private Vector3 targetPosForward;
    private Vector3 targetPosUp;

    [SerializeField]
    [Tooltip("How long it takes for the camera to reach it's destination")]
    private float movementDampening = 10f;

    [Header("Quality of use Attributes")]

    [SerializeField]
    [Tooltip("How much movement is needed to freely move the camera.")]
    [Range(0, 1)]
    float breakForce = 0.2f;

    private float timeToSnapBack;

    [SerializeField]
    [Tooltip("Time in seconds of player inactivity before the camera snaps back to the target block.")]
    [Range(0, 20)]
    private float timeBeforeSnap = 1.5f;

    private bool snapIsDelayed = false;

    private Transform targetBlock;

    [SerializeField]
    private bool buildMode = false;

    private void Awake()
    {
        pivot = transform.parent;

        targetPosRight = transform.position;
        targetPosForward = transform.position;
        targetPosUp = transform.position;

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            targetPosRight += transform.right * Input.GetAxis("Horizontal") * mouseSensitivity;

            targetPosForward += transform.forward * Input.GetAxis("Vertical") * mouseSensitivity;
        }

        if (Input.GetAxis("Left_Trigger") != 0 || Input.GetAxis("Right_Trigger") != 0)
        {
            targetPosUp += transform.up * Input.GetAxis("Left_Trigger") * mouseSensitivity;

            targetPosUp -= transform.up * Input.GetAxis("Right_Trigger") * mouseSensitivity;
        }
    }

    void LateUpdate()
    {
        Vector3 euler = pivot.rotation.eulerAngles;
        Quaternion rot = Quaternion.Euler(0f, euler.y, 0f);
        transform.rotation = rot;

        if (buildMode)
        {
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

                if (Time.time > timeToSnapBack)
                {
                    if (Vector3.Distance(transform.position, targetBlock.position) > 0.1f)
                    {
                        Vector3 targetBlockPos;

                        targetBlockPos = targetBlock.position;

                        transform.position = Vector3.Lerp(transform.position, targetBlockPos, Time.deltaTime * 3f);
                        pivot.position = Vector3.Lerp(pivot.position, targetBlockPos, Time.deltaTime * 3f);

                        targetPosRight = transform.position;
                        targetPosForward = transform.position;
                        targetPosUp = transform.position;
                    }
                }
            }
        }
        else
        {
            // move towards the player
            Vector3 targetPos = player.position;

            pivot.position = targetPos + player.forward * 2f;

            targetPosRight = transform.position;
            targetPosForward = transform.position;
            targetPosUp = transform.position;
        }
    }

    public bool BuildMode
    {
        get { return buildMode; }
        set { buildMode = value; }
    }

    public void SetBuildMode(bool isBuildMode)
    {
        buildMode = isBuildMode;
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