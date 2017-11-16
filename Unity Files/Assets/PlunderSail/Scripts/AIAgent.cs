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

    [Header("Reward")]
    [SerializeField]
    private int goldReward = 50;

    [Header("Debug Info")]
    [SerializeField]
    protected float currentMoveSpeed;

    [SerializeField]
    protected float speedPerSail;

    protected List<IBehaviour> behaviours;

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

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();

        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        components = GetComponent<ComponentManager>();
        weaponController = GetComponent<WeaponController>();

        behaviours = new List<IBehaviour>(5);
    }

    protected override void Start()
    {
        base.Start();

        startPos = transform.position;

        startRot = transform.rotation;

        targetDirection = transform.forward;

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
        // check if sunk

        if (Input.GetKeyDown(KeyCode.Space))
        {
            transform.position = startPos;
            transform.rotation = startRot;
            targetRotation = startRot;
            targetDirection = transform.forward;
        }

        if (transform.position.y < -5)
        {
            if (player != null)
            {
                player.GiveGold(goldReward);
            }

            Destroy(gameObject);
        }

        // Collision Avoidance

        Ray ray1 = new Ray(transform.position, transform.forward + transform.right * 0.1f);
        Ray ray2 = new Ray(transform.position, transform.forward - transform.right * 0.1f);

        Debug.DrawRay(transform.position, (transform.forward + transform.right * 0.1f) * sightDistance, Color.red);
        Debug.DrawRay(transform.position, (transform.forward - transform.right * 0.1f) * sightDistance, Color.red);

        int layerMask = LayerMask.GetMask("Island");

        if (Physics.Raycast(ray1, sightDistance) && Physics.Raycast(ray2, sightDistance))
        {
            Debug.Log("Something is ahead of me");

            Ray Rightray = new Ray(transform.position, transform.forward + transform.right * sightAngle);
            Debug.DrawRay(transform.position, (transform.forward + transform.right * sightAngle) * sightDistance, Color.red);

            Ray Leftray = new Ray(transform.position, transform.forward - transform.right * sightAngle);
            Debug.DrawRay(transform.position, (transform.forward - transform.right * sightAngle) * sightDistance, Color.red);

            // If there is a collision at both left and right, increase sight angle
            if (Physics.Raycast(Rightray, sightDistance) && Physics.Raycast(Leftray, sightDistance))
            {
                sightAngle += 0.1f;
            }

            // when there's no collision in left or right turn in that direction

            if (!Physics.Raycast(Rightray, sightDistance))
            {
                // turn right
                targetDirection = transform.forward + transform.right * sightAngle;
            }

            if (!Physics.Raycast(Leftray, sightDistance))
            {
                // turn left
                targetDirection = transform.forward - transform.right * sightAngle;
            }
        }
        else if(!Physics.Raycast(ray1, sightDistance) && Physics.Raycast(ray2, sightDistance))
        {
            // turn right
            targetDirection = transform.forward + transform.right * 0.1f;
        }
        else if (!Physics.Raycast(ray2, sightDistance) && Physics.Raycast(ray1, sightDistance))
        {
            // turn left
            targetDirection = transform.forward - transform.right * 0.1f;
        }
        else 
        {
            sightAngle = 0f;
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