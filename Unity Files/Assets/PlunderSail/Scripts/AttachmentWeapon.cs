using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentWeapon : AttachmentBase
{
    [SerializeField]
    protected bool facingLeft;

    [SerializeField]
    protected bool doubleFacing = false;

    [SerializeField]
    protected GameObject projectilePrefab;
    protected Transform[] firePoints;

    private void Awake()
    {
        firePoints = new Transform[3];

        firePoints[0] = transform.GetChild(0).GetChild(0).transform;
        firePoints[1] = transform.GetChild(0).GetChild(1).transform;
        firePoints[2] = transform.GetChild(0).GetChild(2).transform;

    }


    public bool DoubleFacing
    {
        get { return doubleFacing; }
    }

    public bool FacingLeft
    {
        get
        {
            facingLeft = (transform.localRotation.y == 0);
            return facingLeft;
        }
    }

    public void FireLeft()
    {
        Debug.Log("Firing Left");

        GameObject projectile1 = (GameObject)Instantiate(projectilePrefab, firePoints[0].position, firePoints[0].rotation);
        GameObject projectile2 = (GameObject)Instantiate(projectilePrefab, firePoints[1].position, firePoints[1].rotation);
        GameObject projectile3 = (GameObject)Instantiate(projectilePrefab, firePoints[2].position, firePoints[2].rotation);
    }

    public void FireRight()
    {
        Debug.Log("Firing Right");
        GameObject projectile1 = (GameObject)Instantiate(projectilePrefab, firePoints[0].position, firePoints[0].rotation);
        GameObject projectile2 = (GameObject)Instantiate(projectilePrefab, firePoints[1].position, firePoints[1].rotation);
        GameObject projectile3 = (GameObject)Instantiate(projectilePrefab, firePoints[2].position, firePoints[2].rotation);
    }
}