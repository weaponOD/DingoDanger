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

    [SerializeField]
    private float turnSpeed;

    private Vector3 defaultOrbPos;

    private Transform mesh;

    private GameObject steeringOrb;

    private float myRotation;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        mesh = transform.GetChild(0);
        steeringOrb = transform.GetChild(1).gameObject;
    }

    void Update()
    {
        targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, 0) * turnSpeed;

        velocity.x = Mathf.Lerp(velocity.x, targetVelocity.x, 2f);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.forward * Time.fixedDeltaTime * moveSpeed);

        defaultOrbPos = gameObject.transform.position + transform.forward * 10;

        steeringOrb.transform.position = defaultOrbPos + transform.right * velocity.x * 0.3f;
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
            if (velocity.x < 0)
            {
                if (myRotation > -maxRollValue)
                {
                    transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, maxRollValue) * Time.deltaTime);
                }
            }
            // Turning Right
            else if (velocity.x > 0)
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
    }
}