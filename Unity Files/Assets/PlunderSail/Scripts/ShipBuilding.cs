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

    [SerializeField]
    private bool canPlace = true;

    [SerializeField]
    private bool dirty = true;

    private int previewGridPosX = 0;
    private int previewGridPosY = 0;
    private int previewGridPosZ = 0;

    private bool canMove = true;
    private float nextTimeToMove;
    private float timeBetweenMoves = 0.1f;

    Dictionary<string, Attachment> attachments;

    [SerializeField]
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
        currentPiece = preview.AttachmentName;
    }

    private void Update()
    {
        if(GameState.BuildMode)
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

            // Move preview down
            if (Input.GetAxisRaw("Left_Trigger") > 0.8f)
            {
                CalculatePerspectiveMovement("down");
            }

            // Move preview up
            if (Input.GetAxisRaw("Right_Trigger") > 0.8f)
            {
                CalculatePerspectiveMovement("up");
            }

            if (Input.GetButtonDown("A_Button"))
            {
                placeAttachment();
            }

            if (dirty)
            {
                ApplyPlacementRules();
                dirty = false;
            }

            if (Time.time > nextTimeToMove)
            {
                canMove = true;
            }
        }
    }

    // Converts absolute movement to movement relative to the camera's angle
    private void CalculatePerspectiveMovement(string _dir)
    {
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
        else if (_dir.Equals("up"))
        {
            MoveUp();
        }
        else if (_dir.Equals("down"))
        {
            MoveDown();
        }
    }

    // Move one place along the X-axis in the positive direction
    private bool MoveRight()
    {
        if (canMove)
        {
            // if not last piece in length of open slots
            if (previewGridPosX < xLength - 1)
            {
                int lengthToEnd = xLength - previewGridPosX;
                // loop until an open spot is found
                for (int x = 1; x < lengthToEnd; x++)
                {
                    if (!grid[previewGridPosX + x, previewGridPosY, previewGridPosZ].BuiltOn)
                    {
                        canMove = false;
                        nextTimeToMove = Time.time + timeBetweenMoves;
                        previewGridPosX += x;
                        preview.MoveToSpot(grid[previewGridPosX, previewGridPosY, previewGridPosZ].transform.position);

                        dirty = true;
                        return true;
                    }
                }
            }
        }

        return false;
    }

    // Move one place along the X-axis in the negetive direction
    private bool MoveLeft()
    {
        if (canMove)
        {
            // if not last piece in length of open slots
            if (previewGridPosX > 0)
            {
                int lengthToEnd = previewGridPosX;
                // loop until an open spot is found
                for (int x = 1; x <= lengthToEnd; x++)
                {
                    if (!grid[previewGridPosX - x, previewGridPosY, previewGridPosZ].BuiltOn)
                    {
                        canMove = false;
                        nextTimeToMove = Time.time + timeBetweenMoves;
                        previewGridPosX -= x;
                        preview.MoveToSpot(grid[previewGridPosX, previewGridPosY, previewGridPosZ].transform.position);

                        dirty = true;
                        return true;
                    }
                }
            }
        }

        return false;
    }

    // Move one place along the Z-axis in the positive direction
    private bool MoveForward()
    {
        if (canMove)
        {
            // if not last piece in length of open slots
            if (previewGridPosZ < zLength - 1)
            {
                int lengthToEnd = zLength - previewGridPosZ;
                // loop until an open spot is found
                for (int z = 1; z < lengthToEnd; z++)
                {
                    if (!grid[previewGridPosX, previewGridPosY, previewGridPosZ + z].BuiltOn)
                    {
                        canMove = false;
                        nextTimeToMove = Time.time + timeBetweenMoves;
                        previewGridPosZ += z;
                        preview.MoveToSpot(grid[previewGridPosX, previewGridPosY, previewGridPosZ].transform.position);

                        dirty = true;
                        return true;
                    }
                }
            }
        }

        return false;
    }

    // Move one place along the Z-axis in the negetive direction
    private bool MoveBack()
    {
        if (canMove)
        {
            // if not last piece in length of open slots
            if (previewGridPosZ > 0)
            {
                int lengthToEnd = previewGridPosZ;
                // loop until an open spot is found
                for (int z = 1; z <= lengthToEnd; z++)
                {
                    if (!grid[previewGridPosX, previewGridPosY, previewGridPosZ - z].BuiltOn)
                    {
                        canMove = false;
                        nextTimeToMove = Time.time + timeBetweenMoves;
                        previewGridPosZ -= z;
                        preview.MoveToSpot(grid[previewGridPosX, previewGridPosY, previewGridPosZ].transform.position);

                        dirty = true;
                        return true;
                    }
                }
            }
        }

        return false;
    }

    // Move one place along the Y-axis in the positive direction
    private bool MoveUp()
    {
        if (canMove)
        {
            // if not last piece in length of open slots
            if (previewGridPosY < yLength - 1)
            {
                int lengthToEnd = yLength - previewGridPosY;
                // loop until an open spot is found
                for (int y = 1; y < lengthToEnd; y++)
                {
                    if (!grid[previewGridPosX, previewGridPosY + y, previewGridPosZ].BuiltOn)
                    {
                        canMove = false;
                        nextTimeToMove = Time.time + timeBetweenMoves;
                        previewGridPosY += y;
                        preview.MoveToSpot(grid[previewGridPosX, previewGridPosY, previewGridPosZ].transform.position);

                        dirty = true;
                        return true;
                    }
                }
            }
        }

        return false;
    }

    // Move one place along the Y-axis in the negetive direction
    private bool MoveDown()
    {
        if (canMove)
        {
            // if not last piece in length of open slots
            if (previewGridPosY > 0)
            {
                int lengthToEnd = previewGridPosY;
                // loop until an open spot is found
                for (int y = 1; y <= lengthToEnd; y++)
                {
                    if (!grid[previewGridPosX, previewGridPosY - y, previewGridPosZ].BuiltOn)
                    {
                        canMove = false;
                        nextTimeToMove = Time.time + timeBetweenMoves;
                        previewGridPosY -= y;
                        preview.MoveToSpot(grid[previewGridPosX, previewGridPosY, previewGridPosZ].transform.position);

                        dirty = true;
                        return true;
                    }
                }
            }
        }

        return false;
    }

    // convert raw angle of camera to the nearest cardinonal point
    private float CalculatePerspectiveAngle()
    {
        float rawAngle = CC.BuildCam.transform.root.localEulerAngles.y;

        float convertedAngle = Mathf.Round(rawAngle / 90) * 90;

        // when rouding to nearest 90 degrees 360 and 0 should be considered the same thing.
        if (convertedAngle >= 360)
        {
            convertedAngle = 0;
        }

        return convertedAngle;
    }

    private void placeAttachment()
    {
        if (canPlace)
        {
            grid[previewGridPosX, previewGridPosY, previewGridPosZ].Attachment = Instantiate(attachments[preview.AttachmentName].GO, preview.transform.position, preview.transform.rotation, baseShip).transform;

            if (currentPiece.Contains("Sail"))
            {
                for(int y = 0; y < 8;y++)
                {
                    grid[previewGridPosX, previewGridPosY + y, previewGridPosZ].BuiltOn = true;
                }
            }
            else
            {
                grid[previewGridPosX, previewGridPosY, previewGridPosZ].BuiltOn = true;
            }

            // First try move it up the y-axis
            if (!MoveUp())
            {
                if (!MoveRight())
                {
                    if (!MoveLeft())
                    {
                        if (!MoveForward())
                        {
                            if (!MoveBack())
                            {
                                if (!MoveDown())
                                {
                                    preview.gameObject.SetActive(false);
                                }
                            }
                        }
                    }
                }
            }
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

    private void ApplyPlacementRules()
    {
        canPlace = CalculateCanPlace();
        preview.SetCanBuild(canPlace);
    }

    private bool CalculateCanPlace()
    {
        if (currentPiece.Contains("Cabin"))
        {
            preview.SetCanBuild(true);
            return true;
        }
        else if (currentPiece.Contains("Sail"))
        {
            // First check if preview has enough space up
            if (previewGridPosY < yLength - 8)
            {
                int lengthToEnd = yLength - previewGridPosY;

                // loop until an open spot is found
                for (int y = 1; y < lengthToEnd; y++)
                {
                    if (grid[previewGridPosX, previewGridPosY + y, previewGridPosZ].BuiltOn)
                    {
                        preview.SetCanBuild(false);
                        return false;
                    }
                }

                preview.SetCanBuild(true);

                return true;
            }
        }

        preview.SetCanBuild(false);
        return false;
    }

    public void UpdatePreview(string _name)
    {
        currentPiece = _name;
        preview.setAttachment(_name, attachments[_name].mesh);

        dirty = true;
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

        if (preview)
        {
            preview.gameObject.SetActive(isBuildMode);
        }
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