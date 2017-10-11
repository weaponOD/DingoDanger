using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(WeaponController))]
[RequireComponent(typeof(ComponentManager))]
public class LivingEntity : MonoBehaviour, IDamageable
{
    [SerializeField]
    protected float starterHealth;

    [SerializeField]
    protected float currentHealth;

    [SerializeField]
    protected float bountyDelay;

    [SerializeField]
    protected GameObject bounty;

    protected Vector3 deathPos;

    protected WeaponController weaponController;

    protected ComponentManager components;

    protected bool dead;

    public event System.Action OnDeath;

    protected virtual void Start()
    {
        currentHealth = starterHealth;
    }

    virtual public void TakeDamage(float damgage)
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

        if(!gameObject.CompareTag("Player"))
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

    IEnumerator SpawnBounty()
    {
        yield return new WaitForSeconds(bountyDelay);

        Instantiate(bounty, deathPos, Quaternion.identity);
    }
}