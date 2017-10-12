using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentBase : MonoBehaviour
{
    [SerializeField]
    protected float currentHealth;

    protected float maxHealth;

    [SerializeField]
    protected bool canPlace = true;

    protected bool isPreview = false;

    private void Awake()
    {
        maxHealth = 100;
        currentHealth = maxHealth;
    }

    public void DisableAttachments()
    {
        AttachmentPoint[] points = GetComponentsInChildren<AttachmentPoint>();

        foreach(AttachmentPoint point in points)
        {
            point.GetComponent<BoxCollider>().enabled = false;
            point.GetComponent<AttachmentPoint>().enabled = false;
        }

        GetComponent<BoxCollider>().enabled = false;
    }

    public void Mirror()
    {
        transform.Rotate(Vector3.up, 180, Space.Self);
    }

    public virtual void TakeDamage(float _damage)
    {
        currentHealth -= _damage;

        Debug.Log("Took Damage");

        if(currentHealth <= 0)
        {
            GameObject.Destroy(gameObject);
        }
    }

    public bool CanPlace
    {
        get { return canPlace; }
        set { canPlace = value; }
    }
    public bool IsPreview
    {
        set { isPreview = value; }
    }

    private void OnDestroy()
    {
        if(GetComponentInParent<Player>())
        {
            GetComponentInParent<Player>().UpdateAttachments();
        }
    }
}