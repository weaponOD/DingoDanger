using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IBehaviour : ScriptableObject
{
    protected Vector3 targetDirection;

    public abstract Vector3 ApplyBehaviour(Vector3 _myPos, Transform _target);
}