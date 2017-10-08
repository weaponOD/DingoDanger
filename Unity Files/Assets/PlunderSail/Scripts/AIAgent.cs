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
    [Range(0, 100)]
    private float moveSpeed;

    [SerializeField]
    [Range(0, 300)]
    private float attackRange;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        //transform.localScale = Vector3.zero;

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
            MoveToAttackRange();
        }
    }

    private void MoveToAttackRange()
    {
        float distance = Vector3.Distance(player.transform.position, transform.position);

        if (distance > attackRange)
        {
            rb.MovePosition(rb.position + transform.forward * moveSpeed * Time.fixedDeltaTime);
        }
    }
}