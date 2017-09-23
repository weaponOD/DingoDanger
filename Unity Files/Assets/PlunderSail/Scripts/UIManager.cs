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
    private Button[] buttons;

    [SerializeField]
    private Image Panel;

    [SerializeField]
    private Image selector;

    private int selectorIndex = 0;

    [SerializeField]
    private int[] prices;

    private ShipBuilder builder;

    private PlayerController player;

    private bool DpadCanPress = false;

    [SerializeField]
    private float timeBetweenPresses = 1f;
    private float timeTillCanPress;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        builder = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ShipBuilder>();
        UpdateUI();
    }

    public bool BuyAttachment()
    {
        int cost = prices[selectorIndex];

        if (player.Gold >= cost)
        {
            player.DeductGold(cost);
            UpdateUI();

            return true;
        }

        return false;
    }

    public void SetBuildPanelStatus(bool _isEnabled)
    {
        Panel.gameObject.SetActive(_isEnabled);
    }

    public void UpdateUI()
    {
        playerGold.text = "Gold: " + player.Gold;

        for(int x = 0; x< buttons.Length;x++)
        {
            if (player.Gold >= prices[x])
            {
                buttons[x].interactable = true;
            }
            else
            {
                buttons[x].interactable = false;
            }
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


        if(DpadCanPress)
        {
            if (Input.GetAxis("Dpad_X") == 1)
            {
                Debug.Log("Right D pad");
                DpadCanPress = false;
            }

            if (Input.GetAxis("Dpad_X") == -1)
            {
                Debug.Log("Left D pad");
                DpadCanPress = false;
            }

            if (Input.GetAxis("Dpad_Y") == 1)
            {
                if (selectorIndex > 0)
                {
                    selectorIndex--;

                    builder.CurrentAttachment = (AttachmentType)selectorIndex;
                    selector.rectTransform.position = new Vector3(selector.rectTransform.position.x, buttons[selectorIndex].transform.position.y);
                }

                DpadCanPress = false;
            }

            if (Input.GetAxis("Dpad_Y") == -1)
            {
                if (selectorIndex < buttons.Length-1)
                {
                    selectorIndex++;


                    builder.CurrentAttachment = (AttachmentType)selectorIndex;
                    selector.rectTransform.position = new Vector3(selector.rectTransform.position.x, buttons[selectorIndex].transform.position.y);
                }

                DpadCanPress = false;
            }

            if (Input.GetAxis("Dpad_Y") == 0)
            {
                DpadCanPress = true;
            }

            if(!DpadCanPress)
            {
                timeTillCanPress = Time.time + timeBetweenPresses;
            }
        }
        else
        {
            if(Time.time > timeTillCanPress)
            {
                DpadCanPress = true;
            }
        }
    }
}