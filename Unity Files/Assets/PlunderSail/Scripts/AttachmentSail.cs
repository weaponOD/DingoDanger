using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentSail : AttachmentBase
{
    private Animator anim;

    protected override void Awake()
    {
        base.Awake();

        anim = GetComponent<Animator>();
    }

    public void Raise()
    {
        anim.SetBool("Open", true);
    }

    public void Lower()
    {
        anim.SetBool("Open", false);
    }
}