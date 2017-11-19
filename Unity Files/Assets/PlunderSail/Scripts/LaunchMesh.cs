using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class LaunchMesh : MonoBehaviour
{
    public float meshWidth = 0.1f;

    [SerializeField]
    private float velocity = 0;

    [SerializeField]
    private float angle = 45;

    public int resolution = 10;

    private Mesh mesh;

    private float g; // gravity
    private float radianAngle;

    private void Awake()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        g = Mathf.Abs(Physics.gravity.y);
    }

    private void OnValidate()
    {
        if (mesh != null)
        {
            makeArcMesh(CalculateArcArray());
        }
    }

    private void Start()
    {
        makeArcMesh(CalculateArcArray());
    }

    public float Velocity
    {
        get { return velocity; }

        set
        {
            velocity = value;
            makeArcMesh(CalculateArcArray());
        }
    }

    public float Angle
    {
        get { return angle; }

        set
        {
            angle = value;
            makeArcMesh(CalculateArcArray());
        }
    }

    private void makeArcMesh(Vector3[] _arcArray)
    {
        mesh.Clear();

        Vector3[] vertices = new Vector3[(resolution + 1) * 2];
        int[] triangles = new int[resolution * 6 * 2];

        for (int i = 0; i <= resolution; i++)
        {
            // set verices
            vertices[i * 2] = new Vector3(meshWidth * 0.5f, _arcArray[i].y, _arcArray[i].x);
            vertices[i * 2 + 1] = new Vector3(meshWidth * -0.5f, _arcArray[i].y, _arcArray[i].x);

            // set triangles
            if (i != resolution)
            {
                triangles[i * 12] = i * 2;
                triangles[i * 12 + 1] = triangles[i * 12 + 4] = i * 2 + 1;
                triangles[i * 12 + 2] = triangles[i * 12 + 3] = (i + 1) * 2;
                triangles[i * 12 + 5] = (i + 1) * 2 + 1;

                triangles[i * 12 + 6] = i * 2;
                triangles[i * 12 + 7] = triangles[i * 12 + 10] = (i + 1) * 2;
                triangles[i * 12 + 8] = triangles[i * 12 + 9] = i * 2 + 1;
                triangles[i * 12 + 11] = (i + 1) * 2 + 1;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
    }

    private Vector3[] CalculateArcArray()
    {
        Vector3[] arcArray = new Vector3[resolution + 1];


        radianAngle = Mathf.Deg2Rad * angle;
        float maxDist = (velocity * velocity * Mathf.Sin(2 * radianAngle)) / g;

        for (int i = 0; i <= resolution; i++)
        {
            float t = (float)i / (float)resolution;
            arcArray[i] = CalculateArcPoint(t, maxDist);
        }

        return arcArray;
    }

    private Vector3 CalculateArcPoint(float t, float maxDist)
    {
        float x = t * maxDist;
        float y = x * Mathf.Tan(radianAngle) - ((g * x * x) / (2 * velocity * velocity * Mathf.Cos(radianAngle) * Mathf.Cos(radianAngle)));

        return new Vector3(x, y);
    }
}