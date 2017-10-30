using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntity
{
    [SerializeField]
    private int gold = 2000;

    [SerializeField]
    private float ramDamage = 20;

    PlayerController controller;

    private AudioSource audioSource;

    [SerializeField]
    private Vector3 velocity;

    [SerializeField]
    private AudioClip[] goldPickup;

    [SerializeField]
    private AudioClip waves;

    [Header("Arc Creator")]
    [SerializeField]
    private GameObject arc;

    private bool aiming = false;

    private LaunchArcMesh[] aimers = null;

    protected override void Start()
    {
        base.Start();
    }

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        weaponController = GetComponent<WeaponController>();
        components = GetComponent<ComponentManager>();

        audioSource = GetComponent<AudioSource>();

        // Subscribe to game state
        GameState.buildModeChanged += SetBuildMode;
    }

    private void Update()
    {
        if (Input.GetAxis("Left_Trigger") == 1)
        {
            if (!aiming)
            {
                Aim(true);
            }
        }
        else
        {
            if (aiming)
            {
                Aim(false);
            }
        }

        if (Input.GetAxis("Right_Trigger") == 1)
        {
            weaponController.FireWeaponsRight(controller.Speed);
            weaponController.FireWeaponsLeft(controller.Speed);
        }
    }


    private void Aim(bool _isAiming)
    {
        aiming = _isAiming;

        if (aimers == null)
        {
            aimers = new LaunchArcMesh[2];

            aimers[0] = Instantiate(arc, transform.position - transform.right * 3 + transform.forward * 2, Quaternion.LookRotation(-transform.right), transform).GetComponent<LaunchArcMesh>();
            aimers[1] = Instantiate(arc, transform.position + transform.right * 3 + transform.forward * 2, Quaternion.LookRotation(transform.right), transform).GetComponent<LaunchArcMesh>();

            aimers[0].gameObject.SetActive(false);
            aimers[1].gameObject.SetActive(false);
        }

        if (weaponController.LeftWeapons.Length > 0)
        {
            //aimers[1].angle = Mathf.Clamp(transform.root.rotation.eulerAngles.z, 0, 85);
            aimers[1].gameObject.SetActive(aiming);
        }

        if (weaponController.RightWeapons.Length > 0)
        {
            //aimers[0].angle = Mathf.Clamp(transform.root.rotation.eulerAngles.z, 0, 85);
            aimers[0].gameObject.SetActive(aiming);
        }
    }

    public int Gold
    {
        get { return gold; }
    }

    public void DeductGold(int _amount)
    {
        gold -= _amount;
    }

    public void GiveGold(int _amount)
    {
        if (goldPickup.Length > 0)
        {
            audioSource.PlayOneShot(goldPickup[Random.Range(0, goldPickup.Length)], Random.Range(0.9f, 1.3f));
        }

        gold += _amount;
    }

    private void SetBuildMode(bool isBuildMode)
    {
        if (!isBuildMode)
        {
            UpdateAttachments();
        }
    }

    public void UpdateAttachments()
    {
        weaponController.LeftWeapons = components.GetAttachedLeftWeapons();
        weaponController.RightWeapons = components.GetAttachedRightWeapons();
        controller.setSpeedBonus(components.getSpeedBonus());
    }

    private void OnDestroy()
    {
        // Unsubscribe to game state
        GameState.buildModeChanged -= SetBuildMode;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.contacts[0].thisCollider.CompareTag("Ram"))
        {
            float hitDamage = collision.relativeVelocity.magnitude;
            Debug.Log("Hit with Ram with a force of " + hitDamage);

            if (collision.collider.gameObject.GetComponent<AttachmentBase>())
            {
                collision.collider.gameObject.GetComponent<AttachmentBase>().TakeDamage(ramDamage);
            }

            if (collision.collider.gameObject.GetComponent<AIAgent>())
            {
                collision.collider.gameObject.GetComponent<AIAgent>().TakeDamage(ramDamage);
            }
        }

        if (collision.contacts[0].thisCollider.gameObject.GetComponent<AttachmentBase>())
        {
            float hitDamage = collision.relativeVelocity.magnitude;
            Debug.Log("Hit piece with a force of " + hitDamage);

            if (hitDamage < 10)
            {
                collision.contacts[0].thisCollider.gameObject.GetComponent<AttachmentBase>().TakeDamage(5 * collision.relativeVelocity.magnitude);
            }
            else
            {
                collision.contacts[0].thisCollider.gameObject.GetComponent<AttachmentBase>().TakeDamage(100 * collision.relativeVelocity.magnitude);
            }
        }
        else
        {
            float hitDamage = collision.relativeVelocity.magnitude;
            Debug.Log("Hit hull with a force of " + hitDamage);
        }
    }
}