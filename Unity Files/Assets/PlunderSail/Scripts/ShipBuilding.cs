using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipBuilding : MonoBehaviour
{
    // Editor References

    [SerializeField]
    private Vector3 GridSize;

    [SerializeField]
    private GameObject AttachmentSpotPrefab;

    [SerializeField]
    private GameObject previewPiecePrefab;

    private Transform hammerAnim = null;

    [SerializeField]
    private GameObject hammerPrefab = null;

    // The Transform child of player used to parent attachments
    [SerializeField]
    private Transform baseShip = null;

    [SerializeField]
    private float refundPercent = 0f;

    [SerializeField]
    private ShopItem[] cabins;

    [SerializeField]
    private ShopItem[] Cannons;

    [SerializeField]
    private ShopItem[] Sails;

    [SerializeField]
    private ShopItem[] Armours;

    // System References
    private CameraController CC = null;

    private Transform playerT = null;

    private Player player = null;

    private Transform playerCentre = null;

    private UIController UI = null;

    // Local Variables
    private AttachmentSpot[,,] grid = null;

    private Transform Buildgrid = null;

    private PreviewPiece preview = null;

    private bool canPlace = true;

    private bool dirty = true;

    private int previewGridPosX = 0;
    private int previewGridPosY = 0;
    private int previewGridPosZ = 0;

    private bool canMove = true;
    private float nextTimeToMove = 0;
    private float timeBetweenMoves = 0.1f;

    private bool sailOverRide = false;

    //private string lastDirection = null;

    [SerializeField]
    private bool skipBuilt = false;

    private Dictionary<string, Attachment> attachments;

    private Dictionary<string, int> goldCosts;

    private string currentPiece;

    private int goldCost = 0;

    private int xLength;
    private int yLength;
    private int zLength;

    private bool armourUnlocked = false;

    private Vector3 centreSpot;

    private void Awake()
    {
        playerT = GameObject.FindGameObjectWithTag("Player").transform;

        player = playerT.GetComponent<Player>();

        CC = GetComponent<CameraController>();

        attachments = new Dictionary<string, Attachment>();

        goldCosts = new Dictionary<string, int>();

        UI = GameObject.FindGameObjectWithTag("GameManager").GetComponent<UIController>();

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
            preview = Instantiate(previewPiecePrefab).GetComponent<PreviewPiece>();
            preview.gameObject.SetActive(false);
        }

        preview.setAttachment("Cabin001", attachments["Cabin001"].mesh);
        goldCost = goldCosts["Cabin001"];

        currentPiece = preview.AttachmentName;
        preview.ShipCentre = new Vector3((int)centreSpot.x, (int)centreSpot.y, (int)centreSpot.z);
    }

    private void Update()
    {
        if (GameState.BuildMode)
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
            if (Input.GetAxisRaw("Left_Bumper") > 0.8f)
            {
                CalculatePerspectiveMovement("down");
            }

            // Move preview up
            if (Input.GetAxisRaw("Right_Bumper") > 0.8f)
            {
                CalculatePerspectiveMovement("up");
            }

            if (Input.GetButtonDown("A_Button"))
            {
                PlaceAttachment();
            }

            if (Input.GetButtonDown("X_Button"))
            {
                MovePreviewToCentre();
            }

            if (Input.GetButtonDown("B_Button"))
            {
                RemoveAttachment();
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
        else if (_dir.Equals("left"))
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
        else if (_dir.Equals("forward"))
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
        else if (_dir.Equals("back"))
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
                if (skipBuilt)
                {
                    int lengthToEnd = xLength - previewGridPosX;
                    // loop until an open spot is found
                    for (int x = 1; x < lengthToEnd; x++)
                    {
                        if (!grid[previewGridPosX + x, previewGridPosY, previewGridPosZ].BuiltOn)
                        {
                            // Loop till open slot if not delete mode, loop till built spot if delete mode
                            canMove = false;
                            nextTimeToMove = Time.time + timeBetweenMoves;
                            previewGridPosX += x;
                            preview.MoveToSpot(grid[previewGridPosX, previewGridPosY, previewGridPosZ].transform.position, new Vector3(previewGridPosX, previewGridPosY, previewGridPosZ));

                            dirty = true;
                            //lastDirection = "right";
                            return true;
                        }
                    }
                }
                else
                {
                    canMove = false;
                    nextTimeToMove = Time.time + timeBetweenMoves;
                    previewGridPosX++;
                    preview.MoveToSpot(grid[previewGridPosX, previewGridPosY, previewGridPosZ].transform.position, new Vector3(previewGridPosX, previewGridPosY, previewGridPosZ));

                    dirty = true;
                    //lastDirection = "right";
                    return true;
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
                if (skipBuilt)
                {
                    int lengthToEnd = previewGridPosX;
                    // loop until an open spot is found
                    for (int x = 1; x <= lengthToEnd; x++)
                    {
                        // Loop till open slot if not delete mode, loop till built spot if delete mode
                        if (!grid[previewGridPosX - x, previewGridPosY, previewGridPosZ].BuiltOn)
                        {
                            canMove = false;
                            nextTimeToMove = Time.time + timeBetweenMoves;
                            previewGridPosX -= x;
                            preview.MoveToSpot(grid[previewGridPosX, previewGridPosY, previewGridPosZ].transform.position, new Vector3(previewGridPosX, previewGridPosY, previewGridPosZ));

                            dirty = true;
                            //lastDirection = "left";
                            return true;
                        }
                    }
                }
                else
                {
                    canMove = false;
                    nextTimeToMove = Time.time + timeBetweenMoves;
                    previewGridPosX--;
                    preview.MoveToSpot(grid[previewGridPosX, previewGridPosY, previewGridPosZ].transform.position, new Vector3(previewGridPosX, previewGridPosY, previewGridPosZ));

                    dirty = true;
                    //lastDirection = "left";

                    return true;
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
                if (skipBuilt)
                {
                    int lengthToEnd = zLength - previewGridPosZ;
                    // loop until an open spot is found
                    for (int z = 1; z < lengthToEnd; z++)
                    {
                        // Loop till open slot if not delete mode, loop till built spot if delete mode
                        if (!grid[previewGridPosX, previewGridPosY, previewGridPosZ + z].BuiltOn)
                        {
                            canMove = false;
                            nextTimeToMove = Time.time + timeBetweenMoves;
                            previewGridPosZ += z;
                            preview.MoveToSpot(grid[previewGridPosX, previewGridPosY, previewGridPosZ].transform.position, new Vector3(previewGridPosX, previewGridPosY, previewGridPosZ));

                            dirty = true;
                            //lastDirection = "forward";
                            return true;
                        }
                    }
                }
                else
                {
                    canMove = false;
                    nextTimeToMove = Time.time + timeBetweenMoves;
                    previewGridPosZ++;
                    preview.MoveToSpot(grid[previewGridPosX, previewGridPosY, previewGridPosZ].transform.position, new Vector3(previewGridPosX, previewGridPosY, previewGridPosZ));

                    dirty = true;
                    //lastDirection = "forward";
                    return true;
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
                if (skipBuilt)
                {
                    int lengthToEnd = previewGridPosZ;
                    // loop until an open spot is found
                    for (int z = 1; z <= lengthToEnd; z++)
                    {
                        // Loop till open slot if not delete mode, loop till built spot if delete mode
                        if (!grid[previewGridPosX, previewGridPosY, previewGridPosZ - z].BuiltOn)
                        {
                            canMove = false;
                            nextTimeToMove = Time.time + timeBetweenMoves;
                            previewGridPosZ -= z;
                            preview.MoveToSpot(grid[previewGridPosX, previewGridPosY, previewGridPosZ].transform.position, new Vector3(previewGridPosX, previewGridPosY, previewGridPosZ));

                            dirty = true;
                            //lastDirection = "back";
                            return true;
                        }
                    }
                }
                else
                {
                    canMove = false;
                    nextTimeToMove = Time.time + timeBetweenMoves;
                    previewGridPosZ--;
                    preview.MoveToSpot(grid[previewGridPosX, previewGridPosY, previewGridPosZ].transform.position, new Vector3(previewGridPosX, previewGridPosY, previewGridPosZ));

                    dirty = true;
                    //lastDirection = "back";
                    return true;
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
                if (skipBuilt)
                {
                    int lengthToEnd = yLength - previewGridPosY;
                    // loop until an open spot is found
                    for (int y = 1; y < lengthToEnd; y++)
                    {
                        // Loop till open slot if not delete mode, loop till built spot if delete mode
                        if (!grid[previewGridPosX, previewGridPosY + y, previewGridPosZ].BuiltOn)
                        {
                            canMove = false;
                            nextTimeToMove = Time.time + timeBetweenMoves;
                            previewGridPosY += y;
                            preview.MoveToSpot(grid[previewGridPosX, previewGridPosY, previewGridPosZ].transform.position, new Vector3(previewGridPosX, previewGridPosY, previewGridPosZ));

                            dirty = true;
                            //lastDirection = "up";
                            return true;
                        }
                    }
                }
                else
                {
                    canMove = false;
                    nextTimeToMove = Time.time + timeBetweenMoves;
                    previewGridPosY++;
                    preview.MoveToSpot(grid[previewGridPosX, previewGridPosY, previewGridPosZ].transform.position, new Vector3(previewGridPosX, previewGridPosY, previewGridPosZ));

                    dirty = true;
                    //lastDirection = "up";
                    return true;
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
                if (skipBuilt)
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
                            preview.MoveToSpot(grid[previewGridPosX, previewGridPosY, previewGridPosZ].transform.position, new Vector3(previewGridPosX, previewGridPosY, previewGridPosZ));

                            dirty = true;
                            //lastDirection = "down";
                            return true;
                        }
                    }
                }
                else
                {
                    canMove = false;
                    nextTimeToMove = Time.time + timeBetweenMoves;
                    previewGridPosY--;
                    preview.MoveToSpot(grid[previewGridPosX, previewGridPosY, previewGridPosZ].transform.position, new Vector3(previewGridPosX, previewGridPosY, previewGridPosZ));

                    dirty = true;
                    //lastDirection = "down";
                    return true;
                }
            }
        }

        return false;
    }

    // convert raw angle of camera to the nearest cardinonal point
    private float CalculatePerspectiveAngle()
    {
        Quaternion relative = Quaternion.Inverse(playerT.transform.rotation) * CC.BuildCam.transform.root.rotation;

        float rawAngle = relative.eulerAngles.y;

        //float rawAngle = CC.BuildCam.transform.root.localEulerAngles.y;

        float convertedAngle = Mathf.Round(rawAngle / 90) * 90;

        // when rouding to nearest 90 degrees 360 and 0 should be considered the same thing.
        if (convertedAngle >= 360)
        {
            convertedAngle = 0;
        }

        return convertedAngle;
    }

    private void PlaceAttachment()
    {
        if (canPlace)
        {
            if (sailOverRide)
            {
                Destroy(grid[previewGridPosX, previewGridPosY, previewGridPosZ].Attachment.gameObject);
                sailOverRide = false;
            }

            Transform newAttachment = Instantiate(attachments[preview.AttachmentName].GO, preview.transform.position, preview.transform.rotation, baseShip).transform;
            grid[previewGridPosX, previewGridPosY, previewGridPosZ].Attachment = newAttachment;

            if (hammerAnim == null)
            {
                hammerAnim = Instantiate(hammerPrefab).transform;
            }

            if (!hammerAnim.gameObject.activeInHierarchy)
            {
                hammerAnim.position = new Vector3(newAttachment.position.x + 1, newAttachment.position.y + 1, newAttachment.position.z - 1);
                hammerAnim.gameObject.SetActive(true);

                Invoke("HideHammerAnim", 0.7f);
            }

            player.DeductGold(goldCosts[preview.AttachmentName]);

            player.UpdateParts();

            UI.UpdateSpeedSlider();

            dirty = true;

            AudioManager.instance.PlaySound("hammer");

            if (currentPiece.Contains("Sail"))
            {
                for (int y = 0; y < 8; y++)
                {
                    if(previewGridPosY + y < GridSize.y)
                    {
                        grid[previewGridPosX, previewGridPosY + y, previewGridPosZ].Attachment = newAttachment;
                        grid[previewGridPosX, previewGridPosY + y, previewGridPosZ].BuiltOn = true;

                        if (y > 0)
                        {
                            if (previewGridPosZ < GridSize.z - 1)
                            {
                                grid[previewGridPosX, previewGridPosY + y, previewGridPosZ + 1].Attachment = newAttachment;
                                grid[previewGridPosX, previewGridPosY + y, previewGridPosZ + 1].Disabled = true;
                            }
                        }
                    }
                }
            }
            else
            {
                grid[previewGridPosX, previewGridPosY, previewGridPosZ].BuiltOn = true;
            }
        }
    }

    private void HideHammerAnim()
    {
        hammerAnim.gameObject.SetActive(false);
    }

    public void UnlockTrident()
    {
        UI.UnlockTrident();
    }

    public void UnlockDropBear()
    {
        UI.UnlockDropBear();
    }

    public void UnlockArmour()
    {
        UI.UnlockArmour();
        armourUnlocked = true;
    }

    private void RemoveAttachment()
    {
        if (grid[previewGridPosX, previewGridPosY, previewGridPosZ].Attachment != null)
        {
            string wholeName = grid[previewGridPosX, previewGridPosY, previewGridPosZ].Attachment.gameObject.name;

            string[] split = wholeName.Split('_');

            wholeName = split[0] + split[1];

            Destroy(grid[previewGridPosX, previewGridPosY, previewGridPosZ].Attachment.gameObject);

            if (wholeName.Contains("Sail"))
            {
                for (int y = 0; y < 8; y++)
                {
                    grid[previewGridPosX, previewGridPosY + y, previewGridPosZ].BuiltOn = false;

                    if (y > 0)
                    {
                        grid[previewGridPosX, previewGridPosY + y, previewGridPosZ + 1].Disabled = false;
                    }
                }
            }
            else
            {
                grid[previewGridPosX, previewGridPosY, previewGridPosZ].BuiltOn = false;
            }

            UI.UpdateSpeedSlider();

            player.GiveGold(Mathf.RoundToInt(goldCosts[wholeName] * refundPercent));
        }

        dirty = true;
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
                }
            }
        }

        // Loop through 3x3 grid on ship and set Anchored to true
        for (int x = (int)centreSpot.x - 1; x < (int)centreSpot.x + 2; x++)
        {
            for (int z = (int)centreSpot.z - 1; z < (int)centreSpot.z + 2; z++)
            {
                grid[x, 0, z].Anchored = true;
            }
        }
    }

    private void ApplyPlacementRules()
    {
        canPlace = CalculateCanPlace();
        preview.SetCanBuild(canPlace);
    }

    // Applies each rule of legal placement and returns true if legal.
    private bool CalculateCanPlace()
    {
        // Can player afford piece
        if (player.Gold < goldCost)
        {
            return false;
        }

        if (!CheckAttachmentRules())
        {
            return false;
        }

        if (!grid[previewGridPosX, previewGridPosY, previewGridPosZ].IsOpen && !sailOverRide)
        {
            return false;
        }

        if (!CheckNotFloating())
        {
            return false;
        }

        return true;
    }

    // Returns true if any of the six neighbours are built on
    private bool CheckNotFloating()
    {
        // If the preview is on the ship then don't check
        if (grid[previewGridPosX, previewGridPosY, previewGridPosZ].Anchored)
        {
            return true;
        }
        else
        {
            // Check Left
            if (previewGridPosX > 0)
            {
                if (grid[previewGridPosX - 1, previewGridPosY, previewGridPosZ].BuiltOn)
                {
                    return true;
                }
            }

            // Check Right
            if (previewGridPosX < xLength - 1)
            {
                if (grid[previewGridPosX + 1, previewGridPosY, previewGridPosZ].BuiltOn)
                {
                    return true;
                }
            }

            // Check Up
            if (previewGridPosY < yLength - 1)
            {
                if (grid[previewGridPosX, previewGridPosY + 1, previewGridPosZ].BuiltOn)
                {
                    return true;
                }
            }

            // Check Down
            if (previewGridPosY > 0)
            {
                if (grid[previewGridPosX, previewGridPosY - 1, previewGridPosZ].BuiltOn)
                {
                    return true;
                }
            }

            // Check Forward
            if (previewGridPosZ < zLength - 1)
            {
                if (grid[previewGridPosX, previewGridPosY, previewGridPosZ + 1].BuiltOn)
                {
                    return true;
                }
            }

            // Check back
            if (previewGridPosZ > 0)
            {
                if (grid[previewGridPosX, previewGridPosY, previewGridPosZ - 1].BuiltOn)
                {
                    return true;
                }
            }
        }

        return false;
    }

    // Applies rules for current attachment type, returns true if the placement is legal
    private bool CheckAttachmentRules()
    {
        sailOverRide = false;

        if (currentPiece.Contains("Cabin"))
        {
            // if on left hand side of boat or centre
            if (previewGridPosX <= centreSpot.x)
            {
                if (grid[previewGridPosX + 1, previewGridPosY, previewGridPosZ].BuiltOn)
                {
                    if (grid[previewGridPosX + 1, previewGridPosY, previewGridPosZ].Attachment.gameObject.name.Contains("Cannon"))
                    {
                        if (grid[previewGridPosX + 1, previewGridPosY, previewGridPosZ].Attachment.GetComponent<WeaponAttachment>().FacingLeft)
                        {
                            preview.SetCanBuild(false);

                            return false;
                        }
                    }

                    if (grid[previewGridPosX + 1, previewGridPosY, previewGridPosZ].Attachment.gameObject.name.Contains("Armour"))
                    {
                        preview.SetCanBuild(false);

                        return false;
                    }
                }
            }

            // if on right hand side of ship
            if (previewGridPosX > centreSpot.x + 1)
            {
                if (grid[previewGridPosX - 1, previewGridPosY, previewGridPosZ].BuiltOn)
                {
                    if (grid[previewGridPosX - 1, previewGridPosY, previewGridPosZ].Attachment.gameObject.name.Contains("Cannon"))
                    {
                        preview.SetCanBuild(false);

                        return false;
                    }

                    if (grid[previewGridPosX - 1, previewGridPosY, previewGridPosZ].Attachment.gameObject.name.Contains("Armour"))
                    {
                        preview.SetCanBuild(false);

                        return false;
                    }
                }
            }

            preview.SetCanBuild(true);
            return true;
        }
        else if (currentPiece.Contains("Cannon"))
        {
            preview.SetCanBuild(true);
            return true;
        }
        else if (currentPiece.Contains("Sail"))
        {
            int lengthToEnd = yLength - previewGridPosY;

            // loop through size of sail and check if all spots are open
            for (int y = 1; y < lengthToEnd; y++)
            {
                if (!grid[previewGridPosX, previewGridPosY + y, previewGridPosZ].IsOpen)
                {
                    preview.SetCanBuild(false);
                    return false;
                }

                if (previewGridPosZ < GridSize.z - 1)
                {
                    if (!grid[previewGridPosX, previewGridPosY + y, previewGridPosZ + 1].IsOpen)
                    {
                        preview.SetCanBuild(false);
                        return false;
                    }
                }
            }

            if (grid[previewGridPosX, previewGridPosY, previewGridPosZ].BuiltOn)
            {
                if (grid[previewGridPosX, previewGridPosY, previewGridPosZ].Attachment.name.Contains("Cabin"))
                {
                    sailOverRide = true;
                }
            }

            preview.SetCanBuild(true);

            return true;
        }
        else if (currentPiece.Contains("Armour"))
        {
            if (!armourUnlocked)
            {
                return false;
            }

            // if on left hand side of boat
            if (previewGridPosX < centreSpot.x)
            {
                if (grid[previewGridPosX + 1, previewGridPosY, previewGridPosZ].BuiltOn)
                {
                    if (!grid[previewGridPosX + 1, previewGridPosY, previewGridPosZ].Attachment.gameObject.name.Contains("Cannon"))
                    {
                        preview.SetCanBuild(true);

                        return true;
                    }
                }
            }

            // if on right hand side of ship
            if (previewGridPosX > centreSpot.x)
            {
                if (grid[previewGridPosX - 1, previewGridPosY, previewGridPosZ].BuiltOn)
                {
                    if (!grid[previewGridPosX - 1, previewGridPosY, previewGridPosZ].Attachment.gameObject.name.Contains("Cannon"))
                    {
                        preview.SetCanBuild(true);

                        return true;
                    }
                }
            }
        }

        return false;
    }

    // Sets the preview 
    public void UpdatePreview(string _name)
    {
        currentPiece = _name;
        preview.setAttachment(_name, attachments[_name].mesh);
        goldCost = goldCosts[_name];

        dirty = true;
    }

    public void moveGridToPlayer(Transform _target)
    {
        // Debug.Log(_target.name);

        Buildgrid.position = new Vector3(playerCentre.position.x, playerCentre.position.y, playerCentre.position.z);

        for (int x = 0; x < xLength; x++)
        {
            for (int y = 0; y < yLength; y++)
            {
                for (int z = 0; z < zLength; z++)
                {
                    grid[x, y, z].Pos = new Vector3(Buildgrid.position.x + (x * 2), Buildgrid.position.y + y, Buildgrid.position.z + (z * 2));
                }
            }
        }

        Buildgrid.rotation = _target.rotation;

        Buildgrid.position -= Buildgrid.forward * Mathf.FloorToInt((GridSize.z / 2)) * 2;
        Buildgrid.position -= Buildgrid.right * Mathf.FloorToInt((GridSize.x / 2)) * 2;

        preview.gameObject.SetActive(true);
        MovePreviewToCentre();
        preview.ShipCentre = new Vector3((int)centreSpot.x, (int)centreSpot.y, (int)centreSpot.z);
    }

    private void MovePreviewToCentre()
    {
        preview.MoveToSpot(grid[(int)centreSpot.x, (int)centreSpot.y, (int)centreSpot.z].transform.position, new Vector3(previewGridPosX, previewGridPosY, previewGridPosZ));

        previewGridPosX = (int)centreSpot.x;
        previewGridPosY = (int)centreSpot.y;
        previewGridPosZ = (int)centreSpot.z;

        dirty = true;
    }

    private void SetBuildMode(bool isBuildMode)
    {
        Buildgrid.gameObject.SetActive(isBuildMode);

        if (preview)
        {
            preview.gameObject.SetActive(isBuildMode);
            preview.transform.rotation = grid[(int)centreSpot.x, (int)centreSpot.y, (int)centreSpot.z].transform.rotation;
        }

        if (!isBuildMode)
        {
            Buildgrid.position = Vector3.zero;
            Buildgrid.rotation = Quaternion.identity;
        }
        else
        {
            dirty = true;

            // check if weapons are unlocked


        }
    }

    public Transform PlayerCentre
    {
        set { playerCentre = value; }
    }

    private void PopulateDictionary()
    {
        int count = 1;

        foreach (ShopItem cabin in cabins)
        {
            attachments.Add("Cabin00" + count, new Attachment(cabin.GO, cabin.GO.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh));
            goldCosts.Add("Cabin00" + count, cabin.cost);
            cabin.costText.text = "" + cabin.cost;

            count++;
        }

        count = 1;

        foreach (ShopItem cannon in Cannons)
        {
            attachments.Add("Cannon00" + count, new Attachment(cannon.GO, cannon.GO.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh));
            goldCosts.Add("Cannon00" + count, cannon.cost);
            cannon.costText.text = "" + cannon.cost;

            count++;
        }

        count = 1;

        foreach (ShopItem sail in Sails)
        {
            attachments.Add("Sail00" + count, new Attachment(sail.GO, sail.GO.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh));
            goldCosts.Add("Sail00" + count, sail.cost);
            sail.costText.text = "" + sail.cost;

            count++;
        }

        count = 1;

        foreach (ShopItem armour in Armours)
        {
            attachments.Add("Armour00" + count, new Attachment(armour.GO, armour.GO.transform.GetChild(0).GetComponent<MeshFilter>().sharedMesh));
            goldCosts.Add("Armour00" + count, armour.cost);
            armour.costText.text = "" + armour.cost;

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

[System.Serializable]
public class ShopItem
{
    public string Name = "";
    public GameObject GO = null;
    public int cost = 0;
    public Text costText = null;
}