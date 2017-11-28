using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class that has a list of behaviours which it uses to steer. The decision to choose which behaviours to apply is done by the children of AI agent.
/// </summary>
/// 
public class AIAgent : LivingEntity
{
    // Editor Variables
    [Header("AI Attributes")]

    [SerializeField]
    [Range(0, 50)]
    [Tooltip("The movement speed the ship will move with no sails.")]
    protected float baseMoveSpeed;

    [SerializeField]
    [Range(0, 1)]
    protected float rotationSpeed;

    [SerializeField]
    [Range(0, 300)]
    protected float attackRange;

    [SerializeField]
    [Range(0, 300)]
    protected float awarenessRange;

    [SerializeField]
    protected float damageSpreadMultiplier;

    [SerializeField]
    private float stunDuration = 1;

    [Header("Reward")]
    [SerializeField]
    private int goldReward = 50;

    [Header("Debug Info")]
    [SerializeField]
    protected float currentMoveSpeed;

    [SerializeField]
    protected float speedPerSail;

    // Non editor Variables
    protected Player player;

    protected Rigidbody rb;

    protected Quaternion targetRotation;

    [SerializeField]
    protected Vector3 targetDirection;

    protected bool isStunned = false;

    [SerializeField]
    [Range(0, 100)]
    protected float sightDistance = 40;

    protected float sightAngle = 0;

    protected Vector3 previousPos = Vector3.zero;

    protected Vector3 startPos;

    protected Quaternion startRot;

    protected enum State { WANDER, CHASE, FIGHT, PATH, FLEE }

    [SerializeField]
    protected State currentState = State.WANDER;

    [SerializeField]
    protected bool stateIsActive = false;

    [SerializeField]
    private float angle;

    protected float stateCoolDown = 2;

    protected float ActiveStateTime = 0;

    protected bool playerRight = false;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();

        if (GameObject.FindGameObjectWithTag("Player"))
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        }

        components = GetComponent<ComponentManager>();
        weaponController = GetComponent<WeaponController>();
    }

    protected override void Start()
    {
        base.Start();

        startPos = transform.position;

        startRot = transform.rotation;

        targetDirection = transform.forward;

        speedPerSail = baseMoveSpeed / components.getSpeedBonus();

        UpdateParts();
    }

    public override void UpdateParts()
    {
        weaponController.LeftWeapons = components.GetAttachedLeftWeapons();
        weaponController.RightWeapons = components.GetAttachedRightWeapons();
        setSpeedBonus(components.getSpeedBonus());

        attachments = components.Attachments;

        if (components.Attachments.Length == 0)
        {
            Die();
        }
    }

    public void setSpeedBonus(float _numOfSails)
    {
        currentMoveSpeed = speedPerSail / 2 + (_numOfSails * speedPerSail);
    }

    protected virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = startPos;
            transform.rotation = startRot;
            targetRotation = startRot;
            targetDirection = transform.forward;
        }

        AvoidCollisions();

        // If there's a collision disable states for x seconds
        if (AvoidCollisions())
        {
            //Debug.Log("Collision Detected, states are on cooldown");

            ActiveStateTime = Time.time + stateCoolDown;
            stateIsActive = false;
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

    protected void SteerInDirection()
    {
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

    protected virtual void CalculateState()
    {
        float distToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distToPlayer < awarenessRange && distToPlayer > attackRange)
        {
            currentState = State.CHASE;
        }

        if (distToPlayer <= attackRange)
        {
            currentState = State.FIGHT;
        }
    }

    protected virtual void ApplyState()
    {
        // Hunt down player if chasing them
        if (currentState == State.CHASE)
        {
            //float distToPlayer = Vector3.Distance(player.transform.position, transform.position);

            //float t = distToPlayer / currentMoveSpeed;

            //Vector3 targetPos = player.transform.position + player.Velocity * t;

            targetDirection = player.transform.position;

            return;
        }

        // In this state we want the AI to steer in a direction that results in the dot product equaling 0, aka we want to our ship to be perpendicular to the player.
        if (currentState == State.FIGHT)
        {
            Vector3 vecBetween = player.transform.position - transform.position;

            // if player is closer to right side
            if (Vector3.Distance(player.transform.position, transform.position + transform.right) < Vector3.Distance(player.transform.position, transform.position - transform.right))
            {
                angle = Vector3.Angle(vecBetween, transform.right);
                playerRight = true;
            }
            else
            {
                angle = Vector3.Angle(vecBetween, -transform.right);
                playerRight = false;
            }

            Quaternion difference = Quaternion.AngleAxis(angle, Vector3.up);

            targetDirection = difference * transform.forward;

            if (angle < 10f)
            {
                if (playerRight)
                {
                    weaponController.FireWeaponsRight(true);
                }
                else
                {
                    weaponController.FireWeaponsLeft(true);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.75F);
        Gizmos.DrawWireSphere(transform.position, awarenessRange);

        Gizmos.color = new Color(1, 0, 0, 0.75F);
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    protected void CheckIfSunk()
    {
        if (transform.position.y < -5)
        {
            if (player != null)
            {
                player.GiveGold(goldReward);
            }

            Destroy(gameObject);
        }
    }

    protected virtual void FixedUpdate()
    {
        if (!isStunned)
        {
            rb.MovePosition(rb.position + transform.forward * currentMoveSpeed * Time.fixedDeltaTime);
        }
    }

    // Returns true if there is a collison
    protected bool AvoidCollisions()
    {
        Ray ray1 = new Ray(transform.position, transform.forward);

        Debug.DrawRay(transform.position, transform.forward * sightDistance, Color.red);

        //int layerMask = LayerMask.GetMask("Island");

        if (Physics.Raycast(ray1, sightDistance))
        {
            Ray Rightray = new Ray(transform.position, transform.forward + transform.right * sightAngle);
            Debug.DrawRay(transform.position, (transform.forward + transform.right * sightAngle) * sightDistance * 1.5f, Color.red);
            RaycastHit hitRight;
            float distanceRight = 0;

            Ray Leftray = new Ray(transform.position, transform.forward - transform.right * sightAngle);
            Debug.DrawRay(transform.position, (transform.forward - transform.right * sightAngle) * sightDistance * 1.5f, Color.red);
            RaycastHit hitLeft;
            float distanceLeft = 0;

            if (Physics.Raycast(Leftray, out hitLeft, sightDistance * 1.5f))
            {
                distanceLeft = hitLeft.distance;
            }

            if (Physics.Raycast(Rightray, out hitRight, sightDistance * 1.5f))
            {
                distanceRight = hitRight.distance;
            }

            // No collisions on both sides
            if (distanceLeft == 0 && distanceRight == 0)
            {
                //Debug.Log("No collisions on both sides");

                targetDirection = transform.forward + transform.right * 0.2f;

                sightAngle = 0;

            }
            else if (distanceLeft != 0 && distanceRight != 0)
            {
                //Debug.Log("collisions on both sides");

                if (distanceLeft <= distanceRight)
                {
                    targetDirection = transform.forward + transform.right * sightAngle;
                }
                else
                {
                    targetDirection = transform.forward - transform.right * sightAngle;
                }

                sightAngle += 0.1f;
            }
            else if (distanceLeft != 0)
            {
                //Debug.Log("collisions on left side");
                // Turn right
                targetDirection = transform.forward + transform.right * sightAngle;
            }
            else if (distanceRight != 0)
            {
                //Debug.Log("collisions on right side");
                // Turn left
                targetDirection = transform.forward - transform.right * sightAngle;
            }
        }
        else
        {
            return false;
        }

        return true;
    }

    public override void TakeDamage(float damgage)
    {
        // spread damage to all attachments

        attachments = components.Attachments;

        for (int i = 0; i < attachments.Length; i++)
        {
            attachments[i].TakeDamage(damgage * damageSpreadMultiplier);
        }

        if (components.Attachments.Length == 0)
        {
            Die();
        }
    }

    protected void RemoveStun()
    {
        isStunned = false;
    }

    public void AddStun()
    {
        isStunned = true;

        Invoke("RemoveStun", stunDuration);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].thisCollider.gameObject.GetComponent<AttachmentBase>())
        {
            float hitDamage = collision.relativeVelocity.magnitude;
            //Debug.Log("AI: Hit piece with a force of " + hitDamage);

            if (hitDamage < 10)
            {
                collision.contacts[0].thisCollider.gameObject.GetComponent<AttachmentBase>().TakeDamage(5 * collision.relativeVelocity.magnitude);
            }
            else
            {
                collision.contacts[0].thisCollider.gameObject.GetComponent<AttachmentBase>().TakeDamage(100 * collision.relativeVelocity.magnitude);
            }
        }
    }
}