using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidanceBehaviour : IBehaviour
{
    public override Vector3 ApplyBehaviour(Vector3 _myPos, Transform _targetPos)
    {
        // targetDirection = (_targetPos - _myPos).normalized;

        return targetDirection;
    }
}