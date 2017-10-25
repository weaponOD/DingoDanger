using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script component that will smoothly rotate the camera around camera Pivot
/// </summary>
/// 
public class PlayModeCam : MonoBehaviour
{
    private enum Mode { LEFTSIDE, BACKSIDE, RIGHTSIDE }

    private Transform cam;                          // the transform of the camera
    private Transform pivot;                        // the point at which the camera pivots around

    private Vector3 lastTargetPosition;

    private Transform target;                       // The target object to follow
    private Rigidbody targetRigidbody;

    [SerializeField]
    private float spinTurnLimit = 90;               // The threshold beyond which the camera stops following the target's rotation. (used in situations where a car spins out, for example)

    private float lastFlatAngle;                    // The relative angle of the target and the rig from the previous frame.
    private float currentTurnAmount;                // How much to turn the camera
    private float turnSpeedVelocityChange;          // The change in the turn speed velocity

    [SerializeField]
    private float moveSpeed;                        // How fast the rig will move to keep up with target's position

    [SerializeField]
    private float turnSpeed;                        // How fast the rig will turn to keep up with target's rotation

    private Mode currentMode = Mode.BACKSIDE;

    [SerializeField]
    private float timeBetweenInput = 0.5f;

    private bool canTurn = true;

    [SerializeField]
    private float xOffset;

    [SerializeField]
    private float yOffset;

    [SerializeField]
    private float zOffset;

    private Vector3 targetForward;

    private void Awake()
    {
        // find the camera in the object hierarchy
        cam = GetComponentInChildren<Camera>().transform;

        cam.localPosition = new Vector3(cam.localPosition.x, cam.localPosition.y, -zOffset);
        pivot = cam.parent;
    }

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0);
    }

    private void Update()
    {
        if (canTurn)
        {
            // Move to left Mode
            if (Input.GetAxis("Mouse_X") < 0)
            {
                if (currentMode > 0)
                {
                    currentMode--;
                    canTurn = false;
                    StartCoroutine(canTurnAgain());
                }
            }

            // Move to Right Mode
            if (Input.GetAxis("Mouse_X") > 0)
            {
                if ((int)currentMode < 2)
                {
                    currentMode++;
                    canTurn = false;
                    StartCoroutine(canTurnAgain());
                }
            }
        }
    }

    IEnumerator canTurnAgain()
    {
        yield return new WaitForSeconds(timeBetweenInput);
        canTurn = true;
    }

    private void FixedUpdate()
    {
        FollowTarget(Time.deltaTime);
    }

    // 
    private void FollowTarget(float deltaTime)
    {
        // if no time has passed then we quit early, as there is nothing to do
        if (!(deltaTime > 0))
            return;

        if (currentMode == Mode.BACKSIDE)
        {
            targetForward = target.forward;
        }
        else if (currentMode == Mode.LEFTSIDE)
        {
            targetForward = -target.right;
        }
        else
        {
            targetForward = target.right;
        }

        Vector3 targetUp = target.up;

        float currentFlatAngle = Mathf.Atan2(targetForward.x, targetForward.z) * Mathf.Rad2Deg;

        if (spinTurnLimit > 0)
        {
            var targetSpinSpeed = Mathf.Abs(Mathf.DeltaAngle(lastFlatAngle, currentFlatAngle)) / deltaTime;
            var desiredTurnAmount = Mathf.InverseLerp(spinTurnLimit, spinTurnLimit * 0.75f, targetSpinSpeed);
            var turnReactSpeed = (currentTurnAmount > desiredTurnAmount ? .1f : 1f);
            if (Application.isPlaying)
            {
                currentTurnAmount = Mathf.SmoothDamp(currentTurnAmount, desiredTurnAmount, ref turnSpeedVelocityChange, turnReactSpeed);
            }
            else
            {
                // for editor mode, smoothdamp won't work because it uses deltaTime internally
                currentTurnAmount = desiredTurnAmount;
            }
        }
        else
        {
            currentTurnAmount = 1;
        }

        lastFlatAngle = currentFlatAngle;

        // camera position moves towards target position:
        
        Vector3 targetFixedPos = new Vector3(target.position.x + target.right.x * xOffset, target.position.y + yOffset, target.position.z);
        transform.position = Vector3.Lerp(transform.position, targetFixedPos, deltaTime * moveSpeed);

        // camera's rotation is split into two parts, which can have independend speed settings:
        // rotating towards the target's forward direction (which encompasses its 'yaw' and 'pitch')

        targetForward.y = 0;

        if (targetForward.sqrMagnitude < float.Epsilon)
        {
            targetForward = transform.forward;
        }

        Quaternion rollRotation = Quaternion.LookRotation(targetForward, Vector3.up);

        transform.rotation = Quaternion.Lerp(transform.rotation, rollRotation, turnSpeed * currentTurnAmount * deltaTime);
    }
}