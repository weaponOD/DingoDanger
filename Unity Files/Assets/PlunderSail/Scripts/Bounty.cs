using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounty : MonoBehaviour
{
    [SerializeField]
    private float sizeIncrement = 0.01f;

    [SerializeField]
    private int goldAmount = 10;

    private float scale;

    private void Awake()
    {
        scale = transform.localScale.x;
        transform.localScale = Vector3.zero;
    }

    private void Update()
    {
        if (transform.localScale.x < scale)
        {
            transform.localScale += new Vector3(sizeIncrement, sizeIncrement, sizeIncrement);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.CompareTag("Player"))
        {
            collision.collider.gameObject.GetComponent<Player>().GiveGold(goldAmount);
            GameObject.Destroy(gameObject);
        }
        
    }
}
