using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AttachmentType { ARMOUR, SAIL, CABIN, WEAPONSINGLE, WEAPONDOUBLE }

public class ShipBuilderOld : MonoBehaviour
{
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

    List<AttachmentPoint> deactivatedPoints;

    GameObject previewPiece = null;

    private bool canPlace = false;

    private bool mirrorWeapon = false;
    private bool rotateWeapon = false;

    [SerializeField]
    private BuildModeCam buildCam;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        baseShip = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetChild(0).transform;
        deactivatedPoints = new List<AttachmentPoint>();

        UI = GetComponent<UIManager>();

        previewPiece = null;
    }

    public AttachmentType CurrentAttachment
    {
        get { return currentAttachment; }
        set { currentAttachment = value; }
    }

    void Update()
    {
        if (!GameState.BuildMode)
        {
            if (previewPiece != null)
            {
                GameObject.Destroy(previewPiece);
                previewPiece = null;
            }
        }

        if (GameState.BuildMode)
        {
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            // if we have a preview piece active
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

                            if (currentAttachment == AttachmentType.WEAPONSINGLE || currentAttachment == AttachmentType.WEAPONDOUBLE)
                            {
                                if (rotateWeapon)
                                {
                                    previewPiece.transform.Rotate(Vector3.up, -90, Space.Self);
                                }

                                previewPiece.GetComponent<WeaponAttachment>().NeedToMirror = mirrorWeapon;
                            }

                            if (currentAttachment == AttachmentType.ARMOUR)
                            {
                                if (rotateWeapon)
                                {
                                    previewPiece.transform.Rotate(Vector3.up, -90, Space.Self);
                                }
                                else if(mirrorWeapon)
                                {
                                    previewPiece.transform.Rotate(Vector3.up, -180, Space.Self);
                                }
                            }
                        }
                    }
                }

                // accept and place attachment
                if (Input.GetButtonDown("A_Button"))
                {
                    if (lastAttachmentPoint != null && previewPiece.GetComponent<AttachmentBase>().CanPlace)
                    {
                        GameObject.Destroy(previewPiece);
                        previewPiece = null;

                        //lastAttachmentPoint.gameObject.SetActive(false);

                        lastAttachmentPoint = null;
                    }
                }

                // Mirror attachment
                if (Input.GetButtonDown("X_Button"))
                {
                    if (currentAttachment == AttachmentType.WEAPONSINGLE || currentAttachment == AttachmentType.WEAPONDOUBLE)
                    {
                        mirrorWeapon = !mirrorWeapon;
                    }
                }
            }

            // Cancel Build/ delete piece
            if (Input.GetButtonDown("B_Button"))
            {
                if (previewPiece != null)
                {
                    Debug.Log("Cancel Build");
                    GameObject.Destroy(previewPiece);
                    previewPiece = null;
                }
                else
                {
                    //if (Physics.Raycast(ray, out hitInfo))
                    //{
                    //    hitPoint = hitInfo.point;

                    //    // if the raycast hit a buildPoint
                    //    if (hitInfo.collider.transform.gameObject.tag == "BuildPoint")
                    //    {
                    //        RefreshAttachmentPoints();

                    //        GameObject.Destroy(hitInfo.collider.transform.parent.parent.gameObject);
                    //    }
                    //}
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
                        if (lastAttachmentPoint.parent.parent.tag == "Weapon")
                        {

                            if (lastAttachmentPoint.parent.parent.GetComponent<WeaponAttachment>().FacingLeft)
                            {
                                buildPoint = _lastAttachmentPoint.position - transform.right * 0.5f;
                            }
                            else
                            {
                                buildPoint = _lastAttachmentPoint.position + transform.right * 0.5f;
                            }
                            canPlace = true;
                        }
                        else
                        {
                            buildPoint = _lastAttachmentPoint.position;
                            canPlace = true;
                        }
                    }
                    else if (name == "Left" || name == "Right")
                    {

                        if (lastAttachmentPoint.parent.parent.tag == "Weapon")
                        {
                            buildPoint = _lastAttachmentPoint.position + _lastAttachmentPoint.forward;
                            buildPoint.y -= 0.5f;
                            canPlace = true;

                            if (lastAttachmentPoint.parent.parent.GetComponent<WeaponAttachment>().FacingLeft)
                            {
                                buildPoint.z -= 0.5f;
                            }
                            else
                            {
                                buildPoint.z += 0.5f;
                            }
                        }
                        else
                        {
                            buildPoint = _lastAttachmentPoint.position + _lastAttachmentPoint.forward;
                            buildPoint.y -= 0.5f;
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
        else if (currentAttachment == AttachmentType.WEAPONSINGLE || currentAttachment == AttachmentType.WEAPONDOUBLE)
        {
            if (_lastAttachmentPoint.tag == "BuildPoint")
            {
                if (!name.Contains("Point"))
                {
                    if (name == "Top")
                    {

                        if (lastAttachmentPoint.parent.parent.tag == "Weapon")
                        {
                            canPlace = true;

                            if (lastAttachmentPoint.parent.parent.GetComponent<WeaponAttachment>().FacingLeft)
                            {
                                buildPoint = _lastAttachmentPoint.position - transform.right * 0.5f;
                            }
                            else
                            {
                                buildPoint = _lastAttachmentPoint.position + transform.right * 0.5f;
                            }

                            buildRot = baseShip.transform.rotation;
                            // needToRotate = true;
                            rotateWeapon = true;
                        }
                        else
                        {
                            canPlace = true;
                            buildPoint = _lastAttachmentPoint.position;

                            buildRot = baseShip.transform.rotation;

                            // needToRotate = true;
                            rotateWeapon = true;
                        }
                    }
                    else if (name == "Left" || name == "Right")
                    {
                        if (lastAttachmentPoint.parent.parent.tag == "Weapon")
                        {
                            canPlace = true;
                            buildPoint = _lastAttachmentPoint.position + _lastAttachmentPoint.forward;
                            rotateWeapon = true;
                            buildPoint.y -= 0.5f;

                            if (lastAttachmentPoint.parent.parent.GetComponent<WeaponAttachment>().FacingLeft)
                            {
                                buildPoint.z -= 0.5f;
                            }
                            else
                            {
                                buildPoint.z += 0.5f;
                            }
                        }
                        else
                        {
                            canPlace = true;
                            buildPoint = _lastAttachmentPoint.position + _lastAttachmentPoint.forward;
                            buildPoint.y -= 0.5f;
                        }
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
                        if (lastAttachmentPoint.parent.parent.tag == "Weapon")
                        {
                            canPlace = true;

                            if (lastAttachmentPoint.parent.parent.GetComponent<WeaponAttachment>().FacingLeft)
                            {
                                buildPoint = _lastAttachmentPoint.position - transform.right * 0.5f;
                            }
                            else
                            {
                                buildPoint = _lastAttachmentPoint.position + transform.right * 0.5f;
                            }

                            buildRot = baseShip.transform.rotation;
                        }
                        else
                        {
                            canPlace = true;
                            buildPoint = _lastAttachmentPoint.position;
                            buildRot = baseShip.transform.rotation;
                        }
                    }
                    else if (name == "Left" || name == "Right")
                    {
                        if (lastAttachmentPoint.parent.parent.tag == "Weapon")
                        {
                            canPlace = true;
                            buildPoint = _lastAttachmentPoint.position + _lastAttachmentPoint.forward;
                            buildPoint.y -= 0.5f;

                            if (lastAttachmentPoint.parent.parent.GetComponent<WeaponAttachment>().FacingLeft)
                            {
                                buildPoint.z -= 0.5f;
                            }
                            else
                            {
                                buildPoint.z += 0.5f;
                            }

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
        else if (currentAttachment == AttachmentType.ARMOUR)
        {
            if (_lastAttachmentPoint.tag == "BuildPoint")
            {
                if (!name.Contains("Point"))
                {
                    if (name != "Top")
                    {
                        if (name == "Left" || name == "Right")
                        {
                            buildPoint = _lastAttachmentPoint.position;
                            buildPoint.y -= 0.5f;
                            canPlace = true;

                            buildRot = baseShip.transform.rotation;

                            if (name == "Left")
                            {
                                mirrorWeapon = true;
                            }
                        }
                        else
                        {
                            buildPoint = _lastAttachmentPoint.position;
                            buildPoint.y -= 0.5f;
                            canPlace = true;
                            rotateWeapon = true;
                            buildRot = _lastAttachmentPoint.rotation;
                        }
                    }
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
            previewPiece = null;
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

    private void RefreshAttachmentPoints()
    {
        foreach (AttachmentPoint point in deactivatedPoints)
        {
            if (point.gameObject != null)
            {
                //point.TurnOn();
            }
        }

        deactivatedPoints.Clear();
    }

    public void DeActivatePoint(AttachmentPoint _attachmentPoint)
    {
        deactivatedPoints.Add(_attachmentPoint);
    }


    public bool HasPreview
    {
        get { return (previewPiece != null); }
    }
}