using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AttachmentType { CABIN, WEAPONSINGLE, SAIL, ARMOUR }

public class ShipBuilder : MonoBehaviour
{
    [SerializeField]
    private AttachmentType currentAttachment;

    [SerializeField]
    private GameObject[] attachmentPrefabs;

    [SerializeField]
    private Material ghostMatGreen;

    private PlayerController player;
    private UIManager UI;

    private Transform baseShip;

    private Transform buildInfo;

    Vector3 hitPoint = Vector3.zero;

    private Transform lastAttachmentPoint;

    private RaycastHit hitInfo;

    GameObject previewPiece = null;

    private bool canPlace = false;

    private bool mirrorWeapon = false;
    private bool rotateWeapon = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        baseShip = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetChild(0).transform;
        UI = GameObject.Find("Canvas").GetComponent<UIManager>();

        previewPiece = null;
    }

    public AttachmentType CurrentAttachment
    {
        get { return currentAttachment; }
        set { currentAttachment = value; }
    }

    void Update()
    {
        if (Input.GetButtonDown("Y_Button"))
        {
            GameState.BuildMode = !GameState.BuildMode;

            if (!GameState.BuildMode)
            {
                if (previewPiece != null)
                {
                    GameObject.Destroy(previewPiece);
                }
            }
        }

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (hitPoint != Vector3.zero)
        {
            Debug.DrawLine(ray.origin, hitPoint, Color.red);
        }


        // if we have a ghost piece active
        if (previewPiece != null)
        {
            if (Physics.Raycast(ray, out hitInfo))
            {
                hitPoint = hitInfo.point;

                // if the raycast hit a buildPoint
                if (hitInfo.collider.transform.gameObject.tag == "BuildPoint")
                {
                    lastAttachmentPoint = hitInfo.collider.transform;

                    buildInfo = ApplyRules(lastAttachmentPoint);

                    if (buildInfo.position != Vector3.zero)
                    {
                        previewPiece.transform.position = buildInfo.position;
                        previewPiece.transform.localRotation = buildInfo.rotation;

                        if (currentAttachment == AttachmentType.WEAPONSINGLE)
                        {

                            if (rotateWeapon)
                            {
                                previewPiece.transform.Rotate(Vector3.up, -90, Space.Self);
                            }

                            previewPiece.GetComponent<AttachmentWeapon>().NeedToMirror = mirrorWeapon;
                        }
                    }
                }
            }

            // Cancel Build
            if (Input.GetButtonDown("B_Button"))
            {
                Debug.Log("Cancel Build");
                GameObject.Destroy(previewPiece);
            }

            // accept and place attachment
            if (Input.GetButtonDown("A_Button"))
            {
                if (lastAttachmentPoint != null && previewPiece.GetComponent<AttachmentBase>().CanPlace)
                {
                    AddAttachment(previewPiece.transform.position, previewPiece.transform.rotation);

                    GameObject.Destroy(previewPiece);

                    lastAttachmentPoint.gameObject.SetActive(false);

                    lastAttachmentPoint = null;
                }
            }

            // Mirror attachment
            if (Input.GetButtonDown("X_Button"))
            {
                if (currentAttachment == AttachmentType.WEAPONSINGLE)
                {
                    mirrorWeapon = !mirrorWeapon;
                }
            }
        }
    }

    private Transform AddAttachment(Vector3 _buildPoint, Quaternion _buildRotation)
    {
        Transform block = Instantiate(attachmentPrefabs[(int)currentAttachment], _buildPoint, _buildRotation, baseShip).transform;

        UI.BuyAttachment();

        return block;
    }

    private Transform ApplyRules(Transform _lastAttachmentPoint)
    {
        canPlace = false;
        rotateWeapon = false;

        string name = _lastAttachmentPoint.name;

        Vector3 buildPoint = Vector3.zero;
        Quaternion buildRot = _lastAttachmentPoint.rotation;

        // Rules for each piece
        if (currentAttachment == AttachmentType.CABIN)
        {
            if (_lastAttachmentPoint.tag == "BuildPoint")
            {
                if (!name.Contains("Point"))
                {
                    if (name == "Top")
                    {
                        if(lastAttachmentPoint.root.tag == "Weapon")
                        {
                            Debug.Log("He's got a weapon!");

                            buildPoint = _lastAttachmentPoint.position - transform.forward * 0.5f;
                            canPlace = true;
                        }
                        else
                        {
                            Debug.Log("He's got a nothing!");
                            buildPoint = _lastAttachmentPoint.position;
                            canPlace = true;
                        }
                    }
                    else
                    {
                        buildPoint = _lastAttachmentPoint.position + _lastAttachmentPoint.forward;
                        buildPoint.y -= 0.5f;
                        canPlace = true;
                    }

                    buildRot = baseShip.transform.rotation;
                }
                else
                {
                    buildPoint = _lastAttachmentPoint.position;
                    canPlace = true;
                }
            }
        }
        else if (currentAttachment == AttachmentType.WEAPONSINGLE)
        {
            if (_lastAttachmentPoint.tag == "BuildPoint")
            {
                if (!name.Contains("Point"))
                {
                    if (name == "Top")
                    {
                        canPlace = true;
                        buildPoint = _lastAttachmentPoint.position;

                        buildRot = baseShip.transform.rotation;
                        // needToRotate = true;
                        rotateWeapon = true;
                    }
                    else if (name == "Left" || name == "Right")
                    {
                        canPlace = true;
                        buildPoint = _lastAttachmentPoint.position + _lastAttachmentPoint.forward;
                        buildPoint.y -= 0.5f;
                    }
                    else if (name == "Forward" || name == "Back")
                    {
                        canPlace = true;
                        rotateWeapon = true;
                        buildPoint = _lastAttachmentPoint.position + _lastAttachmentPoint.forward;
                        buildPoint.y -= 0.5f;
                    }
                }
                else
                {
                    rotateWeapon = true;
                    canPlace = true;
                    buildPoint = _lastAttachmentPoint.position;

                    buildRot = baseShip.transform.rotation;
                }
            }
        }
        else if (currentAttachment == AttachmentType.SAIL)
        {
            if (_lastAttachmentPoint.tag == "BuildPoint")
            {
                if (!name.Contains("Point"))
                {
                    if (name == "Top")
                    {
                        canPlace = true;
                        buildPoint = _lastAttachmentPoint.position;
                        buildRot = baseShip.transform.rotation;
                    }
                    else
                    {
                        canPlace = true;
                        buildPoint = _lastAttachmentPoint.position + _lastAttachmentPoint.forward;
                        buildPoint.y -= 0.5f;
                        buildRot = baseShip.transform.rotation;
                    }
                }
                else
                {
                    canPlace = true;
                    buildPoint = _lastAttachmentPoint.position;
                    buildRot = baseShip.transform.rotation;
                }
            }
        }

        Transform build = transform;

        build.position = buildPoint;
        build.rotation = buildRot;

        return build.transform;
    }

    public void SpawnPreviewAttachment(int _attachment)
    {
        if (previewPiece != null)
        {
            GameObject.Destroy(previewPiece);
        }

        currentAttachment = (AttachmentType)_attachment;

        previewPiece = Instantiate(attachmentPrefabs[(int)currentAttachment], Vector3.zero, Quaternion.identity);

        previewPiece.GetComponent<AttachmentBase>().DisableAttachments();

        previewPiece.GetComponent<AttachmentBase>().IsPreview = true;

        MeshRenderer[] rendererList = previewPiece.GetComponentsInChildren<MeshRenderer>();

        foreach (MeshRenderer renderer in rendererList)
        {
            renderer.material = ghostMatGreen;
        }
    }

    public bool HasPreview
    {
        get { return (previewPiece != null); }
    }
}