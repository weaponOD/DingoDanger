using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttachmentSingle : WeaponAttachment
{
    protected override void Awake()
    {
        base.Awake();

        firePoints = new Transform[1];
        effectPoints = new Transform[1];

        firePoints[0] = transform.GetChild(0).GetChild(0).transform;
        effectPoints[0] = firePoints[0].GetChild(0);
    }

    protected override IEnumerator Fire()
    {
        Projectile shot =  Instantiate(projectilePrefab, firePoints[0].position, firePoints[0].rotation).GetComponent<Projectile>();
        shot.Damage = damage;

        Destroy(Instantiate(shootParticle.gameObject, effectPoints[0].position, effectPoints[0].rotation) as GameObject, shootParticle.main.startLifetime.constant);
        PlayRandomSound();

        yield return new WaitForSeconds(Random.Range(minFireTime, maxFireTime));
    }
}
