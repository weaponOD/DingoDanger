using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentBase : MonoBehaviour
{
    protected float currentHealth;
    protected float maxHealth;

    [SerializeField]
    private Vector3 eular;

    public void DisableAttachments()
    {
        AttachmentPoint[] points = GetComponentsInChildren<AttachmentPoint>();

        foreach(AttachmentPoint point in points)
        {
            point.GetComponent<BoxCollider>().enabled = false;
        }

        GetComponent<BoxCollider>().enabled = false;
    }

    public void Update()
    {
        eular = transform.localEulerAngles;
    }

    public void Mirror()
    {
        transform.Rotate(Vector3.up, 180, Space.Self);
    }
}