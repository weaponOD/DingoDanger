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

    protected override void Awake()
    {
        base.Awake();

        skin = GetComponent<SkinnedMeshRenderer>();

        fullHPMat = skin.materials[0];

        anim = GetComponent<Animator>();
    }

    public override void TakeDamage(float _damage)
    {
        currentHealth -= _damage;

        // switch to broken mesh
        if (currentHealth < (maxHealth * healthWhenBroken))
        {
            if (brokenSail != null && !broken)
            {
                skin.materials[0] = brokenSail;
                broken = true;
            }
        }

        // destroyed 
        if (currentHealth <= 0)
        {
            transform.parent = null;

            entity.UpdateParts();

            if (!GetComponent<Rigidbody>())
            {
                gameObject.AddComponent<Rigidbody>();
                gameObject.GetComponent<Rigidbody>().AddExplosionForce(100, transform.position, 0.1f);
            }

            Destroy(gameObject, 3f);
        }
    }

    public override void RestoreHealth()
    {
        broken = false;
        smoking = false;
        skin.materials[0] = fullHPMat;
        currentHealth = maxHealth;
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