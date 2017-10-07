using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class WeaponController : MonoBehaviour
{
    private float reloadTime = 2f;
    private float nextReloadTimeRight = 0;
    private float nextReloadTimeLeft = 0;

    private bool canShootRight = true;
    private bool canShootLeft = true;

    [SerializeField]
    private float minFireTime;

    [SerializeField]
    private float maxFireTime;

    private AudioSource audioSource;

    private PlayerController player;

    [SerializeField]
    private AudioClip FireShout;

    [SerializeField]
    private AttachmentWeapon[] leftWeapons;

    [SerializeField]
    private AttachmentWeapon[] rightWeapons;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        player = GetComponent<PlayerController>();

        // Subscribe to game state
        GameState.buildModeChanged += SetBuildMode;
    }

    public AttachmentWeapon[] LeftWeapons
    {
        set { leftWeapons = value; }
    }

    public AttachmentWeapon[] RightWeapons
    {
        set { rightWeapons = value; }
    }

    private void Update()
    {
        if (!GameState.BuildMode)
        {
            if (Time.time > nextReloadTimeRight)
            {
                canShootRight = true;
            }

            if (Time.time > nextReloadTimeLeft)
            {
                canShootLeft = true;
            }
        }

        if (Input.GetAxis("Left_Trigger") == 1)
        {
            if (canShootLeft)
            {
                if (leftWeapons.Length > 0)
                {
                    audioSource.PlayOneShot(FireShout, 0.8F);
                    StartCoroutine(FireLeft());

                    canShootLeft = false;
                    nextReloadTimeLeft = Time.time + reloadTime;
                }
            }
        }

        if (Input.GetAxis("Right_Trigger") == 1)
        {
            if (canShootRight)
            {
                if (rightWeapons.Length > 0)
                {
                    audioSource.PlayOneShot(FireShout, 0.8F);

                    StartCoroutine(FireRight());

                    canShootRight = false;
                    nextReloadTimeRight = Time.time + reloadTime;
                }
            }
        }
    }

    private IEnumerator FireLeft()
    {
        yield return new WaitForSeconds(FireShout.length);

        foreach (AttachmentWeapon weapon in leftWeapons)
        {
            weapon.FireLeft();
            yield return new WaitForSeconds(Random.Range(minFireTime, maxFireTime));
        }
    }

    private IEnumerator FireRight()
    {
        yield return new WaitForSeconds(FireShout.length);

        foreach (AttachmentWeapon weapon in rightWeapons)
        {
            weapon.FireRight();
            yield return new WaitForSeconds(Random.Range(minFireTime, maxFireTime));
        }
    }

    private void SetBuildMode(bool isBuildMode)
    {
        canShootRight = !isBuildMode;
        canShootLeft = !isBuildMode;
    }

    private void OnDestroy()
    {
        // Unsubscribe to game state
        GameState.buildModeChanged -= SetBuildMode;
    }
}
