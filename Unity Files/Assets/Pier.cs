using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pier : MonoBehaviour
{
    [SerializeField]
    private Transform dockingPos;

    private UIManager UI;
    private GameManager gm;

    private void Awake()
    {
        UI = GameObject.FindGameObjectWithTag("GameManager").GetComponent<UIManager>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Has entered");
            UI.ShowPierPopUp(true);
            gm.setPier(dockingPos);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player Has left");
            UI.ShowPierPopUp(false);
            gm.setPier(null);
        }
    }
}