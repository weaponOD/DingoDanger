using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBuilding : MonoBehaviour
{
    [SerializeField]
    private Vector3 GridSize;

    [SerializeField]
    private GameObject testCabin;

    // System References
    private UIManager UI;

    private Transform player;

    private Transform playerCentre;

    // The Transform child of player used to parent attachments
    private Transform baseShip;

    // Local Variables
    private BuildSpace[,,] grid;

    private Transform Buildgrid;

    private Transform previewPiece;

    Dictionary<string, Mesh> previewMeshes;

    [SerializeField]
    private GameObject previewPiecePrefab;

    [SerializeField]
    GameObject[] cabins;

    [SerializeField]
    GameObject[] Cannons;

    [SerializeField]
    GameObject[] Sails;

    [SerializeField]
    GameObject[] Armours;

    [SerializeField]
    GameObject[] Rams;

    private int xLength;
    private int yLength;
    private int zLength;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        baseShip = player.Find("Components");

        UI = GetComponent<UIManager>();

        previewMeshes = new Dictionary<string, Mesh>();

        InitGrid();

        // Subscribe to game state
        GameState.buildModeChanged += SetBuildMode;
    }

    private void Start()
    {
        Buildgrid = new GameObject("Grid").transform;

        PopulateDictionary();

        if (previewPiecePrefab)
        {
            previewPiece = Instantiate(previewPiecePrefab, transform).transform;
        }
    }

    private void InitGrid()
    {
        xLength = (int)GridSize.x;
        yLength = (int)GridSize.y;
        zLength = (int)GridSize.z;

        grid = new BuildSpace[xLength, yLength, zLength];

        for (int x = 0; x < xLength; x++)
        {
            for (int y = 0; y < yLength; y++)
            {
                for (int z = 0; z < zLength; z++)
                {
                    grid[x, y, z] = new BuildSpace();
                }
            }
        }
    }

    public void UpdatePreview(string _name)
    {
        Debug.Log(_name);
    }

    public void moveGridToPlayer(Transform _target)
    {
        Buildgrid.position = new Vector3(playerCentre.position.x, playerCentre.position.y, playerCentre.position.z);

        for (int x = 0; x < xLength; x++)
        {
            for (int y = 0; y < yLength; y++)
            {
                for (int z = 0; z < zLength; z++)
                {
                    grid[x, y, z].Pos = new Vector3(Buildgrid.position.x + (x * 2), Buildgrid.position.y + y, Buildgrid.position.z + (z * 2));
                    //Instantiate(testCabin, grid[x, y, z].Pos, Quaternion.identity, Buildgrid.transform);
                }
            }
        }

        Buildgrid.rotation = _target.rotation;

        Buildgrid.position -= Buildgrid.forward * Mathf.FloorToInt((GridSize.z / 2)) * 2;
        Buildgrid.position -= Buildgrid.right * Mathf.FloorToInt((GridSize.x / 2)) * 2;
        // Buildgrid.Rotate(Vector3.up, 90, Space.Self);
    }

    private void SetBuildMode(bool isBuildMode)
    {

    }

    public Transform PlayerCentre
    {
        set { playerCentre = value; }
    }

    private void PopulateDictionary()
    {
        int count = 0;

        foreach (GameObject cabin in cabins)
        {
            previewMeshes.Add("Cabin" + count, cabin.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh);
            count++;
        }

        count = 0;

        foreach (GameObject cannon in Cannons)
        {
            previewMeshes.Add("Cannon" + count, cannon.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh);
            count++;
        }

        count = 0;

        foreach (GameObject sail in Sails)
        {
            previewMeshes.Add("Sail" + count, sail.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh);
            count++;
        }

        count = 0;

        foreach (GameObject armour in Armours)
        {
            previewMeshes.Add("Armour" + count, armour.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh);
            count++;
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe to game state
        GameState.buildModeChanged -= SetBuildMode;
    }
}


public class BuildSpace
{
    private bool disabled;
    private Vector3 position;
    private Transform attachment;

    public BuildSpace()
    {
        position = Vector3.zero;
        attachment = null;
        disabled = false;
    }

    public Vector3 Pos
    {
        get { return position; }
        set { position = value; }
    }
}