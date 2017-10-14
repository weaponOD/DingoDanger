﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttachmentDouble : WeaponAttachment
{
    protected override void Awake()
    {
        base.Awake();

        firePoints = new Transform[2];

        firePoints[0] = transform.GetChild(0).GetChild(0).transform;
        firePoints[1] = transform.GetChild(0).GetChild(1).transform;
    }

    protected override IEnumerator Fire()
    {
        Instantiate(projectilePrefab, firePoints[0].position, firePoints[0].rotation);

        Destroy(Instantiate(shootParticle.gameObject, firePoints[0].position, firePoints[0].rotation) as GameObject, shootParticle.main.startLifetime.constant);
        PlayRandomSound();

        yield return new WaitForSeconds(Random.Range(minFireTime, maxFireTime));

        Instantiate(projectilePrefab, firePoints[1].position, firePoints[1].rotation);

        Destroy(Instantiate(shootParticle.gameObject, firePoints[1].position, firePoints[1].rotation) as GameObject, shootParticle.main.startLifetime.constant);
        PlayRandomSound();

        yield return new WaitForSeconds(Random.Range(minFireTime, maxFireTime));
    }
}
