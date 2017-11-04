using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntity
{
    [SerializeField]
    private int gold = 2000;

    [SerializeField]
    private float ramDamage = 20;

    [SerializeField]
    private float KnockBackForce;

    PlayerController controller;

    private AudioSource audioSource;

    [SerializeField]
    private AudioClip[] goldPickup;

    [Header("Arc Creator")]
    [SerializeField]
    private GameObject arc;

    private bool aiming = false;

    private Vector3 aimTarget;

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
            weaponController.FireWeaponsRight();
            weaponController.FireWeaponsLeft();
        }

        if (aiming)
        {
            aimTarget = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

            foreach (LaunchArcMesh arc in aimers)
            {
                // arc.end += aimTarget;
            }
        }

        velocity = controller.Speed;
    }

    private void Aim(bool _isAiming)
    {
        aiming = _isAiming;

        controller.Aiming = aiming;

        if (aimers == null)
        {
            WeaponAttachment[] leftWeapons = weaponController.LeftWeapons;

            aimers = new LaunchArcMesh[leftWeapons.Length];

            int count = 0;

            foreach (WeaponAttachment weapon in leftWeapons)
            {
                aimers[count] = Instantiate(arc, weapon.transform.position, Quaternion.LookRotation(-transform.right), transform).GetComponent<LaunchArcMesh>();
                count++;
            }

            foreach (LaunchArcMesh arc in aimers)
            {
                arc.gameObject.SetActive(false);
                // aimTarget = arc.end;
            }
        }

        if (weaponController.LeftWeapons.Length > 0)
        {
            foreach (LaunchArcMesh arc in aimers)
            {
                arc.gameObject.SetActive(aiming);
            }
        }

        if (weaponController.RightWeapons.Length > 0)
        {
            //aimers[0].gameObject.SetActive(aiming);
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
        Debug.Log("Updating Attachments");

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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            controller.AddStun();

            // calculate force vector
            var force = transform.position - collision.transform.position;
            // normalize force vector to get direction only and trim magnitude
            force.Normalize();
            gameObject.GetComponent<Rigidbody>().AddForce(force * KnockBackForce);

            Debug.Log("Knocked Back");
        }

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
    }
}