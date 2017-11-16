using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    // UI References

    [SerializeField]
    private float goldDisplayTime = 2;

    [Header("Drag References into these")]
    [SerializeField]
    private GameObject Canvas = null;

    [SerializeField]
    private GameObject buildPanel = null;

    [SerializeField]
    private GameObject DockPopUp = null;

    [SerializeField]
    private GameObject goldHUD = null;

    [SerializeField]
    private Text goldText = null;

    [SerializeField]
    private Image fadePlane = null;

    [SerializeField]
    private GameObject[] menuItems = null;

    [SerializeField]
    private GameObject mapMenu = null;

    int selectedMenuItem = 0;

    // System References
    private Player player = null;

    private PlayerController playerController = null;

    private ShipBuilding builder = null;

    private GameManager GM = null;

    private CameraController CC = null;

    [SerializeField]
    private GameObject[] horizontalMenu = null;

    [SerializeField]
    private Slider speedSlider = null; 

    // Functionality variables
    private int selectedGenre;

    private string[] genres;

    private float timeTillCanPress;

    [SerializeField]
    private float timeBetweenPresses;

    // Functionality variables
    private int selectedAttachment;

    private bool DpadCanPress = false;

    private bool paused = false;

    private int currentMenu = 0;

    private void Awake()
    {
        // Subscribe to game state
        GameState.buildModeChanged += SetBuildPanelStatus;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        playerController = player.GetComponent<PlayerController>();

        builder = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ShipBuilding>();

        GM = builder.gameObject.GetComponent<GameManager>();

        CC = GM.GetComponent<CameraController>();

        Canvas = GameObject.FindGameObjectWithTag("Canvas");

        // Subscribe to player's gold change
        player.GoldChanged += GoldChanged;

        if (Canvas)
        {
            // Look for BuildingUI
            if (Canvas.transform.Find("BuildingUI"))
            {
                buildPanel = Canvas.transform.Find("BuildingUI").gameObject;
                buildPanel.SetActive(false);
            }
            else
            {
                Debug.LogError("Canvas does not have child called BuildingUI.");
            }

            // Look for PierPopUp
            if (Canvas.transform.Find("PierPopUp"))
            {
                DockPopUp = Canvas.transform.Find("PierPopUp").gameObject;
                DockPopUp.SetActive(false);
            }
            else
            {
                Debug.LogError("Canvas does not have child called PierPopUp.");
            }

            // Look for GoldHUD
            if (Canvas.transform.Find("GoldHUD"))
            {
                goldHUD = Canvas.transform.Find("GoldHUD").gameObject;
                goldHUD.SetActive(false);
                goldText = goldHUD.transform.GetComponentInChildren<Text>();
            }
            else
            {
                Debug.LogError("Canvas does not have child called GoldHUD.");
            }

            // Look for Fade Plane
            if (Canvas.transform.Find("Fade"))
            {
                fadePlane = Canvas.transform.Find("Fade").GetComponent<Image>();
            }
        }
        else
        {
            Debug.LogError("Canvas could not be found.");
        }

        genres = new string[5];

        genres[0] = "Cabin";
        genres[1] = "Cannon";
        genres[2] = "Sail";
        genres[3] = "Armour";
        genres[4] = "Ram";
    }

    private void Start()
    {
        selectedGenre = 0;

        selectedAttachment = 0;

        horizontalMenu[selectedGenre].SetActive(true);

        DpadCanPress = true;

        goldText.text = "" + player.Gold;

        UpdateSpeedSlider();
    }

    private void GoldChanged()
    {
        goldText.text = "" + player.Gold;

        if (!GameState.BuildMode)
        {
            goldHUD.SetActive(true);

            Invoke("HideGoldHUD", goldDisplayTime);
        }
    }

    private void HideGoldHUD()
    {
        goldHUD.SetActive(false);
    }

    public void UpdateSpeedSlider()
    {
        //speedSlider.minValue = 0;
        //speedSlider.maxValue = ;
        
        float percent = playerController.MaxSpeed / playerController.CappedSpeed;
        speedSlider.value = percent;
    }

    private void Update()
    {
        if (paused)
        {
            // Interact with the currently selected button
            if (Input.GetButtonDown("A_Button"))
            {
                ProcessInput(selectedMenuItem);
            }

            // move back to the pause menu
            if (Input.GetButtonDown("B_Button"))
            {
                ProcessInput(-1);
            }

            if (DpadCanPress)
            {
                // Move up the vertical menu
                if (Input.GetAxis("Dpad_Y") == 1)
                {
                    if (selectedMenuItem > 0)
                    {
                        selectedMenuItem--;

                        DpadCanPress = false;
                    }
                }

                // Move down the vertical menu
                if (Input.GetAxis("Dpad_Y") == -1)
                {
                    if (selectedMenuItem < 3)
                    {
                        selectedMenuItem++;

                        DpadCanPress = false;
                    }
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

                if (Input.GetAxis("Dpad_Y") == 0 && Input.GetAxis("Dpad_X") == 0)
                {
                    DpadCanPress = true;
                }
            }
        }
        else
        {
            if (DpadCanPress)
            {
                // Move up the vertical menu
                if (Input.GetAxis("Dpad_Y") == 1)
                {
                    if (selectedGenre > 0)
                    {
                        ChangeGenreSelection(-1);

                        DpadCanPress = false;
                    }
                }

                // Move down the vertical menu
                if (Input.GetAxis("Dpad_Y") == -1)
                {
                    if (selectedGenre < horizontalMenu.Length - 1)
                    {
                        ChangeGenreSelection(1);

                        DpadCanPress = false;
                    }
                }

                // Move right along the horizontal menu
                if (Input.GetAxis("Dpad_X") == 1)
                {
                    if (selectedAttachment < horizontalMenu[selectedGenre].transform.childCount - 1)
                    {
                        ChangeAttachmentSelection(+1);

                        DpadCanPress = false;
                    }
                }

                // Move left along the horizontal menu
                if (Input.GetAxis("Dpad_X") == -1)
                {
                    if (selectedAttachment > 0)
                    {
                        ChangeAttachmentSelection(-1);

                        DpadCanPress = false;
                    }
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

                if (Input.GetAxis("Dpad_Y") == 0 && Input.GetAxis("Dpad_X") == 0)
                {
                    DpadCanPress = true;
                }
            }
        }
    }

    private void ProcessInput(int _buttonSelected)
    {
        if (_buttonSelected == -1)
        {
            menuItems[currentMenu].SetActive(false);
            menuItems[0].SetActive(true);
            currentMenu = 0;
        }

        // Pause menu
        if (currentMenu == 0)
        {
            PauseMenuInput(_buttonSelected);
            selectedMenuItem = 0;
        }

        // Sound Menu
        if (currentMenu == 1)
        {
            SoundMenuInput(_buttonSelected);
            selectedMenuItem = 0;
        }

        // controls Menu
        if (currentMenu == 2)
        {
            ControlsMenuInput(_buttonSelected);
            selectedMenuItem = 0;
        }
    }

    private void PauseMenuInput(int _button)
    {
        if (_button == 0)
        {
            GM.Pause();
            showPauseMenu(false);
        }

        if (_button == 1)
        {
            menuItems[0].SetActive(false);
            menuItems[1].SetActive(true);
            currentMenu = 1;
        }

        if (_button == 2)
        {
            menuItems[0].SetActive(false);
            menuItems[2].SetActive(true);
            currentMenu = 2;
        }

        if (_button == 3)
        {
            Application.Quit();
        }
    }


    private void SoundMenuInput(int _button)
    {
        if (_button == 0)
        {

        }

        if (_button == 1)
        {

        }

        if (_button == 2)
        {

        }

        if (_button == 3)
        {

        }
    }

    private void ControlsMenuInput(int _button)
    {
        if (_button == 0)
        {
            CC.InvertY();
        }

        if (_button == 1)
        {
            CC.InvertX();
        }
    }

    public void showPauseMenu(bool _isPaused)
    {
        ShowMap(false);

        paused = _isPaused;

        menuItems[0].SetActive(_isPaused);
        currentMenu = 0;

        if (!_isPaused)
        {
            foreach (GameObject item in menuItems)
            {
                item.SetActive(false);
            }
        }
    }

    public void ShowMap(bool _show)
    {
        mapMenu.SetActive(_show);
    }

    private void ChangeGenreSelection(int _change)
    {
        horizontalMenu[selectedGenre].SetActive(false);

        selectedAttachment = 0;

        selectedGenre += _change;

        builder.UpdatePreview(genres[selectedGenre] + "00" + (selectedAttachment + 1));

        horizontalMenu[selectedGenre].SetActive(true);
    }

    private void ChangeAttachmentSelection(int _change)
    {
        selectedAttachment += _change;

        builder.UpdatePreview(genres[selectedGenre] + "00" + (selectedAttachment + 1));
    }

    public void ShowPierPopUp(bool _show)
    {
        DockPopUp.SetActive(_show);
    }

    public void FadeScreen()
    {
        StartCoroutine(FadeScreenYoyo());
    }

    private IEnumerator FadeScreenYoyo()
    {
        StartCoroutine(Fade(Color.clear, Color.black, 1));

        yield return new WaitForSeconds(1.2f);

        StartCoroutine(Fade(Color.black, Color.clear, 1));
    }


    // Fades the fadePlane image from a colour to another over x seconds.
    private IEnumerator Fade(Color from, Color to, float time)
    {
        float speed = 1 / time;
        float percent = 0;

        while (percent < 1)
        {
            percent += Time.deltaTime * speed;
            fadePlane.color = Color.Lerp(from, to, percent);

            yield return null;
        }
    }

    public void SetBuildPanelStatus(bool _isEnabled)
    {
        ShowMap(false);

        buildPanel.SetActive(_isEnabled);

        goldHUD.SetActive(_isEnabled);

        if (_isEnabled)
        {
            //buttons[selectedIndex].image.color = Color.green;
        }
    }

    private void OnDestroy()
    {
        // un-Subscribe to game state
        GameState.buildModeChanged -= SetBuildPanelStatus;

        // un-Subscribe to player's gold change
        player.GoldChanged -= GoldChanged;
    }
}