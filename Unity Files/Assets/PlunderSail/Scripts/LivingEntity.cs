using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(WeaponController))]
[RequireComponent(typeof(ComponentManager))]
public class LivingEntity : MonoBehaviour, IDamageable
{
    [Header("Attributes of living entity")]
    [SerializeField]
    protected float starterHealth;

    [SerializeField]
    protected float bountyDelay;

    [SerializeField]
    [Tooltip("The GameObjects to spawn after the entity dies")]
    protected GameObject[] bounty;

    [Header("Sounds")]

    [SerializeField]
    private string sinkingSound = "CHANGE";

    [Header("Debug Info")]
    [SerializeField]
    protected float currentHealth;

    protected Vector3 deathPos;

    protected WeaponController weaponController;

    protected ComponentManager components;

    protected Vector3 velocity;

    protected AttachmentBase[] attachments;

    protected bool dead;

    public delegate void Dead(LivingEntity _entity);
    public event Dead OnDeath;

    protected virtual void Start()
    {
        currentHealth = starterHealth;
    }

    public virtual void TakeDamage(float damgage)
    {
        // Debug.Log(gameObject.name + " took " + damgage + " damage");

        currentHealth -= damgage;

        if (currentHealth <= 0 && !dead)
        {
            Die();
        }
    }

    public virtual void UpdateParts()
    {

    }

    public bool isDead
    {
        get { return dead; }
    }

    public void RepairAttachments()
    {
        for (int i = 0; i < attachments.Length; i++)
        {
            attachments[i].RestoreHealth();
        }
    }

    [ContextMenu("Self Destruct")]
    protected virtual void Die()
    {
        dead = true;

        if (OnDeath != null)
        {
            OnDeath(this);
        }

        if (gameObject.CompareTag("Enemy"))
        {
            Crumble();
        }
        else
        {
            Debug.Log("Player dead");
        }
    }

    protected void Crumble()
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.constraints = RigidbodyConstraints.None;
        rb.mass = 2f;
        rb.useGravity = true;

        deathPos = transform.position;
        deathPos.y = -2f;

        StartCoroutine(SpawnBounty());

        AudioManager.instance.PlaySound(sinkingSound);
    }

    public Vector3 Velocity
    {
        get { return velocity; }

        set { velocity = value; }
    }


    IEnumerator SpawnBounty()
    {
        yield return new WaitForSeconds(bountyDelay);

        for(int i = 0; i < bounty.Length; i++)
        {
            if(bounty[i] != null)
            {
                Instantiate(bounty[i], deathPos + Random.insideUnitSphere * 2, Quaternion.identity);
            }
        }
    }
}