using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentWeapon : AttachmentBase
{
    [SerializeField]
    protected bool facingLeft;

    [SerializeField]
    protected bool doubleFacing = false;

    [SerializeField]
    private float minFireTime;

    [SerializeField]
    private float maxFireTime;

    [SerializeField]
    private AudioClip[] shootSound;

    private AudioSource audioSource;

    public bool needToRotate = false;

    public bool needToMirror = false;

    [SerializeField]
    protected GameObject projectilePrefab;
    protected Transform[] firePoints;

    private void Awake()
    {
        firePoints = new Transform[3];

        audioSource = transform.root.GetComponent<AudioSource>();

        firePoints[0] = transform.GetChild(0).GetChild(0).transform;
        firePoints[1] = transform.GetChild(0).GetChild(1).transform;
        firePoints[2] = transform.GetChild(0).GetChild(2).transform;
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

    public bool DoubleFacing
    {
        get { return doubleFacing; }
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

    private IEnumerator Fire()
    {
        Instantiate(projectilePrefab, firePoints[0].position, firePoints[0].rotation);
        PlayRandomSound();

        yield return new WaitForSeconds(Random.Range(minFireTime, maxFireTime));

        Instantiate(projectilePrefab, firePoints[1].position, firePoints[1].rotation);
        PlayRandomSound();

        yield return new WaitForSeconds(Random.Range(minFireTime, maxFireTime));

        Instantiate(projectilePrefab, firePoints[2].position, firePoints[2].rotation);
        PlayRandomSound();
    }

    private void PlayRandomSound()
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.volume = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(shootSound[Random.Range(0, shootSound.Length)], 0.4F);
    }
}