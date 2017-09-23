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
}