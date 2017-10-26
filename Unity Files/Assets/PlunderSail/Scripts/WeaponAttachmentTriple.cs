using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAttachmentTriple : WeaponAttachment
{
    protected override void Awake()
    {
        base.Awake();

        firePoints = new Transform[3];

        firePoints[0] = transform.GetChild(0).GetChild(0).transform;
        firePoints[1] = transform.GetChild(0).GetChild(1).transform;
        firePoints[2] = transform.GetChild(0).GetChild(2).transform;
    }

    protected override IEnumerator Fire(Vector3 _shipVelocity)
    {
        Projectile shot = Instantiate(projectilePrefab, firePoints[0].position, firePoints[0].rotation).GetComponent<Projectile>();
        shot.Damage = damage;

        Destroy(Instantiate(shootParticle.gameObject, firePoints[0].position, firePoints[0].rotation) as GameObject, shootParticle.main.startLifetime.constant);
        PlayRandomSound();

        yield return new WaitForSeconds(Random.Range(minFireTime, maxFireTime));

        Projectile shot2 = Instantiate(projectilePrefab, firePoints[1].position, firePoints[1].rotation).GetComponent<Projectile>();
        shot2.Damage = damage;


        Destroy(Instantiate(shootParticle.gameObject, firePoints[1].position, firePoints[1].rotation) as GameObject, shootParticle.main.startLifetime.constant);
        PlayRandomSound();

        yield return new WaitForSeconds(Random.Range(minFireTime, maxFireTime));

        Projectile shot3 =  Instantiate(projectilePrefab, firePoints[2].position, firePoints[2].rotation).GetComponent<Projectile>();
        shot3.Damage = damage;

        Destroy(Instantiate(shootParticle.gameObject, firePoints[2].position, firePoints[1].rotation) as GameObject, shootParticle.main.startLifetime.constant);
        PlayRandomSound();
    }
}
