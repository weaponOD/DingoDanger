using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum SailingState { IDLE, SLOW, FAST };

    [Header("Movement Attributes")]
    [SerializeField]
    [Tooltip("The degrees per frame the ship can rotate with no open sails")]
    private float baseTurnSpeed;

    [SerializeField]
    [Tooltip("The change in speed per second while speeding up")]
    private float acceleration;

    [SerializeField]
    [Tooltip("The change in speed per second while slowing down")]
    private float deacceleration;

    [SerializeField]
    private float maxSpeed = 45.0f;

    [SerializeField]
    [Tooltip("The number of seconds that the player does not move forward after a collision")]
    private float stunDuration = 0;

    private float maxRudder = 6.0f;

    private float maxRoll = 12;

    [Header("Speed Modifiers")]
    [Tooltip("The additional speed per sail when the sails are open")]
    [SerializeField]
    private float bonusSpeed;

    [Tooltip("how much less speed each sail gives you")]
    [SerializeField]
    private float bonusDropoff;

    [Tooltip("how much less speed each sail gives you")]
    [SerializeField]
    private float minBonusSpeed;

    [SerializeField]
    private float cannonWeight = 0;

    [SerializeField]
    private float slowMaxSpeedPercent = 0;

    [SerializeField]
    private float slowAccelerationPercent = 0;

    [Header("Turn Speed Attributes")]

    [SerializeField]
    private float turnSpeedDecay;

    [SerializeField]
    private float turnSpeedGrowth;

    [SerializeField]
    private float minTurnSpeed;

    [SerializeField]
    private float maxTurnSpeed;

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
    private float maxMoveSpeed;

    [SerializeField]
    private float turnSpeed;

    [SerializeField]
    private SailingState sailState = SailingState.IDLE;

    [SerializeField]
    private Vector3 velocity;

    // The stearing wheel on the ship
    private Transform wheel;

    private AudioSource audioSource;

    private GameManager GM;

    private ComponentManager components;

    private GameObject rudderControl;

    private Rigidbody rb;

    [SerializeField]
    private float heading = 0.0f;
    [SerializeField]
    private float rudder = 0.0f;
    [SerializeField]
    private float rudderAngle = 0.0f;

    private float totalBonusSpeed;

    [SerializeField]
    private float steering;

    private Vector3 previousPos;

    private bool aiming = false;

    private bool stunned = false;

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
            if (!aiming)
            {
                steering = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
            }
            else
            {
                steering = 0;
            }

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
                if ((int)sailState < 2)
                {
                    sailState++;
                }
                else
                {
                    sailState = SailingState.IDLE;
                }

                SetSailsToState(sailState);
            }

            if (Time.deltaTime != 0)
            {
                velocity = (transform.position - previousPos) / Time.deltaTime;
                previousPos = transform.position;
            }
        }
    }

    public bool Aiming
    {
        set { aiming = value; }
    }

    private void FixedUpdate()
    {
        if (!GameState.BuildMode)
        {
            heading = (heading + rudder * Time.deltaTime * signedSqrt(turnSpeed)) % 360;

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

            if (sailState == SailingState.IDLE)
            {
                if (moveSpeed > 0)
                {
                    moveSpeed -= deacceleration * Time.fixedDeltaTime;
                }
                else
                {
                    moveSpeed = 0f;
                }

                if (turnSpeed < maxTurnSpeed)
                {
                    turnSpeed += turnSpeedGrowth * Time.fixedDeltaTime;
                }
                else
                {
                    turnSpeed = maxTurnSpeed;
                }
            }
            else if (sailState == SailingState.SLOW)
            {
                if (moveSpeed < (maxMoveSpeed * slowMaxSpeedPercent))
                {
                    moveSpeed += (acceleration * slowAccelerationPercent) * Time.fixedDeltaTime;
                }
                else
                {
                    moveSpeed = maxMoveSpeed;
                }

                if (turnSpeed > minTurnSpeed)
                {
                    turnSpeed -= turnSpeedDecay * Time.fixedDeltaTime;
                }
                else
                {
                    turnSpeed = minTurnSpeed;
                }
            }
            else if (sailState == SailingState.FAST)
            {
                if (moveSpeed < maxMoveSpeed)
                {
                    moveSpeed += acceleration * Time.fixedDeltaTime;
                }
                else
                {
                    moveSpeed = maxMoveSpeed;
                }

                if (turnSpeed > minTurnSpeed)
                {
                    turnSpeed -= turnSpeedDecay * Time.fixedDeltaTime;
                }
                else
                {
                    turnSpeed = minTurnSpeed;
                }
            }

            if (!stunned)
            {
                rb.MovePosition(rb.position + transform.forward * moveSpeed * Time.fixedDeltaTime);
            }
        }
    }

    public Vector3 Velocity
    {
        get { return velocity; }
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
            sailState = SailingState.SLOW;

            SetSailsToState(sailState);
        }
    }

    public void ResetHeading()
    {
        heading = transform.root.localEulerAngles.y;
    }

    private void SetSailsToState(SailingState _state)
    {
        if (_state == SailingState.IDLE)
        {
            if (fullSpeed.Length > 0 && !GameState.BuildMode)
            {
                audioSource.PlayOneShot(fullSpeed[Random.Range(0, fullSpeed.Length)], Random.Range(0.9f, 1.3f));
            }

            components.RaiseSails();
        }
        else if (_state == SailingState.SLOW)
        {
            if (slowDown.Length > 0)
            {
                audioSource.PlayOneShot(slowDown[Random.Range(0, fullSpeed.Length)], Random.Range(0.9f, 1.3f));
            }

            components.LowerSails();
        }
        else if (_state == SailingState.FAST)
        {
            if (slowDown.Length > 0)
            {
                audioSource.PlayOneShot(slowDown[Random.Range(0, fullSpeed.Length)], Random.Range(0.9f, 1.3f));
            }

            components.LowerSails();
        }
    }

    public void setSpeedBonus(float _numOfSails)
    {
        totalBonusSpeed = 0;

        for (int x = 0; x < _numOfSails; x++)
        {
            totalBonusSpeed += Mathf.Clamp(bonusSpeed - (bonusDropoff * x), minBonusSpeed, bonusSpeed);
        }

        totalBonusSpeed -= (cannonWeight * components.GetAttachedLeftWeapons().Length) + (cannonWeight * components.GetAttachedRightWeapons().Length);

        maxMoveSpeed = Mathf.Clamp(totalBonusSpeed, 0f, maxSpeed);
    }

    void OnCollisionEnter(Collision c)
    {
        Vector3 dir = c.contacts[0].point - transform.position;

        dir = -dir.normalized;

        rb.AddForce(dir * (5 * moveSpeed));
    }

    private void RemoveStun()
    {
        stunned = false;
    }

    public void AddStun()
    {
        stunned = true;

        Invoke("RemoveStun", stunDuration);
    }

    public Vector3 Speed
    {
        get { return velocity; }
    }

    private void OnDestroy()
    {
        // Unsubscribe to game state
        GameState.buildModeChanged -= SetBuildMode;
    }
}
