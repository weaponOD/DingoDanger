using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntity
{
    [Header("Player Attributes")]

    [SerializeField]
    private int startGold = 0;

    [SerializeField]
    private int currentGold = 0;

    private int maxGold = 99999;

    [SerializeField]
    private float ramDamage = 0;

    [Header("Collision Knockback")]

    [SerializeField]
    private float islandKnockBack = 700000;

    [SerializeField]
    [Tooltip("The number of seconds that the player does not move forward after a collision")]
    private float islandStunDuration = 0;

    [SerializeField]
    private float shipKnockBack = 0;

    [SerializeField]
    [Tooltip("The number of seconds that the player does not move forward after a collision")]
    private float stunDuration = 0;

    [SerializeField]
    private string goldGainedSound = "";

    private PlayerController controller = null;

    private CameraController CC;

    public delegate void GoldReceiced();

    public event GoldReceiced GoldChanged;

    [Header("Aim visualiser Attributes")]

    [SerializeField]
    [Tooltip("Maximum distance from the ship that the mesh can go.")]
    private float maxRange = 50;

    [Tooltip("minimum distance from the ship that the mesh can go.")]
    [SerializeField]
    private float minimumRange = 0;

    [Header("Sounds")]
    [SerializeField]
    private string takeDamageSound = "CHANGE";

    private float range = 25;

    private float previousRange;

    private bool aiming = false;

    private Transform ship;

    private string aimDir = "";

    private bool hasControl = false;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        weaponController = GetComponent<WeaponController>();
        components = GetComponent<ComponentManager>();

        ship = transform.GetChild(0).GetChild(0);

        CC = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CameraController>();

        // Subscribe to game state
        GameState.buildModeChanged += SetBuildMode;
    }

    protected override void Start()
    {
        base.Start();

        currentGold = startGold;

        range = minimumRange;
    }

    private void Update()
    {

        // Check Input for aiming
        if (!GameState.BuildMode && !GameState.Paused && hasControl)
        {

            // if we're not aiming fire both sides
            if (!aiming)
            {
                if (Input.GetAxis("Right_Trigger") == 1)
                {
                    // check if there's cannons on both sides otherwise left side dones't have cooldown on firing
                    if (weaponController.RightWeapons.Length > 0)
                    {
                        weaponController.FireWeaponsLeft(false);
                        weaponController.FireWeaponsRight(true);
                    }
                    else
                    {
                        weaponController.FireWeaponsLeft(true);
                    }
                }
            }
            // calculate which direction we're aiming and then fire those cannons
            if (Input.GetAxis("Left_Trigger") == 1)
            {
                // first frame that the trigger is held down set aiming to true and activate the aiming mesh in the correct direction.
                if (!aiming) // && weaponController.HasWeapons
                {
                    aimDir = CC.TryAim();

                    if (aimDir.Equals("left"))
                    {
                        aiming = true;

                        WeaponAttachment[] leftWeapons = components.GetAttachedLeftWeapons();

                        foreach (WeaponAttachment weapon in leftWeapons)
                        {
                            weapon.Aim(range);
                        }
                    }

                    if (aimDir.Equals("right"))
                    {
                        aiming = true;

                        WeaponAttachment[] rightWeapons = components.GetAttachedRightWeapons();

                        foreach (WeaponAttachment weapon in rightWeapons)
                        {
                            weapon.Aim(range);
                        }
                    }
                }
            }
            else
            {
                if (aiming)
                {
                    aiming = false;

                    CC.CancelAim();

                    if (aimDir.Equals("right"))
                    {
                        WeaponAttachment[] rightWeapons = components.GetAttachedRightWeapons();

                        foreach (WeaponAttachment weapon in rightWeapons)
                        {
                            weapon.CancelAim();
                        }
                    }

                    if (aimDir.Equals("left"))
                    {
                        WeaponAttachment[] leftWeapons = components.GetAttachedLeftWeapons();

                        foreach (WeaponAttachment weapon in leftWeapons)
                        {
                            weapon.CancelAim();
                        }
                    }
                }
            }

            if (Input.GetAxis("Right_Trigger") == 1)
            {
                if (aimDir.Equals("left"))
                {
                    weaponController.FireWeaponsLeft(true);
                }

                if (aimDir.Equals("right"))
                {
                    weaponController.FireWeaponsRight(true);
                }
            }

            if (aiming)
            {
                range += Input.GetAxis("Vertical");
                range = Mathf.Clamp(range, minimumRange, maxRange);

                if (aimDir.Equals("left"))
                {
                    WeaponAttachment[] leftWeapons = components.GetAttachedLeftWeapons();

                    foreach (WeaponAttachment weapon in leftWeapons)
                    {
                        weapon.UpdateRange(range);
                    }
                }

                if (aimDir.Equals("right"))
                {
                    WeaponAttachment[] rightWeapons = components.GetAttachedRightWeapons();

                    foreach (WeaponAttachment weapon in rightWeapons)
                    {
                        weapon.UpdateRange(range);
                    }
                }
            }
        }

        velocity = controller.Velocity;
    }

    public bool HasControl
    {
        set { hasControl = value; }
    }

    public override void TakeDamage(float damgage)
    {
        base.TakeDamage(damgage);
        AudioManager.instance.PlaySound(takeDamageSound);
    }

    // Returns how much gold the player currently has
    public int Gold
    {
        get { return currentGold; }
    }

    // Remove the specifed amount of gold from the player
    public void DeductGold(int _amount)
    {
        currentGold -= _amount;

        currentGold = Mathf.Clamp(currentGold, 0, maxGold);

        if (GoldChanged != null)
        {
            GoldChanged();
        }
    }

    // Add the specified amount of gold to the player
    public void GiveGold(int _amount)
    {
        AudioManager.instance.PlaySound(goldGainedSound);

        currentGold += _amount;

        currentGold = Mathf.Clamp(currentGold, 0, maxGold);

        if (GoldChanged != null)
        {
            GoldChanged();
        }
    }

    public void FallOfWorld()
    {
        ship.gameObject.AddComponent<Rigidbody>();
        ship.gameObject.GetComponent<Rigidbody>().velocity = velocity;

        controller.Stop();
        ship.parent = null;

        TakeDamage(currentHealth + 100);
    }

    public void Respawn()
    {
        components.DestroyAll();
        Destroy(ship.GetComponent<Rigidbody>());

        ship.parent = transform.GetChild(0);

        ship.localPosition = new Vector3(0f, 0f, 3f);
        ship.localEulerAngles = Vector3.zero;
        ship.localScale = new Vector3(1f, 1f, 1f);

        currentHealth = starterHealth;
        dead = false;

        if(currentGold < startGold)
        {
            currentGold = startGold;
        }
    }

    private void SetBuildMode(bool isBuildMode)
    {
        if (!isBuildMode)
        {
            UpdateParts();
        }
    }

    // Function which checks which attachments are on the ship.
    public override void UpdateParts()
    {
        weaponController.LeftWeapons = components.GetAttachedLeftWeapons();
        weaponController.RightWeapons = components.GetAttachedRightWeapons();
        controller.setSpeedBonus(components.getSpeedBonus());

        attachments = components.Attachments;
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
            // When colliding with an enemy we want the player to knock them back unless they're moving slower.

            //  first check who's going faster
            AIAgent AI = collision.gameObject.GetComponent<AIAgent>();

            if (velocity.magnitude > AI.Velocity.magnitude)
            {
                // player going faster means we knock the AI back instead of us
                AI.AddStun();

                // calculate force vector
                var force = collision.transform.position - transform.position;

                // normalize force vector to get direction only and trim magnitude
                force.Normalize();
                AI.GetComponent<Rigidbody>().AddForce(force * shipKnockBack);
            }
            else
            {
                // Stun stops the player controller from moving forward and allows us to add forces to the player
                controller.AddStun(stunDuration);

                // calculate force vector
                var force = transform.position - collision.transform.position;

                // normalize force vector to get direction only and trim magnitude
                force.Normalize();
                gameObject.GetComponent<Rigidbody>().AddForce(force * shipKnockBack);
            }
        }

        if (collision.contacts[0].thisCollider.CompareTag("Ram"))
        {
            float hitDamage = collision.relativeVelocity.magnitude;

            // Debug.Log("Hit with Ram with a force of " + hitDamage);

            if (collision.collider.gameObject.GetComponent<AttachmentBase>())
            {
                collision.collider.gameObject.GetComponent<AttachmentBase>().TakeDamage(ramDamage);
            }

            if (collision.collider.gameObject.GetComponent<AIAgent>())
            {
                collision.collider.gameObject.GetComponent<AIAgent>().TakeDamage(ramDamage);
            }
        }

        if (collision.gameObject.layer == 8)
        {
            // Stun stops the player controller from moving forward and allows us to add forces to the player
            controller.AddStun(islandStunDuration);

            // calculate force vector
            var force = transform.position - collision.transform.position;
            // normalize force vector to get direction only and trim magnitude
            force.Normalize();
            gameObject.GetComponent<Rigidbody>().AddForce(force * islandKnockBack);
        }
    }
}