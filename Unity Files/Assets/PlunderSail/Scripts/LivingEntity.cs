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
    protected float leadReloadTime;

    [SerializeField]
    protected float tridentReloadTime;

    [SerializeField]
    protected float bountyDelay;

    [SerializeField]
    [Tooltip("The GameObjects to spawn after the entity dies")]
    protected GameObject[] bounty;

    [Header("Debug Info")]
    [SerializeField]
    protected float currentHealth;

    protected Vector3 deathPos;

    protected WeaponController weaponController;

    protected ComponentManager components;

    protected Vector3 velocity;

    protected bool dead;

    public event System.Action OnDeath;

    protected virtual void Start()
    {
        currentHealth = starterHealth;
    }

    public virtual void TakeDamage(float damgage)
    {
        Debug.Log("Took " + damgage + "Damage");

        currentHealth -= damgage;

        if (currentHealth <= 0 && !dead)
        {
            Die();
        }
    }

    [ContextMenu("Self Destruct")]
    protected void Die()
    {
        dead = true;
        if (OnDeath != null)
        {
            OnDeath();
        }

        if (!gameObject.CompareTag("Player"))
        {
            Rigidbody rb = GetComponent<Rigidbody>();

            GetComponentInChildren<buoyancy>().enabled = false;
            rb.constraints = RigidbodyConstraints.None;
            rb.mass = 2f;
            rb.useGravity = true;

            deathPos = transform.position;
            deathPos.y = -2f;
            StartCoroutine(SpawnBounty());
        }
    }

    public Vector3 Velocity
    {
        get { return velocity; }

        set { velocity = value; }
    }


    IEnumerator SpawnBounty()
    {
        yield return new WaitForSeconds(bountyDelay);

        foreach (GameObject reward in bounty)
        {
            Instantiate(reward, deathPos, Quaternion.identity);
        }
    }
}