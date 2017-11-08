using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentBase : MonoBehaviour
{
    [Header("Base Stat")]
    [SerializeField]
    protected float maxHealth;

    protected float currentHealth;

    protected bool canPlace = true;

    protected bool isPreview = false;

    protected Player player;

    protected AIAgent AI;

    protected virtual void Awake()
    {
        currentHealth = maxHealth;

        player = transform.root.GetComponent<Player>();

        AI = transform.root.GetComponent<AIAgent>();
    }

    [ContextMenu("Self Destruct")]
    public virtual void TakeDamage(float _damage)
    {
        currentHealth -= _damage;

        if (currentHealth <= 0)
        {
            transform.parent = null;

            if (player != null)
            {
                player.UpdateAttachments();
            }

            if(AI != null)
            {
                AI.UpdateAttachments();
            }

            if (!GetComponent<Rigidbody>())
            {
                gameObject.AddComponent<Rigidbody>();
            }

            Destroy(gameObject, 3f);
        }
    }

    private void OnDestroy()
    {
        if (GetComponentInParent<Player>())
        {
            GetComponentInParent<Player>().UpdateAttachments();
        }
    }
}