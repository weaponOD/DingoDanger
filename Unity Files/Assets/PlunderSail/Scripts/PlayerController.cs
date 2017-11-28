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
    private float moveSpeedCap = 45.0f;

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
    private float armourWeight = 0;

    [SerializeField]
    private float slowMaxSpeedPercent = 0;

    [SerializeField]
    private float slowAccelerationPercent = 0;

    [SerializeField]
    private float idleHoldTime = 2;

    [Header("Turn Speed Attributes")]

    [SerializeField]
    private float turnSpeedDecay;

    [SerializeField]
    private float turnSpeedGrowth;

    [SerializeField]
    private float minTurnSpeed;

    [SerializeField]
    private float maxTurnSpeed;

    [Header("Sound Clip Names")]

    [SerializeField]
    private string fullSpeedSound = "default Value";

    [SerializeField]
    private string slowDownSound = "default Value";

    [Header("Wheel and Rudder")]
    [SerializeField]
    private Transform wheelVisual = null;

    [SerializeField]
    private Transform rudderVisual = null;

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

    private ComponentManager components;

    private GameObject rudderControl;

    private Rigidbody rb;

    private CameraController CC;

    [SerializeField]
    private float heading = 0.0f;
    [SerializeField]
    private float rudder = 0.0f;
    [SerializeField]
    private float rudderAngle = 0.0f;

    private float totalBonusSpeed;

    private float idleChargeUp;

    private bool canMove = false;

    [SerializeField]
    private float steering;

    private Vector3 previousPos;

    private UIController UI = null;

    private bool stunned = false;

    private void Awake()
    {
        components = GetComponent<ComponentManager>();

        CC = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CameraController>();

        UI = CC.gameObject.GetComponent<UIController>();

        rb = GetComponent<Rigidbody>();

        rudderControl = GameObject.Find("rudderControl");

        // Subscribe to game state
        GameState.buildModeChanged += SetBuildMode;
    }

    private void Update()
    {
        if (!GameState.BuildMode && !GameState.Paused && canMove)
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

            // Switch between fast and slow mode
            if (Input.GetButtonDown("A_Button"))
            {
                if (components.getSpeedBonus() > 0)
                {
                    if (sailState == SailingState.SLOW)
                    {
                        sailState = SailingState.FAST;
                    }
                    else
                    {
                        sailState = SailingState.SLOW;
                    }

                    SetSailsToState(sailState);
                }
            }

            if(Input.GetButton("B_Button"))
            {
                idleChargeUp += 1 * Time.deltaTime;

                if(idleChargeUp > idleHoldTime)
                {
                    if(sailState != SailingState.IDLE)
                    {
                        sailState = SailingState.IDLE;

                        SetSailsToState(sailState);
                    }
                }
            }
            else
            {
                idleChargeUp = 0;
            }

            if (Time.deltaTime != 0)
            {
                velocity = (transform.position - previousPos) / Time.deltaTime;
                previousPos = transform.position;
            }
        }
    }

    public bool CanMove
    {
        set { canMove = value; }
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
                else if (moveSpeed > (maxMoveSpeed * slowMaxSpeedPercent) + 1)
                {
                    moveSpeed -= deacceleration * Time.fixedDeltaTime;
                }
                else
                {
                    moveSpeed = (maxMoveSpeed * slowMaxSpeedPercent);
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
                else if (moveSpeed > maxMoveSpeed + 1)
                {
                    moveSpeed -= deacceleration * Time.fixedDeltaTime;
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
        if (_input < 0)
        {
            return -_input;
        }
        else
        {
            return _input;
        }
    }

    public void Stop()
    {
        moveSpeed = 0;
        maxMoveSpeed = 0;
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
        // Sail state can be set to slow if it's build mode
        if (_state == SailingState.SLOW)
        {
            if (!GameState.BuildMode)
            {
                AudioManager.instance.PlaySound(slowDownSound);
            }

            UI.GoingFast(false);

            components.LowerSails();
        }

        // the other two states cannont be accessed if it's in build mode
        if (!GameState.BuildMode)
        {
            if (_state == SailingState.IDLE)
            {
                components.RaiseSails();

                components.FullSpeed(false);
                UI.GoingFast(false);
                CC.DisableFastMode();
            }
            else if(_state == SailingState.SLOW)
            {
                CC.DisableFastMode();
                UI.GoingFast(false);

                components.FullSpeed(false);
            }
            else if (_state == SailingState.FAST)
            {
                AudioManager.instance.PlaySound(fullSpeedSound);

                components.LowerSails();

                components.FullSpeed(true);

                UI.GoingFast(true);

                CC.EnableFastMode();
            }
        }
    }

    public void setSpeedBonus(float _numOfSails)
    {
        totalBonusSpeed = 0;

        for (int x = 0; x < _numOfSails; x++)
        {
            totalBonusSpeed += Mathf.Clamp(bonusSpeed - (bonusDropoff * x), minBonusSpeed, bonusSpeed);
        }

        float totalCannonWeight = (cannonWeight * components.GetAttachedLeftWeapons().Length) + (cannonWeight * components.GetAttachedRightWeapons().Length);

        float totalArmourWeight = (armourWeight * components.Armour);

        totalBonusSpeed -= (totalCannonWeight + totalArmourWeight);

        maxMoveSpeed = Mathf.Clamp(totalBonusSpeed, 0f, moveSpeedCap);
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

    public void AddStun(float _duration)
    {
        stunned = true;

        moveSpeed = 0;

        Invoke("RemoveStun", _duration);
    }

    public float CappedSpeed
    {
        get { return moveSpeedCap; }
    }

    public float MaxSpeed
    {
        get { return maxMoveSpeed; }
    }

    public bool HasSails
    {
        get
        {
            return (components.GetAttachedSails().Length > 0);
        }
    }

    public bool HasCannons
    {
        get
        {
            return (components.HasWeapons);
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe to game state
        GameState.buildModeChanged -= SetBuildMode;
    }
}
