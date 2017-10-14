using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// This Behaviour steers the AI towards the a point to the lef or right of the player, whichever is closest. This allows the AI to be along side the player rather than behind.
/// </summary>
/// 
public class WanderBehaviour : IBehaviour
{

    public override Vector3 ApplyBehaviour(Vector3 _myPos, Transform _target)
    {
        Vector2 circle = Random.insideUnitCircle;
        Vector3 RandomDir = new Vector3(circle.x, 0f, circle.y);

        targetDirection = RandomDir - _myPos;

        return targetDirection;
    }
}
