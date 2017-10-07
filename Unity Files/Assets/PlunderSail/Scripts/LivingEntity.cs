using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(WeaponController))]
[RequireComponent(typeof(ComponentManager))]
public class LivingEntity : MonoBehaviour, IDamageable
{
    [SerializeField]
    protected float starterHealth;

    protected float currentHealth;

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
        GameObject.Destroy(gameObject);
    }
}