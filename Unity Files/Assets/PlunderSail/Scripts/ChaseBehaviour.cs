using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Behaviour steers the AI towards the a point to the lef or right of the player, whichever is closest. This allows the AI to be along side the player rather than behind.
/// </summary>
/// 
public class ChaseBehaviour : IBehaviour
{
    private Vector3 leftSidePoint;
    private Vector3 RightSidePoint;

    public override Vector3 ApplyBehaviour(Transform _me, Transform _target)
    {
        // work out which side of the player is closer to the AI
        leftSidePoint = _target.position - _target.right * 20;
        RightSidePoint = _target.position + _target.right * 20;

        if (Vector3.Distance(_me.position, leftSidePoint) < Vector3.Distance(_me.position, RightSidePoint))
        {
            targetDirection = (leftSidePoint - _me.position).normalized;
        }
        else
        {
            targetDirection = (RightSidePoint - _me.position).normalized;
        }
        
        return targetDirection;
    }
}