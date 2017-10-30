using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentSail : AttachmentBase
{
    private RaycastHit hitInfo;

    [SerializeField]
    private Material greenMat;

    [SerializeField]
    private Material redMat;

    [SerializeField]
    private bool currentlyGreen = true;

    private Vector3 lastPos;

    private Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Raise()
    {
        Debug.Log("Raised");

        anim.SetBool("Raised", true);
    }

    public void Lower()
    {
        Debug.Log("Lowered");

        anim.SetBool("Raised", false);
    }
}