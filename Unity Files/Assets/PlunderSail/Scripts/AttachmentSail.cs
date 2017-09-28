using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentSail : AttachmentBase
{
    private RaycastHit hitInfo;
    private Material material;
    private Color defaulColor;


    private void Awake()
    {
        material = GetComponentInChildren<MeshRenderer>().material;
        defaulColor = material.color;
    }


    private void Update()
    {
        canPlace = true;
        //Debug.DrawLine(transform.position, transform.position + transform.right * 4f, Color.red);

        if (Physics.Raycast(transform.position, transform.right, out hitInfo))
        {
            if (hitInfo.collider.transform.tag == "Sail")
            {
                canPlace = false;
            }
        }

        if (Physics.Raycast(transform.position, -transform.right, out hitInfo))
        {
            if (hitInfo.collider.transform.tag == "Sail")
            {
                canPlace = false;
            }
        }


        if(GameState.BuildMode)
        {
            if (!canPlace)
            {
                material.color = Color.red;
            }
            else
            {
                material.color = defaulColor;
            }
        }
    }
}