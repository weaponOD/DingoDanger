using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : LivingEntity
{
    private int gold = 1000;

    private bool buildMode = false;

    [SerializeField]
    private float ramDamage = 20;

    PlayerController controller;

    buoyancy buoyant;

    protected override void Start()
    {
        base.Start();
    }

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        weaponController = GetComponent<WeaponController>();
        components = GetComponent<ComponentManager>();

        buoyant = GetComponentInChildren<buoyancy>();

        // Subscribe to game state
        GameState.buildModeChanged += SetBuildMode;
    }

    public int Gold
    {
        get { return gold; }
    }

    public void DeductGold(int _amount)
    {
        gold -= _amount;
    }

    public void GiveGold(int _amount)
    {
        gold += _amount;
    }

    private void SetBuildMode(bool isBuildMode)
    {
        buildMode = isBuildMode;

        buoyant.enabled = !buildMode;

        if (!buildMode)
        {
            Debug.Log("started");
            weaponController.LeftWeapons = components.GetAttachedLeftWeapons();
            weaponController.RightWeapons = components.GetAttachedRightWeapons();
            controller.setSpeedBonus(components.getSpeedBonus());
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe to game state
        GameState.buildModeChanged -= SetBuildMode;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.GetComponent<AttachmentBase>())
        {
            collision.collider.gameObject.GetComponent<AttachmentBase>().TakeDamage(ramDamage);
        }

        if (collision.collider.gameObject.GetComponent<AIAgent>())
        {
            collision.collider.gameObject.GetComponent<AIAgent>().TakeDamage(ramDamage);
        }
    }
}