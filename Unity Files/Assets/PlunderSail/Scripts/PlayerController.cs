using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    [Header("Player Attributes")]
    [SerializeField]
    [Tooltip("The speed at which the player will move when no sails are open")]
    private float baseMoveSpeed;

    [SerializeField]
    [Tooltip("The additional speed per sail when the sails are open")]
    private float sailBonus;

    [SerializeField]
    [Tooltip("The degrees per frame the ship can rotate with no open sails")]
    private float baseTurnSpeed;

    [SerializeField]
    [Tooltip("The reduction per sail to turn rate when sails are open")]
    private float TurnRatePenalty;

    [SerializeField]
    [Tooltip("The degrees per frame that the steering wheel rotates")]
    private float wheelTurnSpeed;

    [SerializeField]
    private float rollSpeed;

    [Header("Sound Resources")]
    [SerializeField]
    private AudioClip[] startBuild;

    [SerializeField]
    private AudioClip[] fullSpeed;

    [SerializeField]
    private AudioClip[] slowDown;

    [Header("Debug Info")]
    [SerializeField]
    private float moveSpeed;

    [SerializeField]
    private float bonusMoveSpeed;

    [SerializeField]
    private float maxMoveSpeed;

    [SerializeField]
    private float turnSpeed;

    [SerializeField]
    private bool sailsOpen = true;

    // Max angle the ship can tilt
    private float maxRollValue = 4;

    private Quaternion pivotRotation;

    [SerializeField]
    private float myRotation;

    private Transform leftThruster = null;
    private Transform rightThruster = null;

    private float defaultRotation;

    private Vector3 velocity;

    private Vector3 targetVelocity;

    // The stearing wheel on the ship
    private Transform wheel;

    private AudioSource audioSource;

    private GameManager GM;

    private ComponentManager components;

    private Transform pivot;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        components = GetComponent<ComponentManager>();

        audioSource = GetComponent<AudioSource>();

        pivot = transform.GetChild(0);

        wheel = transform.GetChild(0).GetChild(0).GetChild(3);

        defaultRotation = transform.rotation.eulerAngles.z;

        // Subscribe to game state
        GameState.buildModeChanged += SetBuildMode;

        maxMoveSpeed = baseMoveSpeed;
        moveSpeed = baseMoveSpeed;
    }

    private void Update()
    {
        if (!GameState.BuildMode)
        {
            // lock the x and z axis rotation to 0f;
            transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

            // set targetVelocity to Value of left Thumb stick
            targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, 0) * turnSpeed;

            // Rotate the steering wheel right
            if (targetVelocity.x > 0)
            {
                wheel.Rotate(-wheelTurnSpeed, 0f, 0f);
            }
            // Rotate the steering wheel left
            else if (targetVelocity.x < 0)
            {
                wheel.Rotate(wheelTurnSpeed, 0f, 0f);
            }

            velocity.x = Mathf.Lerp(velocity.x, targetVelocity.x, 2f);

            // Lower or raise Sails
            if (Input.GetButtonDown("A_Button"))
            {
                sailsOpen = !sailsOpen;

                LowerSails(sailsOpen);
            }
        }
    }

    private void FixedUpdate()
    {
        if (!GameState.BuildMode)
        {
            rb.MovePosition(transform.position + pivot.forward * moveSpeed * Time.fixedDeltaTime);

            // Turn right
            if (velocity.x > 0)
            {
                pivot.Rotate(0f, turnSpeed * Time.deltaTime, 0f);
            }
            // Turn left
            else if (velocity.x < 0)
            {
                pivot.Rotate(0f, -turnSpeed * Time.deltaTime, 0f);
            }
            else
            {
                if (!Mathf.Approximately(pivot.localEulerAngles.z, 0f))
                {
                    Debug.Log("Turn back to default turn");

                    if (pivot.localEulerAngles.z != 0)
                    {
                        pivot.localEulerAngles = new Vector3(pivot.localEulerAngles.x, pivot.localEulerAngles.y, Mathf.MoveTowardsAngle(pivot.localEulerAngles.z, 0f, 1f));
                    }
                }
                else
                {
                    myRotation = 0;
                }
            }

            if (velocity.x != 0f)
            {
                // Turning left
                if (targetVelocity.x < 0)
                {
                    if (myRotation > -maxRollValue)
                    {
                        pivot.Rotate(new Vector3(pivot.rotation.x, pivot.rotation.y, maxRollValue * 0.6f) * rollSpeed * Time.deltaTime);
                        myRotation -= 1 * rollSpeed * Time.deltaTime;
                    }
                }
                // Turning Right
                else if (targetVelocity.x > 0)
                {
                    if (myRotation < maxRollValue)
                    {
                        pivot.Rotate(new Vector3(pivot.rotation.x, pivot.rotation.y, -maxRollValue * 0.6f) * rollSpeed * Time.deltaTime);
                        myRotation += 1 * rollSpeed * Time.deltaTime;
                    }
                }
            }

            pivotRotation = pivot.rotation;

            rb.rotation = pivot.rotation;
        }
    }

    private void LateUpdate()
    {
        if (!GameState.BuildMode)
        {
            pivot.rotation = pivotRotation;

            pivot.rotation = Quaternion.Euler(0f, pivot.rotation.eulerAngles.y, pivot.rotation.eulerAngles.z);
        }
    }

    private void LowerSails(bool _isOpen)
    {
        if (_isOpen)
        {
            moveSpeed = maxMoveSpeed;

            turnSpeed = baseTurnSpeed - (bonusMoveSpeed * TurnRatePenalty);

            if (fullSpeed.Length > 0 && !GameState.BuildMode)
            {
                audioSource.PlayOneShot(fullSpeed[Random.Range(0, fullSpeed.Length)], Random.Range(0.9f, 1.3f));
            }

            components.LowerSails();
        }
        else
        {
            moveSpeed = baseMoveSpeed;

            turnSpeed = baseTurnSpeed;

            if (slowDown.Length > 0)
            {
                audioSource.PlayOneShot(slowDown[Random.Range(0, fullSpeed.Length)], Random.Range(0.9f, 1.3f));
            }

            components.RaiseSails();
        }
    }

    public void setSpeedBonus(float _bonus)
    {
        bonusMoveSpeed = (_bonus * sailBonus);

        maxMoveSpeed = baseMoveSpeed + bonusMoveSpeed;
        moveSpeed = maxMoveSpeed;

        turnSpeed = baseTurnSpeed - (bonusMoveSpeed * TurnRatePenalty);
    }

    public float MoveSpeed
    {
        get { return moveSpeed; }

        set { moveSpeed = value; }
    }

    public void ResetMoveSpeed()
    {
        moveSpeed = baseMoveSpeed;
    }

    private void SetBuildMode(bool isBuildMode)
    {
        rb.isKinematic = isBuildMode;

        if (isBuildMode)
        {
            sailsOpen = true;

            LowerSails(sailsOpen);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe to game state
        GameState.buildModeChanged -= SetBuildMode;
    }
}