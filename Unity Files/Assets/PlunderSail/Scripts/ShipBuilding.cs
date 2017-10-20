﻿using System.Collections;
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

    private CameraController CC;

    private Transform player;

    private Transform playerCentre;

    // The Transform child of player used to parent attachments
    [SerializeField]
    private Transform baseShip;

    // Local Variables
    private AttachmentSpot[,,] grid;

    private Transform Buildgrid;

    private PreviewPiece preview;

    private int previewGridPosX = 0;
    private int previewGridPosY = 0;
    private int previewGridPosZ = 0;

    private bool canMove = true;
    private float nextTimeToMove;
    private float timeBetweenMoves = 0.1f;

    Dictionary<string, Attachment> attachments;

    private string currentPiece;

    private int xLength;
    private int yLength;
    private int zLength;
    private Vector3 centreSpot;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        CC = GetComponent<CameraController>();

        UI = GetComponent<UIManager>();

        attachments = new Dictionary<string, Attachment>();

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

        preview.setAttachment("Cabin0", attachments["Cabin0"].mesh);
    }

    private void Update()
    {
        // Move preview right
        if (Input.GetAxisRaw("Horizontal") > 0.8f)
        {
            CalculatePerspectiveMovement("right");
        }

        // Move preview Left
        if (Input.GetAxisRaw("Horizontal") < -0.8f)
        {
            CalculatePerspectiveMovement("left");
        }

        // Move preview forward
        if (Input.GetAxisRaw("Vertical") > 0.8f)
        {
            CalculatePerspectiveMovement("forward");
        }

        // Move preview back
        if (Input.GetAxisRaw("Vertical") < -0.8f)
        {
            CalculatePerspectiveMovement("back");
        }

        if (Input.GetButtonDown("A_Button"))
        {
            placeAttachment();

            Debug.Log(CalculatePerspectiveAngle());
        }

        if (Time.time > nextTimeToMove)
        {
            canMove = true;
        }
    }

    private void CalculatePerspectiveMovement(string _dir)
    {
        Debug.Log(_dir);

        if (_dir.Equals("right"))
        {
            if (CalculatePerspectiveAngle() == 0)
            {
                MoveBack();
            }
            else if (CalculatePerspectiveAngle() == 90)
            {
                MoveLeft();
            }
            else if (CalculatePerspectiveAngle() == 180)
            {
                MoveForward();
            }
            else if (CalculatePerspectiveAngle() == 270)
            {
                MoveRight();
            }
        }
        else if (_dir.Equals("left"))
        {
            if (CalculatePerspectiveAngle() == 0)
            {
                MoveForward();
            }
            else if (CalculatePerspectiveAngle() == 90)
            {
                MoveRight();
            }
            else if (CalculatePerspectiveAngle() == 180)
            {
                MoveBack();
            }
            else if (CalculatePerspectiveAngle() == 270)
            {
                MoveLeft();
            }
        }
        else if (_dir.Equals("forward"))
        {
            if (CalculatePerspectiveAngle() == 0)
            {
                MoveRight();
            }
            else if (CalculatePerspectiveAngle() == 90)
            {
                MoveBack();
            }
            else if (CalculatePerspectiveAngle() == 180)
            {
                MoveLeft();
            }
            else if (CalculatePerspectiveAngle() == 270)
            {
                MoveForward();
            }
        }
        else if (_dir.Equals("back"))
        {
            if (CalculatePerspectiveAngle() == 0)
            {
                MoveLeft();
            }
            else if (CalculatePerspectiveAngle() == 90)
            {
                MoveForward();
            }
            else if (CalculatePerspectiveAngle() == 180)
            {
                MoveRight();
            }
            else if (CalculatePerspectiveAngle() == 270)
            {
                MoveBack();
            }
        }
    }

    private void MoveRight()
    {
        if (canMove)
        {
            if (previewGridPosX < xLength - 1)
            {
                if (!grid[previewGridPosX + 1, previewGridPosY, previewGridPosZ].BuiltOn)
                {
                    canMove = false;
                    nextTimeToMove = Time.time + timeBetweenMoves;
                    previewGridPosX++;
                    preview.MoveToSpot(grid[previewGridPosX, previewGridPosY, previewGridPosZ].transform.position);
                }
            }
        }
    }

    private void MoveLeft()
    {
        if (canMove)
        {
            if (previewGridPosX > 0)
            {
                if (!grid[previewGridPosX - 1, previewGridPosY, previewGridPosZ].BuiltOn)
                {
                    canMove = false;
                    nextTimeToMove = Time.time + timeBetweenMoves;
                    previewGridPosX--;
                    preview.MoveToSpot(grid[previewGridPosX, previewGridPosY, previewGridPosZ].transform.position);
                }
            }
        }
    }

    private void MoveForward()
    {
        if (canMove)
        {
            if (previewGridPosZ < zLength - 1)
            {
                if (!grid[previewGridPosX, previewGridPosY, previewGridPosZ + 1].BuiltOn)
                {
                    canMove = false;
                    nextTimeToMove = Time.time + timeBetweenMoves;
                    previewGridPosZ++;
                    preview.MoveToSpot(grid[previewGridPosX, previewGridPosY, previewGridPosZ].transform.position);
                }
            }
        }
    }

    private void MoveBack()
    {
        if (canMove)
        {
            if (previewGridPosZ > 0)
            {
                if (!grid[previewGridPosX, previewGridPosY, previewGridPosZ - 1].BuiltOn)
                {
                    canMove = false;
                    nextTimeToMove = Time.time + timeBetweenMoves;
                    previewGridPosZ--;
                    preview.MoveToSpot(grid[previewGridPosX, previewGridPosY, previewGridPosZ].transform.position);
                }
            }
        }
    }

    // convert raw angle of camera to the nearest cardinonal point
    private float CalculatePerspectiveAngle()
    {
        float rawAngle = CC.BuildCam.transform.root.localEulerAngles.y;

        float convertedAngle = Mathf.Round(rawAngle / 90) * 90;

        if (convertedAngle >= 360)
        {
            convertedAngle = 0;
        }

        return convertedAngle;
    }

    private void placeAttachment()
    {
        Instantiate(attachments[preview.AttachmentName].GO, preview.transform.position, preview.transform.rotation, baseShip);
        grid[previewGridPosX, previewGridPosY, previewGridPosZ].BuiltOn = true;

        // move the preview away //

        // First try move it up the y-axis
        if (previewGridPosY < yLength - 1)
        {
            previewGridPosY++;
            preview.MoveToSpot(grid[previewGridPosX, previewGridPosY, previewGridPosZ].transform.position);
        }
    }

    private void InitGrid()
    {
        xLength = (int)GridSize.x;
        yLength = (int)GridSize.y;
        zLength = (int)GridSize.z;

        centreSpot = new Vector3(Mathf.FloorToInt(xLength / 2), 0f, Mathf.FloorToInt(zLength / 2));

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
        currentPiece = _name;
        preview.setAttachment(_name, attachments[_name].mesh);
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

        previewGridPosX = (int)centreSpot.x;
        previewGridPosY = (int)centreSpot.y;
        previewGridPosZ = (int)centreSpot.z;

        preview.gameObject.SetActive(true);
        preview.MoveToSpot(grid[(int)centreSpot.x, (int)centreSpot.y, (int)centreSpot.z].transform.position);
    }

    private void SetBuildMode(bool isBuildMode)
    {
        Buildgrid.gameObject.SetActive(isBuildMode);
        preview.gameObject.SetActive(isBuildMode);
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
            attachments.Add("Cabin" + count, new Attachment(cabin, cabin.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh));
            count++;
        }

        count = 0;

        foreach (GameObject cannon in Cannons)
        {
            attachments.Add("Cannon" + count, new Attachment(cannon, cannon.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh));
            count++;
        }

        count = 0;

        foreach (GameObject sail in Sails)
        {
            attachments.Add("Sail" + count, new Attachment(sail, sail.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh));
            count++;
        }

        count = 0;

        foreach (GameObject armour in Armours)
        {
            attachments.Add("Armour" + count, new Attachment(armour, armour.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh));
            count++;
        }
    }

    private void OnDestroy()
    {
        // Unsubscribe to game state
        GameState.buildModeChanged -= SetBuildMode;
    }
}

public class Attachment
{
    public GameObject GO;
    public Mesh mesh;

    public Attachment(GameObject _gameObject, Mesh _mesh)
    {
        GO = _gameObject;
        mesh = _mesh;
    }
}