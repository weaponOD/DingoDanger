using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentSpot : MonoBehaviour
{
    private bool disabled;
    private bool built = false;
    private bool anchored = false;
    private Transform attachment = null;

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

    public bool Anchored
    {
        get { return anchored; }
        set { anchored = value; }
    }

    public bool BuiltOn
    {
        get
        {
            if (attachment == null)
            {
                built = false;
            }

            return built;
        }
        set { built = value; }
    }
}