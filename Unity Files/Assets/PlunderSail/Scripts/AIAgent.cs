using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    // Variables
    private Transform player;
    
    private Rigidbody rb;

    [SerializeField]
    private float moveSpeed = 2f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        transform.LookAt(player);
	}

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + transform.forward * Time.fixedDeltaTime * moveSpeed);
    }

    public void SetPlayer(Transform _player)
    {
        player = _player;
    }
}
