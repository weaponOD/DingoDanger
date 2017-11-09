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
    private float stunDuration = 1;

    [Header("Difficulty and Reward")]
    [SerializeField]
    TIER difficulty;

    [SerializeField]
    private int goldReward = 50;

    [Header("Debug Info")]
    [SerializeField]
    protected float currentMoveSpeed;

    [SerializeField]
    protected float speedPerSail;

    [SerializeField]
    protected List<IBehaviour> behaviours;

    // Non editor Variables
    protected Player player;

    protected Rigidbody rb;

    protected Quaternion targetRotation;
    protected Vector3 targetDirection;

    protected bool isStunned = false;

    protected Vector3 previousPos = Vector3.zero;

    private enum TIER { BASIC, MIDLEVEL, ELITE }

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        components = GetComponent<ComponentManager>();
        weaponController = GetComponent<WeaponController>();

        behaviours = new List<IBehaviour>(5);
    }

    protected override void Start()
    {
        base.Start();

        speedPerSail = baseMoveSpeed / components.getSpeedBonus();

        UpdateAttachments();
    }

    public void UpdateAttachments()
    {
        weaponController.LeftWeapons = components.GetAttachedLeftWeapons();
        weaponController.RightWeapons = components.GetAttachedRightWeapons();
        setSpeedBonus(components.getSpeedBonus());
    }

    public void setSpeedBonus(float _numOfSails)
    {
        currentMoveSpeed = (_numOfSails * speedPerSail);
    }

    protected virtual void Update()
    {
        // reset variables before calculating what to do this frame.
        targetDirection = Vector3.zero;
        behaviours.Clear();

        targetDirection = transform.forward;

        // check if sunk

        if (transform.position.y < -5)
        {
            player.GiveGold(goldReward);
            GameObject.Destroy(gameObject);
        }

        if (!GameState.BuildMode)
        {
            // Calculate which behaviours to use this frame.

            float distanceToPlayer = Vector3.Distance(transform.position, player.transform.position);

            if (distanceToPlayer > awarenessRange)
            {
                // WanderBehaviour wander = (WanderBehaviour)ScriptableObject.CreateInstance("WanderBehaviour");

                // behaviours.Add(wander);
            }

            // Check if the target is further than attack range, if so Chase target.
            if (Vector3.Distance(transform.position, player.transform.position) > attackRange)
            {
                ChaseBehaviour chase = (ChaseBehaviour)ScriptableObject.CreateInstance("ChaseBehaviour");

                behaviours.Add(chase);
            }

            // Check if the target is in attack range, if so align to target.
            if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                AlignmentBehaviour align = (AlignmentBehaviour)ScriptableObject.CreateInstance("AlignmentBehaviour");

                behaviours.Add(align);

                Debug.DrawRay(transform.position + transform.forward * 0.2f, transform.right, Color.red);

                Debug.DrawRay(transform.position + transform.forward * 0.2f, -transform.right, Color.red);

                if (Physics.Raycast(transform.position + transform.forward * 0.2f, transform.right, attackRange))
                {
                    weaponController.FireWeaponsRight();
                }

                if (Physics.Raycast(transform.position + transform.forward * 0.2f, -transform.right, attackRange))
                {
                    weaponController.FireWeaponsLeft();
                }
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, player.transform.position) < awarenessRange)
            {
                FleeBehaviour flee = (FleeBehaviour)ScriptableObject.CreateInstance("FleeBehaviour");

                behaviours.Add(flee);
            }
        }

        foreach (IBehaviour behaviour in behaviours)
        {
            targetDirection += behaviour.ApplyBehaviour(transform.position, player.transform);
        }

        //create the rotation we need to be in to look at the target
        targetRotation = Quaternion.LookRotation(targetDirection);

        //rotate us over time according to speed until we are in the required rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);

        if (Time.deltaTime != 0)
        {
            velocity = (transform.position - previousPos) / Time.deltaTime;
            previousPos = transform.position;
        }
    }

    protected virtual void FixedUpdate()
    {
        if (!isStunned)
        {
            rb.MovePosition(rb.position + transform.forward * currentMoveSpeed * Time.fixedDeltaTime);
        }
    }

    private void RemoveStun()
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
        if (collision.gameObject.CompareTag("Player"))
        {
            // how much the character should be knocked back
            var magnitude = 5000;
            // calculate force vector
            var force = transform.position - collision.transform.position;
            // normalize force vector to get direction only and trim magnitude
            force.Normalize();
            gameObject.GetComponent<Rigidbody>().AddForce(force * magnitude);

            Debug.Log("Knocked Back");
        }


        if (collision.contacts[0].thisCollider.gameObject.GetComponent<AttachmentBase>())
        {
            float hitDamage = collision.relativeVelocity.magnitude;
            Debug.Log("AI: Hit piece with a force of " + hitDamage);

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