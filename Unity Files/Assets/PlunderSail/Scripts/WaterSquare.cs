using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Generates a plane with a specific resolution and transforms the plane to make waves
public class WaterSquare
{
    public Transform squareTransform;

    //Add the wave mesh to the MeshFilter
    public MeshFilter terrainMeshFilter;

    //The total size in m
    private float size;
    //Resolution = Width of one square
    public float spacing;
    //The total number of vertices we need to generate based on size and spacing
    private int width;

    //For the thread to update the water
    //The local center position of this square to fake transformpoint in a thread
    public Vector3 centerPos;
    //The latest vertices that belong to this square
    public Vector3[] vertices;

    public WaterSquare(GameObject waterSquareObj, float size, float spacing)
    {
        this.squareTransform = waterSquareObj.transform;

        this.size = size;
        this.spacing = spacing;

        this.terrainMeshFilter = squareTransform.GetComponent<MeshFilter>();

        //Calculate the data we need to generate the water mesh   
        width = (int)(size / spacing);
        //Because each square is 2 vertices, so we need one more
        width += 1;

        //Center the sea
        float offset = -((width - 1) * spacing) / 2;

        Vector3 newPos = new Vector3(offset, squareTransform.position.y, offset);

        squareTransform.position += newPos;

        //Save the center position of the square
        this.centerPos = waterSquareObj.transform.localPosition;

        //Save the vertices so we can update them in a thread
        this.vertices = terrainMeshFilter.mesh.vertices;
    }
}