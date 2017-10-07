using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : LivingEntity
{
    [SerializeField]
    private float baseMoveSpeed = 5;

    private int gold = 1000;

    private bool buildMode = false;

    PlayerController controller;

    protected override void Start()
    {
        base.Start();
    }

    private void Awake()
    {
        controller = GetComponent<PlayerController>();
        weaponController = GetComponent<WeaponController>();
        components = GetComponent<ComponentManager>();

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

        if(!buildMode)
        {
            weaponController.LeftWeapons = components.GetAttachedLeftWeapons();
            weaponController.RightWeapons = components.GetAttachedRightWeapons();
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe to game state
        GameState.buildModeChanged -= SetBuildMode;
    }
}