using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachmentSail : AttachmentBase
{
    private RaycastHit hitInfo;

    [SerializeField]
    private Material greenMat;

    [SerializeField]
    private Material redMat;

    [SerializeField]
    private bool currentlyGreen = true;

    private Vector3 lastPos;

    private void Update()
    {
        if (isPreview)
        {
            if (transform.position != lastPos)
            {
                canPlace = true;
                lastPos = transform.position;
            }

            Debug.DrawLine(transform.position + transform.up * 0.9f, transform.position + transform.up * 0.9f + transform.right * 4f, Color.green);

            Ray rayRight = new Ray(transform.position + transform.up * 0.9f, transform.right);

            if (Physics.Raycast(rayRight, out hitInfo, 2f))
            {
                if (hitInfo.collider.transform.tag == "Sail")
                {
                    canPlace = false;
                }
            }

            Ray rayLeft = new Ray(transform.position + transform.up * 0.9f, -transform.right);

            if (Physics.Raycast(rayLeft, out hitInfo, 2f))
            {
                if (hitInfo.collider.transform.tag == "Sail")
                {
                    canPlace = false;
                }
            }

            Ray rayForward = new Ray(transform.position + transform.up * 0.9f, transform.forward);

            if (Physics.Raycast(rayForward, out hitInfo, 2f))
            {
                if (hitInfo.collider.transform.tag == "Sail")
                {
                    canPlace = false;
                }
            }


            Ray rayBack = new Ray(transform.position + transform.up * 0.9f, -transform.forward);

            if (Physics.Raycast(rayBack, out hitInfo, 2f))
            {
                if (hitInfo.collider.transform.tag == "Sail")
                {
                    canPlace = false;
                }
            }


            // Diagonals
            Ray rayForwardRight = new Ray(transform.position + transform.up * 0.9f, transform.forward + transform.right);

            if (Physics.Raycast(rayForwardRight, out hitInfo, 2f))
            {
                if (hitInfo.collider.transform.tag == "Sail")
                {
                    canPlace = false;
                }
            }

            Ray rayForwardleft = new Ray(transform.position + transform.up * 0.9f, transform.forward + -transform.right);

            if (Physics.Raycast(rayForwardleft, out hitInfo, 2f))
            {
                if (hitInfo.collider.transform.tag == "Sail")
                {
                    canPlace = false;
                }
            }

            Ray rayBackRight = new Ray(transform.position + transform.up * 0.9f, -transform.forward + transform.right);

            if (Physics.Raycast(rayBackRight, out hitInfo, 2f))
            {
                if (hitInfo.collider.transform.tag == "Sail")
                {
                    canPlace = false;
                }
            }


            Ray rayBackLeft = new Ray(transform.position + transform.up * 0.9f, -transform.forward + -transform.right);

            if (Physics.Raycast(rayBackLeft, out hitInfo, 2f))
            {
                if (hitInfo.collider.transform.tag == "Sail")
                {
                    canPlace = false;
                }
            }

            if (canPlace)
            {
                if (!currentlyGreen)
                {
                    MeshRenderer[] rendererList = GetComponentsInChildren<MeshRenderer>();

                    foreach (MeshRenderer renderer in rendererList)
                    {
                        renderer.material = greenMat;
                    }

                    currentlyGreen = true;
                }
            }
            else
            {
                MeshRenderer[] rendererList = GetComponentsInChildren<MeshRenderer>();

                foreach (MeshRenderer renderer in rendererList)
                {
                    renderer.material = redMat;
                }
            }
        }
    }
}