using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : LivingEntity
{
    [SerializeField]
    private int gold = 0;

    [SerializeField]
    private float ramDamage = 0;

    [SerializeField]
    private float KnockBackForce = 0;

    private PlayerController controller = null;

    private AudioSource audioSource = null;

    private CameraController CC;

    [SerializeField]
    private AudioClip[] goldPickup = null;

    [SerializeField]
    private MeshFilter rangeMeshFilter;

    private Mesh rangeMesh;

    public delegate void GoldReceiced();

    public event GoldReceiced GoldChanged;


    [Header("Aim visualiser Attributes")]

    [SerializeField]
    [Tooltip("Maximum distance from the ship that the mesh can go.")]
    private float maxRange = 50;

    [Tooltip("minimum distance from the ship that the mesh can go.")]
    [SerializeField]
    private float minimumRange = 0;

    private float range = 25;

    private float previousRange;

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        weaponController = GetComponent<WeaponController>();
        components = GetComponent<ComponentManager>();

        CC = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CameraController>();

        audioSource = GetComponent<AudioSource>();

        // Subscribe to game state
        GameState.buildModeChanged += SetBuildMode;
    }

    protected override void Start()
    {
        base.Start();

        rangeMesh = new Mesh();
        rangeMesh.name = "Range Mesh";
        rangeMeshFilter.mesh = rangeMesh;

        range = minimumRange;

        rangeMeshFilter.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetAxis("Left_Trigger") == 1)
        {
            weaponController.FireWeaponsLeft();
        }

        if (Input.GetAxis("Right_Trigger") == 1)
        {
            weaponController.FireWeaponsRight();
        }

        if(!GameState.BuildMode)
        {
            //if (Input.GetAxis("Left_Bumper") == 1)
            //{
            //    rangeMeshFilter.transform.localEulerAngles = new Vector3(0f, 270f, 0f);
            //    rangeMeshFilter.gameObject.SetActive(true);
            //    CC.AimLeft();

            //    range += Input.GetAxis("Mouse_Y");

            //    range = Mathf.Clamp(range, minimumRange, maxRange);

            //    if (previousRange != range)
            //    {
            //        previousRange = range;

            //        Debug.Log("Previous was not the same, updating weapons now");

            //        foreach (WeaponAttachment weapon in components.GetAttachedLeftWeapons())
            //        {
            //            weapon.Target = transform.position - transform.right * range;
            //            weapon.Aim(true);
            //        }
            //    }
            //}
            //else if (Input.GetAxis("Right_Bumper") == 1)
            //{
            //    rangeMeshFilter.transform.localEulerAngles = new Vector3(0f, 90f, 0f);
            //    rangeMeshFilter.gameObject.SetActive(true);
            //    CC.AimRight();

            //    range += Input.GetAxis("Mouse_Y");
            //    range = Mathf.Clamp(range, minimumRange, maxRange);
            //}
            //else
            //{
            //    rangeMeshFilter.gameObject.SetActive(false);
            //    CC.CancelAim();
            //}
        }

        velocity = controller.Velocity;
    }

    private void LateUpdate()
    {
        Vector3 eular = rangeMeshFilter.gameObject.transform.eulerAngles;
        rangeMeshFilter.gameObject.transform.eulerAngles = new Vector3(0f, eular.y, 0f);
        DrawRangeMesh();
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

        if (GoldChanged != null)
        {
            GoldChanged();
        }

        gold += _amount;
    }

    private void DrawRangeMesh()
    {
        Vector3[] vertices = new Vector3[4];
        int[] triangles = { 0, 1, 2, 0, 2, 3 };

        vertices[0] = new Vector3(-5, 0, -1);       // left back
        vertices[1] = new Vector3(-range / 2, 0, range);    // left forward
        vertices[2] = new Vector3(range / 2, 0, range);     // right forward
        vertices[3] = new Vector3(5, 0, -1);        // right back

        rangeMesh.Clear();
        rangeMesh.vertices = vertices;
        rangeMesh.triangles = triangles;
        rangeMesh.RecalculateNormals();
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
                AI.GetComponent<Rigidbody>().AddForce(force * KnockBackForce);
            }
            else
            {
                // Stun stops the player controller from moving forward and allows us to add forces to the player
                controller.AddStun();

                // calculate force vector
                var force = transform.position - collision.transform.position;
                // normalize force vector to get direction only and trim magnitude
                force.Normalize();
                gameObject.GetComponent<Rigidbody>().AddForce(force * KnockBackForce);
            }
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