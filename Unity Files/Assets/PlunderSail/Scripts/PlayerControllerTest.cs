using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTest : MonoBehaviour
{
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
    private float maxRoll;

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


    // The stearing wheel on the ship
    private Transform wheel;

    private AudioSource audioSource;

    private GameManager GM;

    private ComponentManager components;

    private GameObject rudderControl;

    private Rigidbody rb;


    public float heading = 0.0f;
    public float rudder = 0.0f;
    public float maxRudder = 6.0f;
    public float rudderAngle = 0.0f;

    private float steering;

    private void Awake()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        components = GetComponent<ComponentManager>();

        rb = GetComponent<Rigidbody>();

        rudderControl = GameObject.Find("rudderControl");

        // Subscribe to game state
        GameState.buildModeChanged += SetBuildMode;
    }

    private void Update()
    {
        if (!GameState.BuildMode)
        {
            // steering
            steering = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;

            if (steering != 0)
            {
                if (Mathf.Abs(rudder) < maxRoll)
                {
                    rudder += steering;
                }
            }
            else
            {
                rudder = Mathf.MoveTowards(rudder, 0f, turnSpeed * 2 * Time.deltaTime);
            }

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
            heading = (heading + rudder * Time.deltaTime * signedSqrt(moveSpeed)) % 360;

            rb.MoveRotation(Quaternion.Euler(new Vector3(0f, heading, -rudder)));

            if (rudderControl)
            {
                rudderAngle = ((-60 * rudder) / maxRudder + heading) % 360;

                rudderControl.transform.eulerAngles = new Vector3(0, rudderAngle, 0);
            }

            if (rudder > maxRudder)
            {
                rudder = maxRudder;
            }
            else if (rudder < -maxRudder)
            {
                rudder = -maxRudder;
            }

            rb.MovePosition(rb.position + transform.forward * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private float signedSqrt(float _input)
    {
        float output = Mathf.Sqrt(Mathf.Abs(_input));

        if (_input < 0)
        {
            return -_input;
        }
        else
        {
            return _input;
        }
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

    private void LowerSails(bool _isOpen)
    {
        if (_isOpen)
        {
            moveSpeed = maxMoveSpeed;

            turnSpeed = baseTurnSpeed - (bonusMoveSpeed * TurnRatePenalty);

            if(turnSpeed < 0)
            {
                turnSpeed = 0;
            }

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

    void OnCollisionEnter(Collision c)
    {
        Vector3 dir = c.contacts[0].point - transform.position;

        dir = -dir.normalized;

        rb.AddForce(dir * (5 * moveSpeed));
    }

    private void OnDestroy()
    {
        // Unsubscribe to game state
        GameState.buildModeChanged -= SetBuildMode;
    }
}
