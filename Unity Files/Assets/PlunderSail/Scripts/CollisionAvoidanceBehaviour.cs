using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidanceBehaviour : IBehaviour
{
    public override Vector3 ApplyBehaviour(Transform _me, Transform _targetPos)
    {
        // layer mask of islands
        int layerMask = 1 << 8;

        forceName = "collisionAvoidance";

        Ray ray = new Ray(_me.position, _me.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 15))
        {
            targetDirection = hit.transform.position - (_me.forward * 15);
            targetDirection.Normalize();

            Debug.Log("There's something ahead of me!");
        }

        return targetDirection;
    }
}