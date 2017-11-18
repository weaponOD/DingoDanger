using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPathFollower : AIAgent
{
    [SerializeField]
    protected Transform path;

    protected Transform[] waypoints = null;

    protected int pathLength = 0;

    protected int currentWayPoint = 0;

    protected override void Awake()
    {
        base.Awake();
    }

    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        waypoints = path.GetComponentsInChildren<Transform>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (waypoints.Length > 1)
        {
            targetDirection = waypoints[currentWayPoint].position - transform.position;

            if (Vector3.Distance(transform.position, waypoints[currentWayPoint].position) < 2)
            {
                if (currentWayPoint < pathLength - 1)
                {
                    currentWayPoint++;
                }
                else
                {
                    currentWayPoint = 0;
                }
            }
        }

        if (targetDirection != Vector3.zero)
        {
            //create the rotation we need to be in to look at the target
            targetRotation = Quaternion.LookRotation(targetDirection);

            //rotate us over time according to speed until we are in the required rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }

        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);

        if (Time.deltaTime != 0)
        {
            velocity = (transform.position - previousPos) / Time.deltaTime;
            previousPos = transform.position;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}