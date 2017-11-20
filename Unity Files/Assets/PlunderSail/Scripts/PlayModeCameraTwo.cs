using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayModeCameraTwo : MonoBehaviour
{
    [Header("Right Stick Rotation")]
    [SerializeField]
    [Tooltip("How much the camera will move when the mouse moves")]
    [Range(1, 20)]
    private float baseMouseSensitivity = 2.5f;

    private float mouseSensitivity = 2.5f;

    [SerializeField]
    [Tooltip("Highest angle the camera can go from the horizon")]
    private float MaxClamp = 60f;

    [SerializeField]
    [Tooltip("Lowest angle the camera can go to the horizon")]
    private float minclamp = 5f;

    [SerializeField]
    [Tooltip("How long it takes for the camera to reach it's destination")]
    private float orbitDampening = 10f;

    [SerializeField]
    private float FovWhenFast = 0;

    [SerializeField]
    private float zoomOutRate = 0;

    [SerializeField]
    private float zoomInRate = 0;

    private float defaultFoV = 0;

    private Transform target;

    private bool aiming = false;

    [SerializeField]
    private bool invertedX = false;

    private bool invertedY = false;

    [SerializeField]
    private Vector3 offset;

    private Transform pivot;

    private Camera myCamera;

    [SerializeField]
    private Vector3 savedRotation;

    [SerializeField]
    private Vector3 localRotation;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

        pivot = transform.parent;

        localRotation = new Vector3(pivot.rotation.eulerAngles.y, pivot.rotation.eulerAngles.x, 0f);

        myCamera = GetComponent<Camera>();

        defaultFoV = myCamera.fieldOfView;

        mouseSensitivity = baseMouseSensitivity;
    }

    // Use this for initialization
    void Start()
    {
        localRotation = new Vector3(pivot.rotation.eulerAngles.y, pivot.rotation.eulerAngles.x, 0f);
    }


    private void OnEnable()
    {
        localRotation.x = target.localEulerAngles.y;
    }

    private void Update()
    {
        if (!GameState.Paused)
        {
            // Rotate the camera based on right thumb stick input
            if (Input.GetAxis("Mouse_X") != 0 || Input.GetAxis("Mouse_Y") != 0)
            {
                if (!aiming)
                {
                    if (invertedX)
                    {
                        localRotation.x += Input.GetAxis("Mouse_X") * mouseSensitivity;
                    }
                    else
                    {
                        localRotation.x -= Input.GetAxis("Mouse_X") * mouseSensitivity;
                    }

                    if (invertedY)
                    {
                        localRotation.y += Input.GetAxis("Mouse_Y") * mouseSensitivity;
                    }
                    else
                    {
                        localRotation.y -= Input.GetAxis("Mouse_Y") * mouseSensitivity;
                    }
                }

                // Clamp the Y rotation to horizon and not flipping it over at the top
                localRotation.y = Mathf.Clamp(localRotation.y, minclamp, MaxClamp);
            }
        }
    }

    public void AimRight()
    {// only done the first time aimRight is called
        if (!aiming)
        {
            savedRotation = localRotation;
        }

        aiming = true;

        localRotation.x = target.localEulerAngles.y + 90;
        localRotation.y = 15;
    }

    public void AimLeft()
    {
        // only done the first time aimLeft is called
        if(!aiming)
        {
            savedRotation = localRotation;
        }

        aiming = true;

        localRotation.x = target.localEulerAngles.y - 90;
        localRotation.y = 15;
    }

    public void CancelAim()
    {
        // only done the first time cancelAim is called
        if (aiming)
        {
            localRotation = savedRotation;
        }

        aiming = false;
    }

    public void SetSensitivity(float _value)
    {
        mouseSensitivity = baseMouseSensitivity * _value;
    }

    public void FastMode(bool _isFast)
    {
        if (_isFast)
        {
            StartCoroutine("ZoomOut");
        }
        else
        {
            StartCoroutine("ZoomIn");
        }
    }

    public float CalculatePerspectiveAngle()
    {
        Quaternion relative = Quaternion.Inverse(target.transform.rotation) * transform.root.rotation;

        float rawAngle = relative.eulerAngles.y;

        return rawAngle;
    }

    private IEnumerator ZoomOut()
    {
        while (myCamera.fieldOfView < FovWhenFast)
        {
            myCamera.fieldOfView += zoomOutRate;
            yield return null;
        }
    }

    public bool InvertX
    {
        get { return invertedX; }
        set { invertedX = value; }
    }

    public bool InvertY
    {
        get { return invertedY; }
        set { invertedY = value; }
    }

    private IEnumerator ZoomIn()
    {
        while (myCamera.fieldOfView > defaultFoV)
        {
            myCamera.fieldOfView -= zoomInRate;
            yield return null;
        }
    }

    private void LateUpdate()
    {
        pivot.position = target.position + target.forward * offset.z + target.right * offset.x + target.up * offset.y;


        // Camera Rotation using the right thumb stick
        Quaternion targetRotation = Quaternion.Euler(localRotation.y, localRotation.x, 0f);
        pivot.rotation = Quaternion.Lerp(pivot.rotation, targetRotation, Time.deltaTime * orbitDampening);
    }
}