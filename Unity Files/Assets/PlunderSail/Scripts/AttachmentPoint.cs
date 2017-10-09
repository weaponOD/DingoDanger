using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentPoint : MonoBehaviour
{
    Transform partOne = null;
    Transform partTwo = null;

    private bool active = true;

    private void Awake()
    {
        // Subscribe to game state
        GameState.buildModeChanged += TurnOn;
    }

    public Transform PartOne
    {
        get { return partOne; }
        set { partOne = value; }
    }

    public Transform PartTwo
    {
        get { return partTwo; }
        set
        {
            partTwo = value;

            if (partTwo == null)
            {
                //gameObject.SetActive(true);
            }
            else
            {
                //gameObject.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (GameState.BuildMode)
        {
            if(active)
            {
                if (Physics.Raycast(transform.position, transform.forward, 0.6f))
                {
                    if (active)
                    {
                        TurnOn(false);
                    }
                }

                if (transform.name.Contains("Point"))
                {
                    if (Physics.Raycast(transform.position, transform.up, 0.6f))
                    {
                        if (active)
                        {
                            TurnOn(false);
                        }
                    }
                }
            }
        }
    }

    public void TurnOn(bool isBuild)
    {
        active = isBuild;
        gameObject.GetComponentInChildren<MeshRenderer>().enabled = isBuild;
    }

    private void OnDestroy()
    {
        // Unsubscribe to game state
        GameState.buildModeChanged -= TurnOn;
    }
}