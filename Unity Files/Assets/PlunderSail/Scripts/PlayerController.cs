using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;

    private Transform pier;

    [Header("Player Controller Attributes")]

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

    private bool buildMode = false;

    [Header("Sound Resources")]

    [SerializeField]
    private AudioClip[] startBuild;

    [SerializeField]
    private AudioClip[] fullSpeed;

    [SerializeField]
    private AudioClip[] slowDown;

    [Header("Debug Info")]
    // Max angle the ship can tilt
    [SerializeField]
    private float maxRollValue = 6;

    [SerializeField]
    private float myRotation;

    [SerializeField]
    private float turnSpeed;

    [SerializeField]
    private float tiltSpeed;

    private Transform leftThruster = null;
    private Transform rightThruster = null;

    private float defaultRotation;

    private float moveSpeed;

    private float bonusMoveSpeed;

    private float maxMoveSpeed;

    private Vector3 velocity;

    private Vector3 targetVelocity;

    // The stearing wheel on the ship
    private Transform wheel;

    private bool sailsDown = true;

    private AudioSource audioSource;

    private GameManager GM;

    private bool movingToPier = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        audioSource = GetComponent<AudioSource>();

        wheel = transform.GetChild(0).GetChild(3);

        leftThruster = transform.GetChild(1);
        rightThruster = transform.GetChild(2);

        defaultRotation = transform.rotation.eulerAngles.z;

        // Subscribe to game state
        GameState.buildModeChanged += SetBuildMode;

        maxMoveSpeed = baseMoveSpeed;
        moveSpeed = baseMoveSpeed;
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);

        if (!movingToPier)
        {
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

            if(Input.GetButtonDown("A_Button"))
            {
                sailsDown = !sailsDown;

                LowerSails(sailsDown);
            }


        }
    }

    private void LowerSails(bool _isDown)
    {
        if(_isDown)
        {
            moveSpeed = maxMoveSpeed;

            turnSpeed = baseTurnSpeed - (bonusMoveSpeed * TurnRatePenalty);

            if(fullSpeed.Length > 0)
            {
                audioSource.PlayOneShot(fullSpeed[Random.Range(0, fullSpeed.Length)], Random.Range(0.9f, 1.3f));
            }
        }
        else
        {
            moveSpeed = baseMoveSpeed;

            turnSpeed = baseTurnSpeed;

            if(slowDown.Length > 0)
            {
                audioSource.PlayOneShot(slowDown[Random.Range(0, fullSpeed.Length)], Random.Range(0.9f, 1.3f));
            }
        }
    }

    void FixedUpdate()
    {
        if(movingToPier)
        {
            if(Vector3.Distance(transform.position, pier.position) > 2)
            {
                transform.position = Vector3.MoveTowards(transform.position, pier.position, moveSpeed * 3 * Time.deltaTime);

                // The position of the pier less the y axis
                Vector3 pierLocation = new Vector3(pier.position.x, transform.position.y,pier.position.z);

                transform.LookAt(pierLocation);
            }
            else
            {
                GM.AtDock();
            }

            return;
        }

        if(!buildMode)
        {
            rb.MovePosition(rb.position + transform.forward * Time.fixedDeltaTime * moveSpeed);

            // Turn right
            if (velocity.x > 0)
            {
                rb.AddForceAtPosition(rightThruster.forward * turnSpeed, rightThruster.position, ForceMode.Impulse);

                if (moveSpeed > baseMoveSpeed / 2)
                {
                    moveSpeed -= 0.01f;
                }
            }
            // Turn left
            else if (velocity.x < 0)
            {
                rb.AddForceAtPosition(leftThruster.forward * turnSpeed, rightThruster.position, ForceMode.Impulse);

                if (moveSpeed > baseMoveSpeed / 2)
                {
                    moveSpeed -= 0.01f;
                }
            }
            else
            {
                if (moveSpeed < maxMoveSpeed)
                {
                    moveSpeed += 0.01f;
                }

                //myRotation = transform.rotation.z * 100f;

                //if (!Mathf.Approximately(myRotation, 0f))
                //{
                //    //Debug.Log("Turn back to default turn");
                //    //transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, defaultRotation * tiltSpeed) * Time.fixedDeltaTime);
                //}
            }
        }
    }

    private void LateUpdate()
    {
        //if (velocity.x != 0f)
        //{
        //    myRotation = transform.rotation.z * 100f;

        //    // Turning left
        //    if (targetVelocity.x < 0)
        //    {
        //        if (Mathf.Abs(myRotation) < maxRollValue)
        //        {
        //            transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, maxRollValue * 0.6f) * Time.deltaTime);
        //        }
        //    }
        //    // Turning Right
        //    else if (targetVelocity.x > 0)
        //    {
        //        if (Mathf.Abs(myRotation) < maxRollValue)
        //        {
        //            transform.Rotate(new Vector3(transform.rotation.x, transform.rotation.y, -maxRollValue * 0.6f) * Time.deltaTime);
        //        }
        //    }

        //    Debug.Log("Turning");

        //    transform.eulerAngles = new Vector3(0f, transform.eulerAngles.y, transform.eulerAngles.z);
        //}
    }

    public void setSpeedBonus(float _bonus)
    {
        bonusMoveSpeed = (_bonus * sailBonus);

        maxMoveSpeed = baseMoveSpeed + bonusMoveSpeed;
        moveSpeed = maxMoveSpeed;

        turnSpeed = baseTurnSpeed - (bonusMoveSpeed * TurnRatePenalty);
    }

    public void moveToPier(bool _moveThere, Transform _dockingPos)
    {
        movingToPier = _moveThere;

        pier = _dockingPos;

        if(movingToPier)
        {
            audioSource.PlayOneShot(startBuild[Random.Range(0, startBuild.Length)], Random.Range(0.9f, 1.3f));
        }
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
        buildMode = isBuildMode;

        rb.isKinematic = buildMode;
    }

    private void OnDestroy()
    {
        // Unsubscribe to game state
        GameState.buildModeChanged -= SetBuildMode;
    }
}