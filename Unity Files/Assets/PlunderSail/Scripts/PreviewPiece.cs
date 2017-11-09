using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewPiece : MonoBehaviour
{
    [SerializeField]
    private Material previewMatGreen;

    [SerializeField]
    private Material previewMatRed;

    private Material currentMat;

    private Vector3 centreGridPos;

    private Vector3 gridPos;

    private MeshFilter meshFilter;
    private Material[] cachedMaterials;
    private MeshRenderer cachedRenderer;
    private Material[] previewMaterials;

    private string attachmentName;

    private Transform player;

    private void Awake()
    {
        meshFilter = GetComponentInChildren<MeshFilter>();
        cachedRenderer = GetComponentInChildren<MeshRenderer>();

        currentMat = previewMatGreen;

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void setAttachment(string _type, Mesh _mesh)
    {
        attachmentName = _type;
        meshFilter.mesh = _mesh;
        UpdateMesh();
    }

    private void UpdateMesh()
    {
        cachedMaterials = cachedRenderer.materials;
        previewMaterials = new Material[cachedMaterials.Length];

        for (int x = 0; x < previewMaterials.Length; x++)
        {
            previewMaterials[x] = currentMat;
        }

        cachedRenderer.materials = previewMaterials;

        ApplyRules();
    }

    public void SetCanBuild(bool _canBuild)
    {
        if(_canBuild)
        {
            currentMat = previewMatGreen;
        }
        else
        {
            currentMat = previewMatRed;
        }

        UpdateMesh();
    }

    private void ApplyRules()
    {
        if (attachmentName.Contains("Cannon"))
        {
            transform.rotation = player.rotation;

            if (gridPos.x > centreGridPos.x)
            {
                transform.Rotate(Vector3.up, 180f, Space.Self);
            }
            else if(gridPos.x < centreGridPos.x)
            {
                transform.Rotate(Vector3.up, 0, Space.Self);
            }
        }
        else if (attachmentName.Contains("Sail"))
        {
            transform.rotation = player.rotation;
            transform.Rotate(Vector3.up, 180f, Space.Self);
        }
    }

    public Vector3 ShipCentre
    {
        set { centreGridPos = value; }
    }

    public Vector3 GridPosition
    {
        get { return gridPos; }
        set { gridPos = value; }
    }

    public string AttachmentName
    {
        get { return attachmentName; }
    }

    public void MoveToSpot(Vector3 _spot, Vector3 _gridPos)
    {
        transform.position = _spot;

        gridPos = _gridPos;

        ApplyRules();
    }
}