using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    // Variables
    private PlayerController player;
    
    private Rigidbody rb;

    [SerializeField]
    private float moveSpeed = 2f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }
	
	void Update ()
    {
        transform.LookAt(player.transform);
	}

    private void FixedUpdate()
    {
        if(!GameState.BuildMode)
        {
            rb.MovePosition(rb.position + transform.forward * Time.fixedDeltaTime * moveSpeed);
        }
    }
}