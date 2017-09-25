using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipMovement : MonoBehaviour
{
    // Variables
    private Rigidbody rb;

    [SerializeField]
    private Vector3 velocity;

    [SerializeField]
    private Vector3 targetVelocity;

    [SerializeField]
    private float maxRollValue = 6;

    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float turnSpeed;

    private Vector3 defaultOrbPos;

    [SerializeField]
    private Transform mesh;

    [SerializeField]
    private GameObject steeringOrb;

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

            Quaternion roation = Quaternion.RotateTowards(transform.rotation, neededRotation, Time.deltaTime * turnSpeed);

            rb.MoveRotation(roation);

            steeringOrb.transform.position = new Vector3(steeringOrb.transform.position.x, 0f, steeringOrb.transform.position.z);

            Debug.DrawLine(transform.position, steeringOrb.transform.position);
            Debug.DrawLine(transform.position, transform.position + transform.forward * 15);
        }
    }
}