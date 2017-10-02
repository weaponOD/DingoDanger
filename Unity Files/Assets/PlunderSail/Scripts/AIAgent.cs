using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : MonoBehaviour
{
    // Variables
    private PlayerController player;

    protected float sizeIncrement = 0.01f;

    private Rigidbody rb;

    private MapManager manager;

    [SerializeField]
    private float moveSpeed = 2f;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        transform.localScale = Vector3.zero;

        manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<MapManager>();
    }
	
	void Update ()
    {
        transform.LookAt(player.transform);

        if (transform.localScale.x < 1f)
        {
            transform.localScale += new Vector3(sizeIncrement, sizeIncrement, sizeIncrement);
        }
    }

    private void FixedUpdate()
    {
        if(!GameState.BuildMode)
        {
            rb.MovePosition(rb.position + transform.forward * Time.fixedDeltaTime * moveSpeed);
        }
    }
}