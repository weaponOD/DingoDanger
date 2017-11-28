using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentSail : AttachmentBase
{
    private Animator anim;

    [SerializeField]
    protected Material brokenSail = null;

    protected SkinnedMeshRenderer skin;

    protected Material fullHPMat;

    protected TrailRenderer[] trails = null;

    protected override void Awake()
    {
        base.Awake();

        trails = GetComponentsInChildren<TrailRenderer>();

        anim = GetComponent<Animator>();
    }

    public override void RestoreHealth()
    {
        broken = false;
        smoking = false;
        //skin.materials[1] = fullHPMat;
        currentHealth = maxHealth;
    }

    public void ActivateTrails(bool _isFullSpeed)
    {
        trails[0].enabled = _isFullSpeed;
        trails[1].enabled = _isFullSpeed;
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