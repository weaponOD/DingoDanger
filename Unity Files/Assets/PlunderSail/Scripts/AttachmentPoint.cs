using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentPoint : MonoBehaviour
{
    Transform partOne = null;
    Transform partTwo = null;

    private void Awake()
    {
        //if(Physics.BoxCast(transform.position, transform.forward * 0.2f, transform.forward * 0.2f))
        //{
        //    gameObject.SetActive(false);
        //}
    }

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
}