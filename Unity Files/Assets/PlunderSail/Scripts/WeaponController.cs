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

    private AudioSource audioSource;

    private PlayerController player;

    [Header("Combat Attributes")]
    [SerializeField]
    private float minFireTime;

    [SerializeField]
    private float maxFireTime;

    [SerializeField]
    private AudioClip[] FireShout;

    [Header("Debug Info")]
    [SerializeField]
    private WeaponAttachment[] leftWeapons;

    [SerializeField]
    private WeaponAttachment[] rightWeapons;

    private int currentShout = 0;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        player = GetComponent<PlayerController>();

        // Subscribe to game state
        GameState.buildModeChanged += SetBuildMode;
    }

    public WeaponAttachment[] LeftWeapons
    {
        get { return leftWeapons; }

        set { leftWeapons = value; }
    }

    public WeaponAttachment[] RightWeapons
    {
        get { return rightWeapons; }

        set { rightWeapons = value; }
    }

    public bool HasWeapons
    {
        get { return (rightWeapons.Length > 0 && leftWeapons.Length > 0); }
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
    }

    public void FireWeaponsRight()
    {
        if (canShootRight)
        {
            if (rightWeapons.Length > 0)
            {
                if (FireShout.Length > 0)
                {
                    currentShout = Random.Range(0, FireShout.Length);

                    if (!audioSource.isPlaying)
                    {
                        audioSource.PlayOneShot(FireShout[currentShout], 0.8F);
                    }
                }

                foreach (WeaponAttachment weapon in rightWeapons)
                {
                    if (weapon != null)
                    {
                        weapon.FireWeapon();
                    }
                }

                canShootRight = false;
                nextReloadTimeRight = Time.time + reloadTime;
            }
        }
    }

    public void FireWeaponsLeft()
    {
        // 
        if (canShootLeft)
        {
            if (leftWeapons.Length > 0)
            {
                // Play audio if any
                if (FireShout.Length > 0)
                {
                    currentShout = Random.Range(0, FireShout.Length);

                    if(!audioSource.isPlaying)
                    {
                        audioSource.PlayOneShot(FireShout[currentShout], 0.8F);
                    }
                }

                foreach (WeaponAttachment weapon in leftWeapons)
                {
                    if (weapon != null)
                    {
                        weapon.FireWeapon();
                    }
                }

                canShootLeft = false;
                nextReloadTimeLeft = Time.time + reloadTime;
            }
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
