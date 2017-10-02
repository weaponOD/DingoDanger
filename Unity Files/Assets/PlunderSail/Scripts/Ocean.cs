using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ocean : MonoBehaviour
{
    private Transform player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
	

    private void Update()
    {
        transform.position = new Vector3(player.position.x,0f, player.position.z);
    }
}