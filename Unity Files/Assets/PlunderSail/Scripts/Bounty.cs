﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounty : MonoBehaviour
{
    [SerializeField]
    private int goldAmount = 10;

    [SerializeField]
    private float distanceToHook = 30;

    [SerializeField]
    private float moveSpeed = 0;

    [SerializeField]
    private string floatSound = "CHANGE";

    private Transform player;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.transform.root.CompareTag("Player"))
        {
            collision.collider.transform.root.GetComponent<Player>().GiveGold(goldAmount);
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        AudioManager.instance.PlaySound(floatSound);

        StartCoroutine(Hooked());
    }

    private IEnumerator Hooked()
    {
        Vector3 vecBetween = player.position - transform.position;

        if(vecBetween.magnitude < distanceToHook)
        {
            transform.position += vecBetween.normalized * Time.deltaTime * moveSpeed;
        }

        yield return null;

        StartCoroutine(Hooked());
    }
}
