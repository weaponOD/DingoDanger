using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidanceBehaviour : IBehaviour
{
    public override Vector3 ApplyBehaviour(Vector3 _myPos, Transform _targetPos)
    {
        //// layer mask of islands
        //int layerMask = 1 << 8;

        //Collider[] islands = Physics.OverlapSphere(_myPos, 15, layerMask);

        //foreach()
        //{

        //}

        return targetDirection;
    }
}