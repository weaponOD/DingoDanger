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

    protected override void Awake()
    {
        base.Awake();

        anim = GetComponent<Animator>();
    }

    public void Raise()
    {
        Debug.Log("Raised");

        anim.SetBool("Open", true);
    }

    public void Lower()
    {
        Debug.Log("Lowered");

        anim.SetBool("Open", false);
    }
}