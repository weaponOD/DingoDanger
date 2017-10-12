using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    // UI References
    [SerializeField]
    private Text playerGold;

    [SerializeField]
    private Button[] buttons;

    [SerializeField]
    private GameObject Panel;

    private int selectedIndex = 0;

    [SerializeField]
    private int[] prices;

    private ShipBuilder builder;

    private Player player;

    private bool DpadCanPress = false;

    private bool placePreview;

    [SerializeField]
    private float timeBetweenPresses = 1f;
    private float timeTillCanPress;

    [SerializeField]
    private Text pierPopup;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        builder = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ShipBuilder>();

        // Subscribe to game state
        GameState.buildModeChanged += SetBuildPanelStatus;

        UpdateUI();
    }

    private bool CanAfford()
    {
        int cost = prices[selectedIndex];

        if (player.Gold >= cost)
        {
            return true;
        }

        return false;
    }

    public void BuyAttachment()
    {
        int cost = prices[selectedIndex];

        if (player.Gold >= cost)
        {
            player.DeductGold(cost);
            UpdateUI();
        }
    }

    public void SetBuildPanelStatus(bool _isEnabled)
    {
        Panel.gameObject.SetActive(_isEnabled);

        if (_isEnabled)
        {
            buttons[selectedIndex].image.color = Color.green;
        }
    }

    public void UpdateUI()
    {
        for (int x = 0; x < buttons.Length; x++)
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
        playerGold.text = "" + player.Gold;

        // cheat gold Increased
        if (Input.GetKeyDown(KeyCode.KeypadPlus))
        {
            player.GiveGold(50);
            UpdateUI();
        }

        // cheat gold decrease
        if (Input.GetKeyDown(KeyCode.KeypadMinus))
        {
            player.DeductGold(50);
            UpdateUI();
        }

        // Check if the player wants to buy the selected attachment
        if (Input.GetButtonDown("A_Button"))
        {
            // check if player has enough gold to make purchase
            if (CanAfford() && !builder.HasPreview)
            {
                placePreview = true;
            }
        }

        if (Input.GetButtonUp("A_Button"))
        {
            if (placePreview)
            {
                // Call ship builder to instantiate the attachment ghost
                builder.SpawnPreviewAttachment(selectedIndex);
                placePreview = false;
            }
        }

        if (DpadCanPress)
        {
            if (Input.GetAxis("Dpad_X") == 1)
            {
                if (selectedIndex < buttons.Length - 1)
                {
                    buttons[selectedIndex].image.color = Color.white;
                    selectedIndex++;

                    if (builder.HasPreview)
                    {
                        builder.SpawnPreviewAttachment(selectedIndex);
                    }

                    buttons[selectedIndex].image.color = Color.green;
                }

                DpadCanPress = false;
            }

            if (Input.GetAxis("Dpad_X") == -1)
            {
                if (selectedIndex > 0)
                {
                    buttons[selectedIndex].image.color = Color.white;
                    selectedIndex--;

                    if (builder.HasPreview)
                    {
                        builder.SpawnPreviewAttachment(selectedIndex);
                    }

                    buttons[selectedIndex].image.color = Color.green;
                }

                DpadCanPress = false;
            }

            if (!DpadCanPress)
            {
                timeTillCanPress = Time.time + timeBetweenPresses;
            }
        }
        else
        {
            if (Time.time > timeTillCanPress)
            {
                DpadCanPress = true;
            }

            if (Input.GetAxis("Dpad_X") == 0)
            {
                DpadCanPress = true;
            }
        }
    }

    private void LateUpdate()
    {
        // Reset
        if (Input.GetButtonDown("Back_Button"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void ShowPierPopUp(bool _show)
    {
        pierPopup.enabled = _show;
    }

    void OnDestroy()
    {
        // un-Subscribe to game state
        GameState.buildModeChanged -= SetBuildPanelStatus;
    }
}