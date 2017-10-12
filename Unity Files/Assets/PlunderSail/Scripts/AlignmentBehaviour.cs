using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This Behaviour steers the AI to the same rotation as the target. This is useful during combat for the AI to be parallel to the target.
/// </summary>
///
public class AlignmentBehaviour : IBehaviour
{
    public override Vector3 ApplyBehaviour(Vector3 _myPos, Transform _target)
    {
        targetDirection = _target.rotation.eulerAngles;

        return targetDirection;
    }
}
