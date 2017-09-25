using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum AttachmentType { CABIN, SINGLEWEAPON, DOUBLEWEAPON, SAIL, ARMOUR }

public class ShipBuilder : MonoBehaviour
{
    private AttachmentType currentAttachment;

    [SerializeField]
    private GameObject[] attachmentPrefabs;

    private PlayerController player;
    private UIManager UI;

    private Transform baseShip;

    private Transform buildInfo;

    private bool buildMode = false;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        baseShip = GameObject.FindGameObjectWithTag("Player").transform.GetChild(0).GetChild(0).transform;
        UI = GameObject.Find("Canvas").GetComponent<UIManager>();
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
            buildMode = !buildMode;

            SetBuildMode(buildMode);
            player.SetBuildMode(buildMode);
        }

        RaycastHit hitInfo;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Input.GetButtonDown("A_Button"))
        {
            // check if can afford first.
            if (UI.BuyAttachment())
            {
                if (Physics.Raycast(ray, out hitInfo))
                {
                    if (hitInfo.collider.transform.gameObject.tag == "BuildPoint")
                    {
                        buildInfo = ApplyRules(hitInfo);

                        if(buildInfo.position != Vector3.zero)
                        {
                            Transform block = AddAttachment(buildInfo.position, buildInfo.rotation);

                            hitInfo.collider.transform.gameObject.GetComponent<AttachmentPoint>().PartTwo = block;
                        }
                    }
                }
            }
        }
    }

    private Transform AddAttachment(Vector3 _buildPoint, Quaternion _buildRotation)
    {
        Transform block = Instantiate(attachmentPrefabs[(int)currentAttachment], _buildPoint, _buildRotation, baseShip).transform;

        return block;
    }

    public void SetBuildMode(bool _isBuildMode)
    {
        buildMode = _isBuildMode;

        UI.SetBuildPanelStatus(_isBuildMode);
    }

    public void AddAttachment(int _attachment)
    {
        currentAttachment = (AttachmentType)_attachment;
    }

    private Transform ApplyRules(RaycastHit _hit)
    {
        string name = _hit.collider.transform.gameObject.name;

        Vector3 buildPoint = Vector3.zero;
        Quaternion buildRot = _hit.collider.transform.rotation;

        if (currentAttachment == AttachmentType.CABIN)
        {
            if (!name.Contains("Point"))
            {
                if (name == "Top")
                {
                    buildPoint = _hit.collider.transform.position;
                }
                else
                {
                    buildPoint = _hit.collider.transform.position + _hit.collider.transform.forward;
                    buildPoint.y -= 0.5f;
                }

                buildRot = baseShip.transform.rotation;
            }
            else
            {
                buildPoint = _hit.collider.transform.position;
            }
        }
        else if (currentAttachment == AttachmentType.SINGLEWEAPON)
        {
            if (!name.Contains("Point"))
            {
                if (name == "Top")
                {
                    buildPoint = _hit.collider.transform.position;
                }
                else if (name == "Left" || name == "Right")
                {
                    buildPoint = _hit.collider.transform.position + _hit.collider.transform.forward;
                    buildPoint.y -= 0.5f;
                }
            }
            else
            {
                buildPoint = _hit.collider.transform.position;
            }
        }
        else if (currentAttachment == AttachmentType.SAIL)
        {
            buildPoint = _hit.collider.transform.position;
            buildRot = baseShip.transform.rotation;
        }

        Transform build = transform;

        build.position = buildPoint;
        build.rotation = buildRot;

        return build.transform;
    }
}