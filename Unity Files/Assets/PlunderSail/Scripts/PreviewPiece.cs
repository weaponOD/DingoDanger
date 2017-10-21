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

    public string AttachmentName
    {
        get { return attachmentName; }
    }

    public void MoveToSpot(Vector3 _spot, Vector3 _centre)
    {
        transform.position = _spot;

        if (attachmentName.Contains("Cannon"))
        {
            transform.rotation = player.rotation;

            if(_spot.z > _centre.z)
            {
                transform.Rotate(Vector3.up, 180f, Space.Self);
            }
        }
        else if (attachmentName.Contains("Sail"))
        {
            transform.rotation = player.rotation;
            transform.Rotate(Vector3.up, 180f, Space.Self);
        }
    }
}