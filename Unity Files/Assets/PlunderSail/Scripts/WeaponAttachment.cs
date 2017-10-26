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

    protected bool facingLeft;

    protected bool needToRotate = false;

    protected bool needToMirror = false;

    [SerializeField]
    protected float damage;

    [SerializeField]
    protected GameObject projectilePrefab;

    protected Transform[] firePoints;

    protected Transform[] effectPoints;

    protected virtual void Awake()
    {
        audioSource = transform.root.GetComponent<AudioSource>();
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

    public void FireLeft(Vector3 _shipVelocity)
    {
        StartCoroutine(Fire(_shipVelocity));
    }

    public void FireRight(Vector3 _shipVelocity)
    {
        StartCoroutine(Fire(_shipVelocity));
    }

    protected virtual IEnumerator Fire(Vector3 _shipVelocity)
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