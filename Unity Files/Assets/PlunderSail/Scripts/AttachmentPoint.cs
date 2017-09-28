using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentPoint : MonoBehaviour
{
    Transform partOne = null;
    Transform partTwo = null;

    public Transform PartOne
    {
        get { return partOne; }
        set { partOne = value; }
    }

    public Transform PartTwo
    {
        get { return partTwo; }
        set
        {
            partTwo = value;

            if(partTwo == null)
            {
                gameObject.SetActive(true);
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        Debug.DrawLine(transform.position, transform.position + transform.forward * 0.6f, Color.red);

        if (Physics.Raycast(transform.position, transform.forward, 0.6f))
        {
            gameObject.SetActive(false);
        }

        if(transform.name.Contains("Point"))
        {
            Debug.DrawLine(transform.position, transform.position + transform.up * 0.6f, Color.red);
            if (Physics.Raycast(transform.position, transform.up, 0.6f))
            {
                gameObject.SetActive(false);
            }
        }
    }
}