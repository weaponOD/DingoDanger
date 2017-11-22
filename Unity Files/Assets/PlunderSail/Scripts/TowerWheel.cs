using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerWheel : TowerBase
{
    [SerializeField]
    protected Transform wheel = null;

    protected override void Awake()
    {
        base.Awake(); firePoints = new Transform[numFirePoints];

        for (int point = 0; point < numFirePoints; point++)
        {
            firePoints[point] = wheel.GetChild(point).transform;
        }
    }

    private void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) < awarenessRange)
        {
            wheel.LookAt(player.transform.position);

            wheel.localEulerAngles = new Vector3(0f, wheel.localEulerAngles.y, 0f);

            firePoints[0].LookAt(player.transform.position);

            if (canFire)
            {
                Fire();
            }
        }
    }
}