using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttachment : AttachmentBase
{
    [SerializeField]
    protected float minFireTime;

    [SerializeField]
    protected float maxFireTime;

    [Header("Effects resources")]
    [SerializeField]
    protected AudioClip[] shootSound;

    [SerializeField]
    protected ParticleSystem shootParticle;

    protected AudioSource audioSource;

    [SerializeField]
    protected bool facingLeft;

    [SerializeField]
    protected float damage;

    [SerializeField]
    protected float projectileForce;

    [SerializeField]
    protected GameObject projectilePrefab;

    protected Transform[] firePoints;

    protected Transform[] effectPoints;

    protected LivingEntity entity;

    protected override void Awake()
    {
        audioSource = transform.root.GetComponent<AudioSource>();

        if(transform.root.GetComponent<Player>())
        {
            entity = transform.root.GetComponent<Player>();
        }
        else
        {
            entity = transform.root.GetComponent<AIAgent>();
        }
    }

    public bool FacingLeft
    {
        get
        {
            facingLeft = (transform.localEulerAngles.y > 95f);
            return facingLeft;
        }
        set { facingLeft = value; }
    }

    public void FireWeapon()
    {
        StartCoroutine(Fire());
    }

    protected virtual IEnumerator Fire()
    {
        yield return null;
    }

    protected void PlayRandomSound()
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.volume = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(shootSound[Random.Range(0, shootSound.Length)], 0.4F);
    }
}