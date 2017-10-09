using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentPoint : MonoBehaviour
{
    [SerializeField]
    private bool builtOn = false;

    private void Awake()
    {
        // Subscribe to game state
        GameState.buildModeChanged += TurnOn;
    }

    private void Update()
    {
        if (GameState.BuildMode)
        {
            if (!builtOn)
            {
                if (Physics.Raycast(transform.position, transform.forward, 0.6f))
                {
                    BuiltOn();
                }

                if (transform.name.Contains("Point"))
                {
                    if (Physics.Raycast(transform.position, transform.up, 0.6f))
                    {
                        BuiltOn();
                    }
                }
            }
        }
    }

    private void BuiltOn()
    {
        builtOn = true;
        gameObject.SetActive(false);
    }

    public void TurnOn(bool isBuild)
    {
        if (!builtOn)
        {
            gameObject.GetComponentInChildren<MeshRenderer>().enabled = isBuild;
            gameObject.GetComponent<BoxCollider>().enabled = isBuild;
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe to game state
        GameState.buildModeChanged -= TurnOn;
    }
}