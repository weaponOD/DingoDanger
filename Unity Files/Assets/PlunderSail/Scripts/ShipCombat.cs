using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipCombat : MonoBehaviour
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

    private List<WeaponAttachment> leftWeapons;
    private List<WeaponAttachment> rightWeapons;

    private WeaponAttachment[] weapons;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        player = GetComponent<PlayerController>();

        leftWeapons = new List<WeaponAttachment>();
        rightWeapons = new List<WeaponAttachment>();

        // Subscribe to game state
        GameState.buildModeChanged += SetBuildMode;

        updateWeapons();
    }

    public void updateWeapons()
    {
        weapons = GetComponentsInChildren<WeaponAttachment>();

        foreach (WeaponAttachment weapon in weapons)
        {
            if (weapon.FacingLeft)
            {
                leftWeapons.Add(weapon);
            }
            else
            {
                rightWeapons.Add(weapon);
            }
        }
    }

    private void Update()
    {
        if(!GameState.BuildMode)
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
                if(leftWeapons.Count > 0)
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
                if(rightWeapons.Count > 0)
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

        foreach (WeaponAttachment weapon in leftWeapons)
        {
            weapon.FireLeft(new Vector3());
            yield return new WaitForSeconds(Random.Range(minFireTime, maxFireTime));
        }
    }

    private IEnumerator FireRight()
    {
        yield return new WaitForSeconds(FireShout.length);

        foreach (WeaponAttachment weapon in rightWeapons)
        {

            Debug.Log("Ship Combat still in use");
            weapon.FireRight(new Vector3());
            yield return new WaitForSeconds(Random.Range(minFireTime, maxFireTime));
        }
    }

    private void SetBuildMode(bool isBuildMode)
    {
        canShootRight = !isBuildMode;
        canShootLeft = !isBuildMode;
    }
}