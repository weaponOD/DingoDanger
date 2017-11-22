using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerFourDir : TowerBase
{
    [SerializeField]
    private float angle;

    protected override void Awake()
    {
        base.Awake();
    }

    private void Update()
    {
        CalculateDirToPlayer();
    }

    private void CalculateDirToPlayer()
    {
        angle = Vector3.SignedAngle(player.transform.position, transform.position, Vector3.up);

        // Check if player is forward of the tower
        if(angle > -1.5 && angle < 5)
        {

        }

        // Check if player is left of the tower
        if (angle > 7 && angle < 8.5f)
        {

        }

        // Check if player is behind the tower
        if (angle > 7 && angle < 8.5f)
        {

        }

        // Check if player is right of the tower
        if (angle > 7 && angle < 8.5f)
        {

        }
    }
}