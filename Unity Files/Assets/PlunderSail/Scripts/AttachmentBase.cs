using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentBase : MonoBehaviour
{
    protected float currentHealth;
    protected float maxHealth;

    [SerializeField]
    protected bool canPlace = true;

    protected bool isPreview = false;

    public void DisableAttachments()
    {
        AttachmentPoint[] points = GetComponentsInChildren<AttachmentPoint>();

        foreach(AttachmentPoint point in points)
        {
            point.GetComponent<BoxCollider>().enabled = false;
        }

        GetComponent<BoxCollider>().enabled = false;
    }

    public void Mirror()
    {
        transform.Rotate(Vector3.up, 180, Space.Self);
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
}