using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPathFollower : AIAgent
{
    [SerializeField]
    protected Transform path;

    [SerializeField]
    protected float fleeRange;

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
        // If there's a collision disable states for x seconds
        if (AvoidCollisions())
        {
            ActiveStateTime = Time.time + stateCoolDown;
        }

        // Apply states only if the ship hasn't collided for x seconds
        if (stateIsActive)
        {
            CalculateState();
            ApplyState();
        }

        // If the AI hasn't detected a collision in x seconds apply state behaviour
        if (Time.time > ActiveStateTime)
        {
            stateIsActive = true;
        }

        SteerInDirection();

        if (dead)
        {
            CheckIfSunk();
        }
    }

    protected override void CalculateState()
    {
        if (currentState == State.FLEE)
        {
            float distToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distToPlayer > fleeRange)
            {
                currentState = State.PATH;
            }
        }
    }

    protected override void ApplyState()
    {
        if (currentState == State.PATH)
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
        }
        else if (currentState == State.FLEE)
        {
            targetDirection = transform.position - player.transform.position;
        }
    }

    public override void TakeDamage(float damgage)
    {
        currentHealth -= damgage;

        if (currentHealth <= 0 && !dead)
        {
            Die();
        }
        else
        {
            currentState = State.FLEE;
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}