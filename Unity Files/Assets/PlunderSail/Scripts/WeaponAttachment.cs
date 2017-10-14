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
    protected GameObject projectilePrefab;

    protected Transform[] firePoints;

    protected Transform[] effectPoints;

    protected virtual void Awake()
    {
        audioSource = transform.root.GetComponent<AudioSource>();
    }

    public bool NeedToMirror
    {
        set
        {
            needToMirror = value;
            if (needToMirror)
            {
                Mirror();
            }
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

    public void FireLeft()
    {
        StartCoroutine(Fire());
    }

    public void FireRight()
    {
        StartCoroutine(Fire());
    }

    protected virtual IEnumerator Fire()
    {
        Instantiate(projectilePrefab, firePoints[0].position, firePoints[0].rotation);

        Destroy(Instantiate(shootParticle.gameObject, firePoints[0].position, firePoints[0].rotation) as GameObject, shootParticle.main.startLifetime.constant);
        PlayRandomSound();

        yield return new WaitForSeconds(Random.Range(minFireTime, maxFireTime));

        Instantiate(projectilePrefab, firePoints[1].position, firePoints[1].rotation);

        Destroy(Instantiate(shootParticle.gameObject, firePoints[1].position, firePoints[1].rotation) as GameObject, shootParticle.main.startLifetime.constant);
        PlayRandomSound();

        yield return new WaitForSeconds(Random.Range(minFireTime, maxFireTime));

        Instantiate(projectilePrefab, firePoints[2].position, firePoints[2].rotation);

        Destroy(Instantiate(shootParticle.gameObject, firePoints[2].position, firePoints[1].rotation) as GameObject, shootParticle.main.startLifetime.constant);
        PlayRandomSound();
    }

    protected void PlayRandomSound()
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.volume = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(shootSound[Random.Range(0, shootSound.Length)], 0.4F);
    }
}