using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    private float moveSpeed;

    private float maxMoveSpeed;

    [SerializeField]
    private float baseMoveSpeed;

    [SerializeField]
    private Vector3 velocity;

    private Vector3 targetVelocity;

    // The stearing wheel on the ship
    private Transform wheel;

    // Steering wheel turn speed
    [SerializeField]
    private float wheelTurnSpeed;

    private bool buildMode = false;

    // Max angle the ship can tilt
    [SerializeField]
    private float maxRollValue = 6;

    private Transform leftThruster = null;
    private Transform rightThruster = null;

    [SerializeField]
    private float myRotation;

    private float defaultRotation;

    [SerializeField]
    private float tiltSpeed;

    [SerializeField]
    private float turnSpeed;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        wheel = transform.GetChild(0).GetChild(3);

        leftThruster = transform.GetChild(1);
        rightThruster = transform.GetChild(2);

        defaultRotation = transform.rotation.eulerAngles.z;

        // Subscribe to game state
        GameState.buildModeChanged += SetBuildMode;

        maxMoveSpeed = baseMoveSpeed;
        moveSpeed = baseMoveSpeed;
    }

    private void Update()
    {
        // set targetVelocity to Value of left Thumb stick
        targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, 0) * turnSpeed;

        // Rotate the steering wheel right
        if (targetVelocity.x > 0)
        {
            wheel.Rotate(-wheelTurnSpeed, 0f, 0f);
        }
        // Rotate the steering wheel left
        else if (targetVelocity.x < 0)
        {
            wheel.Rotate(wheelTurnSpeed, 0f, 0f);
        }

        velocity.x = Mathf.Lerp(velocity.x, targetVelocity.x, 2f);
    }

    void FixedUpdate()
    {
        if(!buildMode)
        {
            rb.MovePosition(rb.position + transform.forward * Time.fixedDeltaTime * moveSpeed);

            // Turn right
            if (velocity.x > 0)
            {
                rb.AddForceAtPosition(rightThruster.forward * turnSpeed, rightThruster.position, ForceMode.Impulse);

                if (moveSpeed > baseMoveSpeed / 2)
                {
                    moveSpeed -= 0.01f;
                }
            }
            // Turn left
            else if (velocity.x < 0)
            {
                rb.AddForceAtPosition(leftThruster.forward * turnSpeed, rightThruster.position, ForceMode.Impulse);

                if (moveSpeed > baseMoveSpeed / 2)
                {
                    moveSpeed -= 0.01f;
                }
            }
            else
            {
                if (moveSpeed < maxMoveSpeed)
                {
                    moveSpeed += 0.01f;
                }

                myRotation = transform.rotation.z * 100f;

                if (!Mathf.Approximately(myRotation, 0f))
                {
                    //Debug.Log("Turn back to default turn");
                    //transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, defaultRotation * tiltSpeed) * Time.fixedDeltaTime);
                }
            }
        }
    }

    private void LateUpdate()
    {
        //if (velocity.x != 0f)
        //{
        //    myRotation = transform.rotation.z * 100f;

        //    // Turning left
        //    if (targetVelocity.x < 0)
        //    {
        //        if (Mathf.Abs(myRotation) < maxRollValue)
        //        {
        //            transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, maxRollValue * 0.6f) * Time.deltaTime);
        //        }
        //    }
        //    // Turning Right
        //    else if (targetVelocity.x > 0)
        //    {
        //        if (Mathf.Abs(myRotation) < maxRollValue)
        //        {
        //            transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, -maxRollValue * 0.6f) * Time.deltaTime);
        //        }
        //    }

        //    Debug.Log("Turning");

        //    transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, transform.eulerAngles.z);
        //}
    }

    public float MoveSpeed
    {
        get { return moveSpeed; }

        set { moveSpeed = value; }
    }

    public void ResetMoveSpeed()
    {
        moveSpeed = baseMoveSpeed;
    }

    private void SetBuildMode(bool isBuildMode)
    {
        buildMode = isBuildMode;

        rb.isKinematic = buildMode;
    }

    private void OnDestroy()
    {
        // Unsubscribe to game state
        GameState.buildModeChanged -= SetBuildMode;
    }
}