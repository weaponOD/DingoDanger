using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerTest : MonoBehaviour
{
    private bool sailsOpen = true;

    private GameManager GM;

    private Rigidbody rb;

    private ComponentManager components;

    [Header("Actual Values")]
    public float speed = 1.0f;
    public float acceleration = 1.0f;
    public float minspeed = 0.25f;
    public float maxspeed = 2.0f;
    public float heading = 0.0f;
    public float rudder = 0.0f;
    public float rudderDelta = 2.0f;
    public float maxRudder = 6.0f;
    public float rudderAngle = 0.0f;

    public float steering;

    private GameObject rudderControl;

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

            steering = Input.GetAxis("Horizontal") * rudderDelta * Time.deltaTime;

            if (steering != 0)
            {
                if (Mathf.Abs(rudder) < 3)
                {
                    rudder += steering;
                }
            }
            else
            {
                rudder = Mathf.MoveTowards(rudder, 0f, rudderDelta * 2 * Time.deltaTime);
            }


            // Sail
            speed += Input.GetAxis("Vertical") * acceleration * Time.deltaTime;

            if (speed > maxspeed)
            {

                speed = maxspeed;

            }
            else if (speed < minspeed)
            {

                speed = minspeed;

            }
        }
    }

    private void FixedUpdate()
    {
        if (!GameState.BuildMode)
        {
            heading = (heading + rudder * Time.deltaTime * signedSqrt(speed)) % 360;

            rb.MoveRotation(Quaternion.Euler(new Vector3(0f, heading, -rudder)));

            if (rudderControl)
            {
                rudderAngle = ((-60 * rudder) / maxRudder + heading) % 360;

                rudderControl.transform.eulerAngles = new Vector3(0, rudderAngle, 0);
            }

            rb.MovePosition(rb.position + transform.forward * speed * Time.fixedDeltaTime);
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
        if (rudder > maxRudder)
        {
            rudder = maxRudder;
        }
        else if (rudder < -maxRudder)
        {
            rudder = -maxRudder;
        }
    }

    void OnCollisionEnter(Collision c)
    {
        Vector3 dir = c.contacts[0].point - transform.position;

        dir = -dir.normalized;

        rb.AddForce(dir * (2 * speed));
    }

    private void OnDestroy()
    {
        // Unsubscribe to game state
        GameState.buildModeChanged -= SetBuildMode;
    }
}
