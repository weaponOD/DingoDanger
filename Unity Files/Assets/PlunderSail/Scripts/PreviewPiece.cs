using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewPiece : MonoBehaviour
{
    [SerializeField]
    private Material previewMat;


    private MeshFilter meshFilter;
    private Material[] cachedMaterials;
    private MeshRenderer cachedRenderer;
    private Material[] previewMaterials;

    private Transform player;

    private void Awake()
    {
        meshFilter = GetComponentInChildren<MeshFilter>();
        cachedRenderer = GetComponentInChildren<MeshRenderer>();

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    public void setAttachment(string _type, Mesh _mesh)
    {
        meshFilter.mesh = _mesh;

        cachedMaterials = cachedRenderer.materials;
        previewMaterials = new Material[cachedMaterials.Length];

        for(int x = 0; x < previewMaterials.Length; x++)
        {
            previewMaterials[x] = previewMat;
        }

        cachedRenderer.materials = previewMaterials;

        if(_type.Contains("Cannon"))
        {
            transform.localEulerAngles = new Vector3(0f, -90,0f);
        }
        else
        {
            transform.rotation = player.rotation;
        }
    }

    public void MoveToSpot(Vector3 _spot)
    {
        transform.position = _spot;
    }
}