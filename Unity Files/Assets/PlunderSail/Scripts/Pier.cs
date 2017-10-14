using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pier : MonoBehaviour
{
    [SerializeField]
    private Transform dockingPos;

    private UIManager UI;
    private GameManager GM;

    private void Awake()
    {
        UI = GameObject.FindGameObjectWithTag("GameManager").GetComponent<UIManager>();
        GM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            UI.ShowPierPopUp(true);
            GM.setPier(dockingPos);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            UI.ShowPierPopUp(false);
            GM.setPier(null);
        }
    }
}