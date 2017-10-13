using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingAgent : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float turnSpeed;

    [SerializeField]
    private float flockingRange;

    [SerializeField]
    private float timeBetweenWanders;

    [Header("Weightings")]
    [SerializeField]
    private float AlignmentWeight;

    [SerializeField]
    private float CohesionWeight;

    [SerializeField]
    private float SeperationWeight;

    [SerializeField]
    private float WanderWeight;

    [SerializeField]
    private float FleeWeight;

    // Forces

    private Vector2 alignmentForce;

    private Vector2 CohesionForce;

    private Vector2 SeperationForce;

    private Vector2 WanderForce;

    private Vector2 FleeForce;

    private Vector2 resultantForce;

    private Vector2 velocity;

    FlockingAgent[] neighbours;

    private float timeToWander = 0;

    private void Update()
    {
        CalculateNeighbours();

        alignmentForce = CalculateAlignmentForce();
        CohesionForce = CalculateCohesionForce();
        SeperationForce = CalculateSeperationForce();
        
        if(Time.time > timeToWander)
        {
            timeToWander = timeBetweenWanders;
            WanderForce = CalculateWanderForce();
        }

        resultantForce = (alignmentForce * AlignmentWeight) + (CohesionForce * CohesionWeight) + (SeperationForce * SeperationWeight) + (WanderForce * WanderWeight);
        resultantForce.Normalize();

        resultantForce *= moveSpeed;

        velocity = Vector2.Lerp(velocity, resultantForce, 2f * Time.deltaTime);

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(new Vector3(velocity.x, 0f, velocity.y)), turnSpeed * Time.deltaTime);

        transform.Translate(new Vector3(resultantForce.x, 0f, resultantForce.y));
    }

    // returns force that steers the agent towards the average velocity of nearby boids
    private Vector2 CalculateAlignmentForce()
    {
        Vector2 force = Vector2.zero;
        int neighbourCount = 0;

        foreach (FlockingAgent boid in neighbours)
        {
            if (boid != this)
            {
                if (Vector2.Distance(Position2D, boid.Position2D) < flockingRange)
                {
                    force += boid.Velocity;
                    neighbourCount++;
                }
            }
        }

        if (neighbourCount == 0)
            return force;

        force /= neighbourCount;
        force.Normalize();

        return force;
    }

    // returns force that steers the agent towards the centre of nearby boids
    private Vector2 CalculateCohesionForce()
    {
        Vector2 force = Vector2.zero;
        int neighbourCount = 0;

        foreach (FlockingAgent boid in neighbours)
        {
            if (boid != this)
            {
                if (Vector2.Distance(Position2D, boid.Position2D) < flockingRange)
                {
                    force += boid.Position2D;
                    neighbourCount++;
                }
            }
        }

        if (neighbourCount == 0)
            return force;

        force /= neighbourCount;
        force = force - Position2D;

        force.Normalize();

        return force;
    }

    // returns force that steers the agent away from nearby boids
    private Vector2 CalculateSeperationForce()
    {
        Vector2 force = Vector2.zero;
        int neighbourCount = 0;

        foreach (FlockingAgent boid in neighbours)
        {
            if (boid != this)
            {
                if (Vector2.Distance(Position2D, boid.Position2D) < flockingRange)
                {
                    force += boid.Position2D - Position2D;
                    neighbourCount++;
                }
            }
        }

        if (neighbourCount == 0)
            return force;

        force /= neighbourCount;
        force *= -1f;

        force.Normalize();

        return force;
    }

    // returns force that steers towards a random point within a unit circle ahead of the agent
    private Vector3 CalculateWanderForce()
    {
        Vector2 force = Vector2.zero;

        Vector2 randomDir = Position2D + resultantForce * 2 + Random.insideUnitCircle;

        force = randomDir - Position2D;

        return force;
    }

    // populates a list of boids within flocking radius.
    private void CalculateNeighbours()
    {
        GameObject[] neighbourhood = GameObject.FindGameObjectsWithTag("Boid");
        neighbours = new FlockingAgent[neighbourhood.Length];
        int indexCount = 0;

        // Loop through array of GameObjects and add boids within flocking radius to our neghbours list.
        foreach (GameObject boid in neighbourhood)
        {
            neighbours[indexCount] = neighbourhood[indexCount].GetComponent<FlockingAgent>();
            indexCount++;
        }
    }

    // returns vector2 that indicates the agents velocity
    public Vector2 Velocity
    {
        get { return resultantForce; }
    }

    // returns vector2 representation of 3D position
    public Vector2 Position2D
    {
        get
        {
            return new Vector2(transform.position.x, transform.position.z);
        }
    }
}