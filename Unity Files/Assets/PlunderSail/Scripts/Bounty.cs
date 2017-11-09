using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bounty : MonoBehaviour
{
    [SerializeField]
    private int goldAmount = 10;

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.gameObject.CompareTag("Player"))
        {
            collision.collider.gameObject.GetComponent<Player>().GiveGold(goldAmount);
            GameObject.Destroy(gameObject);
        }
        
    }
}
