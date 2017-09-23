using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public enum AttachmentType { CABIN, WEAPON, SAIL, ARMOUR}

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

        RaycastHit hit;

        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(ray, out hit))
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);
        }

        if (Input.GetButtonDown("A_Button"))
        {
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.transform.gameObject.tag == "BuildPoint")
                {
                    Transform block = AddAttachment(hit.collider.transform);

                    hit.collider.transform.gameObject.GetComponent<AttachmentPoint>().PartTwo = block;
                }
            }
        }
    }

    private Transform AddAttachment(Transform _buildpoint)
    {
        Transform block = Instantiate(attachmentPrefabs[(int)currentAttachment], _buildpoint.position, _buildpoint.rotation, baseShip).transform;

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