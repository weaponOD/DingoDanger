using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Attributes")]

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

    private float maxRudder = 6.0f;

    [SerializeField]
    private float maxRoll;

    [Header("sail bonuses")]
    [Tooltip("The additional speed per sail when the sails are open")]
    [SerializeField]
    private float bonusSpeed;

    [Tooltip("how much less speed each sail gives you")]
    [SerializeField]
    private float bonusDropoff;

    [Tooltip("how much less speed each sail gives you")]
    [SerializeField]
    private float minBonusSpeed;

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
    private bool sailsOpen = true;

    [SerializeField]
    private Vector3 velocity;

    // The stearing wheel on the ship
    private Transform wheel;

    private AudioSource audioSource;

    private GameManager GM;

    private ComponentManager components;

    private GameObject rudderControl;

    private Rigidbody rb;

    private buoyancy buoyant;

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

    private void Awake()
    {
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        components = GetComponent<ComponentManager>();

        rb = GetComponent<Rigidbody>();

        buoyant = GetComponentInChildren<buoyancy>();

        rudderControl = GameObject.Find("rudderControl");

        // Subscribe to game state
        GameState.buildModeChanged += SetBuildMode;
    }

    private void Update()
    {
        if (!GameState.BuildMode)
        {
            // steering
            if(!aiming)
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
                sailsOpen = !sailsOpen;

                LowerSails(sailsOpen);
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

            if (sailsOpen)
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
            else
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

            rb.MovePosition(rb.position + transform.forward * moveSpeed * Time.fixedDeltaTime);
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
            sailsOpen = true;

            LowerSails(sailsOpen);
        }
    }

    public void ResetHeading()
    {
        heading = transform.root.localEulerAngles.y;
    }

    private void LowerSails(bool _isOpen)
    {
        if (_isOpen)
        {
            if (fullSpeed.Length > 0 && !GameState.BuildMode)
            {
                audioSource.PlayOneShot(fullSpeed[Random.Range(0, fullSpeed.Length)], Random.Range(0.9f, 1.3f));
            }

            components.LowerSails();
        }
        else
        {
            if (slowDown.Length > 0)
            {
                audioSource.PlayOneShot(slowDown[Random.Range(0, fullSpeed.Length)], Random.Range(0.9f, 1.3f));
            }

            components.RaiseSails();
        }

        //turnSpeed = baseTurnSpeed * (maxMoveSpeed - moveSpeed);

        //if (turnSpeed <= 0)
        //{
        //    turnSpeed = 1;
        //}
    }

    public void Buoyant(bool _isBuoyant)
    {
        //buoyant.enabled = _isBuoyant;
    }

    public void setSpeedBonus(float _numOfSails)
    {
        totalBonusSpeed = 0;

        for (int x = 0; x < _numOfSails; x++)
        {
            totalBonusSpeed += Mathf.Clamp(bonusSpeed - (bonusDropoff * x), minBonusSpeed, bonusSpeed);
        }

        maxMoveSpeed = Mathf.Clamp(totalBonusSpeed, 0f, maxSpeed);
    }

    void OnCollisionEnter(Collision c)
    {
        Vector3 dir = c.contacts[0].point - transform.position;

        dir = -dir.normalized;

        rb.AddForce(dir * (5 * moveSpeed));
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
