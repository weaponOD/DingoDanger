using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentBase : MonoBehaviour
{
    [Header("Base Stat")]
    [SerializeField]
    protected float maxHealth;

    protected float currentHealth;

    protected LivingEntity entity;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;

        entity = transform.root.GetComponent<LivingEntity>();
    }

    public virtual void TakeDamage(float _damage)
    {
        currentHealth -= _damage;

        if (currentHealth <= 0)
        {
            transform.parent = null;

            entity.UpdateParts();

            if (!GetComponent<Rigidbody>())
            {
                gameObject.AddComponent<Rigidbody>();
            }

            Destroy(gameObject, 3f);
        }
    }
}