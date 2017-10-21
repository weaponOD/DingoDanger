using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentSpot : MonoBehaviour
{
    private bool disabled;
    private bool built = false;
    private Transform attachment;

    public Vector3 Pos
    {
        get { return transform.position; }
        set { transform.position = value; }
    }

    public Transform Attachment
    {
        get { return attachment; }
        set { attachment = value; }
    }

    public bool BuiltOn
    {
        get { return built; }
        set { built = value; }
    }
}