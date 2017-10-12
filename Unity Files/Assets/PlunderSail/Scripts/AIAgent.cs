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

    [Header("Debug Info")]
    [SerializeField]
    protected float currentMoveSpeed;

    [SerializeField]
    protected float BonusMoveSpeed;

    [SerializeField]
    private bool chasingTarget;

    [SerializeField]
    private bool aligningWithTarget;

    [SerializeField]
    protected List<IBehaviour> behaviours;

    // Non editor Variables
    protected Player player;

    protected Rigidbody rb;

    protected Quaternion targetRotation;
    protected Vector3 targetDirection;

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

        weaponController.LeftWeapons = components.GetAttachedLeftWeapons();
        weaponController.RightWeapons = components.GetAttachedRightWeapons();
        BonusMoveSpeed = components.getSpeedBonus();

        currentMoveSpeed = baseMoveSpeed + BonusMoveSpeed;
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
            player.GiveGold(50);
            GameObject.Destroy(gameObject);
        }

        if (!GameState.BuildMode)
        {
            // Calculate which behaviours to use this frame.

            // Check if the target is further than attack range, if so Chase target.
            if (Vector3.Distance(transform.position, player.transform.position) > attackRange)
            {
                chasingTarget = true;

                ChaseBehaviour chase = (ChaseBehaviour)ScriptableObject.CreateInstance("ChaseBehaviour");

                behaviours.Add(chase);
            }
            else
            {
                chasingTarget = false;
            }

            // Check if the target is in attack range, if so align to target.
            if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                aligningWithTarget = true;

                AlignmentBehaviour align = (AlignmentBehaviour)ScriptableObject.CreateInstance("AlignmentBehaviour");

                behaviours.Add(align);

                Debug.DrawRay(transform.position + transform.forward * 0.2f, transform.right,Color.red);

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
            else
            {
                
                aligningWithTarget = false;
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
    }

    protected virtual void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.forward * currentMoveSpeed * Time.fixedDeltaTime);
    }
}