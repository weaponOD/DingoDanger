using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentPoint : MonoBehaviour
{
    Transform partOne = null;
    Transform partTwo = null;

    private bool active = true;

    private ShipBuilder builder;

    private void Awake()
    {
        builder = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ShipBuilder>();
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
        if (active)
        {
            if (GameState.BuildMode)
            {
                if (Physics.Raycast(transform.position, transform.forward, 0.6f))
                {
                    if (active)
                    {
                        TurnOff();
                        active = false;
                    }
                    builder.DeActivatePoint(this);
                }

                if (transform.name.Contains("Point"))
                {
                    if (Physics.Raycast(transform.position, transform.up, 0.6f))
                    {
                        if (active)
                        {
                            TurnOff();
                            active = false;
                        }
                        builder.DeActivatePoint(this);
                    }
                }
            }
        }
    }


    public void TurnOff()
    {
        active = false;
        this.gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;

        Debug.Log("Turn off");
    }

    public void TurnOn()
    {
        active = true;
        this.gameObject.GetComponentInChildren<MeshRenderer>().enabled = true;

        Debug.Log("Turn On");
    }
}