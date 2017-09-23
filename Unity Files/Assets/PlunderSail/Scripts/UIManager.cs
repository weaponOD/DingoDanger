using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    // UI References
    [SerializeField]
    private Text playerGold;

    [SerializeField]
    private Button btn_Cabin;

    [SerializeField]
    private Button btn_Weapon;

    [SerializeField]
    private Image Panel;

    [SerializeField]
    private int[] prices;

    private ShipBuilder builder;

    private PlayerController player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        builder = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ShipBuilder>();
        UpdateUI();
    }

    public void BuyAttachment(int attachment)
    {
        int cost = prices[attachment];

        if (player.Gold >= cost)
        {
            player.DeductGold(cost);
            builder.CurrentAttachment = (AttachmentType)attachment;
            UpdateUI();
        }
    }

    public void SetBuildPanelStatus(bool _isEnabled)
    {
        Panel.gameObject.SetActive(_isEnabled);
    }

    public void UpdateUI()
    {
        playerGold.text = "Gold: " + player.Gold;

        if(player.Gold >= prices[0])
        {
            btn_Cabin.interactable = true;
        }
        else
        {
            btn_Cabin.interactable = false;
        }

        if (player.Gold >= prices[1])
        {
            btn_Weapon.interactable = true;
        }
        else
        {
            btn_Weapon.interactable = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            player.GiveGold(50);
            UpdateUI();
        }

        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            player.DeductGold(50);
            UpdateUI();
        }
    }
}