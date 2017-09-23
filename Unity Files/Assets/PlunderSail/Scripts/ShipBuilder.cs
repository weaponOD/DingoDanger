using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum AttachmentType { CABIN, SINGLEWEAPON, DOUBLEWEAPON, SAIL, ARMOUR}

public class ShipBuilder : MonoBehaviour
{
    private AttachmentType currentAttachment;

    [SerializeField]
    private GameObject[] attachmentPrefabs;

    private PlayerController player;
    private UIManager UI;

    private Transform baseShip;

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
            if(UI.BuyAttachment())
            {
                if (Physics.Raycast(ray, out hitInfo))
                {
                    if (hitInfo.collider.transform.gameObject.tag == "BuildPoint")
                    {
                        string name = hitInfo.collider.transform.gameObject.name;

                        Vector3 buildPoint = hitInfo.collider.transform.position;

                        if (!name.Contains("Point"))
                        {
                            buildPoint += hitInfo.normal;
                            buildPoint.y -= 0.5f;
                        }
                        if (name == "Top")
                        {
                            buildPoint.y -= 0.5f;
                        }

                        Transform block = AddAttachment(buildPoint, hitInfo.collider.transform.rotation);

                        hitInfo.collider.transform.gameObject.GetComponent<AttachmentPoint>().PartTwo = block;
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
}