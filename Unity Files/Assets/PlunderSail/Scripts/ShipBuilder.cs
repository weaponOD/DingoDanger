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
    private Material ghostMat;

    private PlayerController player;
    private UIManager UI;

    private Transform baseShip;

    private Transform buildInfo;

    Vector3 hitPoint = Vector3.zero;

    private Transform lastAttachmentPoint;

    private RaycastHit hitInfo;

    GameObject attachment = null;

    private bool canPlace = false;

    private bool mirrorWeapon = false;
    private bool rotateWeapon = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        baseShip = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetChild(0).transform;
        UI = GameObject.Find("Canvas").GetComponent<UIManager>();

        attachment = null;
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
                if (attachment != null)
                {
                    GameObject.Destroy(attachment);
                }
            }
        }

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (hitPoint != Vector3.zero)
        {
            Debug.DrawLine(ray.origin, hitPoint, Color.red);
        }

        if (attachment != null)
        {
            if (Physics.Raycast(ray, out hitInfo))
            {
                hitPoint = hitInfo.point;

                if (hitInfo.collider.transform.gameObject.tag == "BuildPoint")
                {
                    lastAttachmentPoint = hitInfo.collider.transform;

                    buildInfo = ApplyRules(hitInfo);

                    if (buildInfo.position != Vector3.zero)
                    {
                        attachment.transform.position = buildInfo.position;
                        attachment.transform.localRotation = buildInfo.rotation;

                        if(currentAttachment == AttachmentType.WEAPONSINGLE)
                        {

                            if(rotateWeapon)
                            {
                                attachment.transform.Rotate(Vector3.up, -90, Space.Self);
                            }
                            
                            attachment.GetComponent<AttachmentWeapon>().NeedToMirror = mirrorWeapon;
                        }
                    }
                }
            }

            // Cancel Build
            if (Input.GetButtonDown("B_Button"))
            {
                Debug.Log("Cancel Build");
                GameObject.Destroy(attachment);
            }

            // accept and place attachment
            if (Input.GetButtonDown("A_Button"))
            {
                Debug.Log("Attachment Placed");
                
                AddAttachment(attachment.transform.position, attachment.transform.rotation);

                GameObject.Destroy(attachment);

                // disable the build point when we build on it
                if (lastAttachmentPoint.gameObject.tag == "BuildPoint")
                {
                    lastAttachmentPoint.gameObject.SetActive(false);
                }
            }

            // Mirror attachment
            if (Input.GetButtonDown("X_Button"))
            {
                if(currentAttachment == AttachmentType.WEAPONSINGLE)
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

    private Transform ApplyRules(RaycastHit _hit)
    {
        canPlace = false;
        rotateWeapon = false;

        string name = _hit.collider.transform.gameObject.name;

        Vector3 buildPoint = Vector3.zero;
        Quaternion buildRot = _hit.collider.transform.rotation;

        // Rules for each piece
        if (currentAttachment == AttachmentType.CABIN)
        {
            if (!name.Contains("Point"))
            {
                if (name == "Top")
                {
                    buildPoint = _hit.collider.transform.position;
                    canPlace = true;
                }
                else
                {
                    buildPoint = _hit.collider.transform.position + _hit.collider.transform.forward;
                    buildPoint.y -= 0.5f;
                    canPlace = true;
                }

                buildRot = baseShip.transform.rotation;
            }
            else
            {
                buildPoint = _hit.collider.transform.position;
                canPlace = true;
            }
        }
        else if (currentAttachment == AttachmentType.WEAPONSINGLE)
        {
            if (!name.Contains("Point"))
            {
                if (name == "Top")
                {
                    canPlace = true;
                    buildPoint = _hit.collider.transform.position;

                    buildRot = baseShip.transform.rotation;
                    // needToRotate = true;
                    rotateWeapon = true;
                }
                else if (name == "Left" || name == "Right")
                {
                    canPlace = true;
                    buildPoint = _hit.collider.transform.position + _hit.collider.transform.forward;
                    buildPoint.y -= 0.5f;
                }
                else if (name == "Forward" || name == "Back")
                {
                    canPlace = true;
                    rotateWeapon = true;
                    buildPoint = _hit.collider.transform.position + _hit.collider.transform.forward;
                    buildPoint.y -= 0.5f;
                }
            }
            else
            {
                rotateWeapon = true;
                canPlace = true;
                buildPoint = _hit.collider.transform.position;

                buildRot = baseShip.transform.rotation;
            }
        }
        else if (currentAttachment == AttachmentType.SAIL)
        {
            if (!name.Contains("Point"))
            {
                if (name == "Top")
                {
                    canPlace = true;
                    buildPoint = _hit.collider.transform.position;
                    buildRot = baseShip.transform.rotation;
                }
                else
                {
                    canPlace = true;
                    buildPoint = _hit.collider.transform.position + _hit.collider.transform.forward;
                    buildPoint.y -= 0.5f;
                    buildRot = baseShip.transform.rotation;
                }
            }
            else
            {
                canPlace = true;
                buildPoint = _hit.collider.transform.position;
                buildRot = baseShip.transform.rotation;
            }
        }

        Transform build = transform;

        build.position = buildPoint;
        build.rotation = buildRot;

        return build.transform;
    }

    public void SpawnGhostAttachment(int _attachment)
    {
        currentAttachment = (AttachmentType)_attachment;

        //attachment = attachmentPrefabs[(int)currentAttachment];
        attachment = Instantiate(attachmentPrefabs[(int)currentAttachment], Vector3.zero, Quaternion.identity);

        attachment.GetComponent<AttachmentBase>().DisableAttachments();

        MeshRenderer[] rendererList = attachment.GetComponentsInChildren<MeshRenderer>();

        foreach(MeshRenderer renderer in rendererList)
        {
            renderer.material = ghostMat;
        }
    }

    public bool HasGhost
    {
        get { return (attachment != null); }
    }
}