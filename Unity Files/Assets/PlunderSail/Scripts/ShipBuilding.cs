using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBuilding : MonoBehaviour
{
    // Editor References

    [SerializeField]
    private Vector3 GridSize;

    [SerializeField]
    private GameObject AttachmentSpotPrefab;

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

    // System References
    private UIManager UI;

    private Transform player;

    private Transform playerCentre;

    // The Transform child of player used to parent attachments
    private Transform baseShip;

    // Local Variables
    private AttachmentSpot[,,] grid;

    private Transform Buildgrid;

    private PreviewPiece preview;

    [SerializeField]
    private Vector3 previewGridPos;

    private bool canMove = true;
    private float nextTimeToMove;
    private float timeBetweenMoves = 0.1f;

    Dictionary<string, Mesh> previewMeshes;

    private int xLength;
    private int yLength;
    private int zLength;
    private Vector3 centreSpot;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        baseShip = player.Find("Components");

        previewGridPos = Vector3.zero;

        UI = GetComponent<UIManager>();

        previewMeshes = new Dictionary<string, Mesh>();

        Buildgrid = new GameObject("Grid").transform;

        InitGrid();

        // Subscribe to game state
        GameState.buildModeChanged += SetBuildMode;
    }

    private void Start()
    {
        PopulateDictionary();

        if (previewPiecePrefab)
        {
            preview = Instantiate(previewPiecePrefab, transform).GetComponent<PreviewPiece>();
            preview.gameObject.SetActive(false);
        }

        preview.setAttachment("Cabin0", previewMeshes["Cabin0"]);
    }

    private void Update()
    {
        // Move preview right
        if (Input.GetAxisRaw("Horizontal") > 0.8f)
        {
            if (canMove)
            {
                if (previewGridPos.x < xLength - 1)
                {
                    if (!grid[(int)previewGridPos.x + 1, (int)previewGridPos.y, (int)previewGridPos.z].BuiltOn)
                    {
                        canMove = false;
                        nextTimeToMove = Time.time + timeBetweenMoves;
                        previewGridPos.x++;
                        preview.MoveToSpot(grid[(int)previewGridPos.x, (int)previewGridPos.y, (int)previewGridPos.z].transform.position);
                    }
                }
            }
        }

        // Move preview Left
        if (Input.GetAxisRaw("Horizontal") < -0.8f)
        {
            if (canMove)
            {
                if (previewGridPos.x > 0)
                {
                    if (!grid[(int)previewGridPos.x - 1, (int)previewGridPos.y, (int)previewGridPos.z].BuiltOn)
                    {
                        canMove = false;
                        nextTimeToMove = Time.time + timeBetweenMoves;
                        previewGridPos.x--;
                        preview.MoveToSpot(grid[(int)previewGridPos.x, (int)previewGridPos.y, (int)previewGridPos.z].transform.position);
                    }
                }
            }
        }

        // Move preview forward
        if (Input.GetAxisRaw("Vertical") > 0.8f)
        {
            if (canMove)
            {
                if (previewGridPos.z < zLength - 1)
                {
                    canMove = false;
                    nextTimeToMove = Time.time + timeBetweenMoves;
                    previewGridPos.z++;
                    preview.MoveToSpot(grid[(int)previewGridPos.x, (int)previewGridPos.y, (int)previewGridPos.z].transform.position);
                }
            }
        }

        // Move preview back
        if (Input.GetAxisRaw("Vertical") < -0.8f)
        {
            if (canMove)
            {
                if (previewGridPos.z > 0)
                {
                    canMove = false;
                    nextTimeToMove = Time.time + timeBetweenMoves;
                    previewGridPos.z--;
                    preview.MoveToSpot(grid[(int)previewGridPos.x, (int)previewGridPos.y, (int)previewGridPos.z].transform.position);
                }
            }
        }

        if(Input.GetButtonDown("A_Button"))
        {
            placeAttachment();
        }

        if (Time.time > nextTimeToMove)
        {
            canMove = true;
        }
    }

    private void placeAttachment()
    {

    }

    private void InitGrid()
    {
        xLength = (int)GridSize.x;
        yLength = (int)GridSize.y;
        zLength = (int)GridSize.z;

        centreSpot = new Vector3(Mathf.FloorToInt(xLength / 2), 0f, Mathf.FloorToInt(zLength / 2));

        Debug.Log(centreSpot);

        grid = new AttachmentSpot[xLength, yLength, zLength];

        for (int x = 0; x < xLength; x++)
        {
            for (int y = 0; y < yLength; y++)
            {
                for (int z = 0; z < zLength; z++)
                {
                    grid[x, y, z] = Instantiate(AttachmentSpotPrefab, Vector3.zero, Quaternion.identity, Buildgrid).GetComponent<AttachmentSpot>();
                    grid[x, y, z].gameObject.SetActive(false);
                }
            }
        }
    }

    public void UpdatePreview(string _name)
    {
        Debug.Log(_name);

        preview.setAttachment(_name, previewMeshes[_name]);
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
                    grid[x, y, z].gameObject.SetActive(true);
                }
            }
        }

        Buildgrid.rotation = _target.rotation;

        Buildgrid.position -= Buildgrid.forward * Mathf.FloorToInt((GridSize.z / 2)) * 2;
        Buildgrid.position -= Buildgrid.right * Mathf.FloorToInt((GridSize.x / 2)) * 2;

        previewGridPos = centreSpot;

        preview.gameObject.SetActive(true);
        preview.MoveToSpot(grid[(int)centreSpot.x, (int)centreSpot.y, (int)centreSpot.z].transform.position);
    }

    private void SetBuildMode(bool isBuildMode)
    {
        Buildgrid.gameObject.SetActive(isBuildMode);
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