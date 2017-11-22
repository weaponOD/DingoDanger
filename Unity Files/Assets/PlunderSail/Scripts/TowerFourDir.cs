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
    }
}