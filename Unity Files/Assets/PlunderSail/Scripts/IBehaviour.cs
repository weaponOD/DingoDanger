using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IBehaviour : ScriptableObject
{
    protected Vector3 targetDirection;

    protected string forceName = "";

    public abstract Vector3 ApplyBehaviour(Transform _me, Transform _target);

    public virtual string Name
    {
        get { return forceName; }
    }
}