using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    // Variables
    private Rigidbody rb;

    private Vector3 velocity;

    private Vector3 targetVelocity;

    [SerializeField]
    private float maxRollValue = 6;

    [SerializeField]
    private float moveSpeed;

    private float defaultMoveSpeed;

    [SerializeField]
    private float tiltSpeed;

    [SerializeField]
    private float turnSpeed;

    private float defaultRotation;

    private Vector3 defaultOrbPos;

    private Transform mesh;

    private Transform wheel;

    private GameObject steeringOrb;

    private float myRotation;

    [SerializeField]
    private float wheelTurnSpeed;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mesh = transform.GetChild(0);

        wheel = transform.GetChild(0).GetChild(3);

        defaultRotation = transform.rotation.z;
        steeringOrb = transform.GetChild(1).gameObject;

        defaultMoveSpeed = moveSpeed;
    }

    void Update()
    {
        targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, 0) * turnSpeed;

        // Turning right
        if (targetVelocity.x > 0)
        {
            wheel.Rotate(-wheelTurnSpeed, 0f, 0f);
        }
        // Turning right
        else if (targetVelocity.x < 0)
        {
            wheel.Rotate(wheelTurnSpeed, 0f, 0f);
        }

        velocity.x = Mathf.Lerp(velocity.x, targetVelocity.x, 2f);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.forward * Time.fixedDeltaTime * moveSpeed);

        defaultOrbPos = gameObject.transform.position + transform.forward * 10;

        steeringOrb.transform.position = defaultOrbPos + transform.right * velocity.x * 0.3f;

        if (steeringOrb.transform.position == defaultOrbPos)
        {
            myRotation = transform.rotation.z * 100f;

            if (!Mathf.Approximately(myRotation, 0f))
            {
                transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, defaultRotation * tiltSpeed) * Time.fixedDeltaTime);
            }
        }
    }

    private void LateUpdate()
    {
        if (steeringOrb.transform.position != defaultOrbPos)
        {
            // rotation around the y axis to steer the ship
            Quaternion neededRotation = Quaternion.LookRotation(steeringOrb.transform.position - transform.position);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, neededRotation, Time.deltaTime * turnSpeed);

            myRotation = transform.rotation.z * 100f;

            // Turning left
            if (targetVelocity.x < 0)
            {
                if (myRotation > -maxRollValue)
                {
                    transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, maxRollValue) * Time.deltaTime);
                }
            }
            // Turning Right
            else if (targetVelocity.x > 0)
            {
                if (myRotation < maxRollValue)
                {
                    transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, -maxRollValue) * Time.deltaTime);
                }
            }

            transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, transform.eulerAngles.z);

            steeringOrb.transform.position = new Vector3(steeringOrb.transform.position.x, 0f, steeringOrb.transform.position.z);

            Debug.DrawLine(transform.position, steeringOrb.transform.position);
            Debug.DrawLine(transform.position, transform.position + transform.forward * 15);
        }
        else
        {
            //myRotation = transform.rotation.z * 100f;

            //if (!Mathf.Approximately(myRotation, 0f))
            //{
            //    transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, defaultRotation) * Time.deltaTime * 0.05f);
            //}
        }
    }

    public float MoveSpeed
    {
        get { return moveSpeed; }

        set { moveSpeed = value; }
    }

    public void ResetMoveSpeed()
    {
        moveSpeed = defaultMoveSpeed;
    }
}