using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    // UI References
    [SerializeField]
    private float goldDisplayTime = 2;

    [SerializeField]
    private float timeBetweenPresses;

    [Header("Button State Sprites")]

    [SerializeField]
    private Sprite defaultSprite = null;

    [SerializeField]
    private Sprite genreHighlightSprite = null;

    [SerializeField]
    private Sprite highlightSprite = null;

    [SerializeField]
    private Sprite pauseHighlightSprite = null;

    // Panel References

    private GameObject Canvas = null;

    private GameObject buildPanel = null;

    private GameObject DockPopUp = null;

    private GameObject goldHUD = null;

    private Text goldText = null;

    private Image fadePlane = null;

    // System References
    private Player player = null;

    private PlayerController playerController = null;

    private ShipBuilding builder = null;

    private GameManager GM = null;

    private CameraController CC = null;

    private GameObject[] horizontalMenu = null;

    private Image[] genreImage = null;

    // Sliders
    private Slider speedSlider = null;

    private Slider musicSlider = null;

    private Slider soundSlider = null;

    [SerializeField]
    private Slider sensitivitySlider = null;

    private Slider selectedSlider = null;

    private Menu[] menu;

    private GameObject mapMenu = null;

    // Functionality variables
    private int selectedGenre;

    private string[] genres;

    private float timeTillCanPress;

    // Functionality variables
    private int selectedAttachment;

    private bool DpadCanPress = false;

    private bool paused = false;

    [SerializeField]
    private int currentMenu = 0;

    [SerializeField]
    private int currentButton = 0;

    [SerializeField]
    private bool sliderSelected = false;

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


                Transform veritcalPanel = buildPanel.transform.Find("VerticalPanel");

                horizontalMenu = new GameObject[4];
                genreImage = new Image[4];

                genreImage[0] = veritcalPanel.GetChild(0).GetComponent<Image>();
                genreImage[1] = veritcalPanel.GetChild(0).GetComponent<Image>();
                genreImage[2] = veritcalPanel.GetChild(0).GetComponent<Image>();
                genreImage[3] = veritcalPanel.GetChild(0).GetComponent<Image>();


                horizontalMenu[0] = veritcalPanel.GetChild(0).GetChild(1).gameObject;
                horizontalMenu[1] = veritcalPanel.GetChild(1).GetChild(1).gameObject;
                horizontalMenu[2] = veritcalPanel.GetChild(2).GetChild(1).gameObject;
                horizontalMenu[3] = veritcalPanel.GetChild(3).GetChild(1).gameObject;

                speedSlider = buildPanel.GetComponentInChildren<Slider>();
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

            // Look for options

            if (Canvas.transform.Find("Options"))
            {
                menu = new Menu[4];

                // Map menu
                mapMenu = Canvas.transform.Find("Options").GetChild(1).gameObject;

                // pause
                menu[0] = new Menu("pauseMenu", Canvas.transform.Find("Options").GetChild(0).gameObject);

                // pause
                menu[1] = new Menu("pauseMenu", Canvas.transform.Find("Options").GetChild(1).gameObject);

                // Audio
                menu[2] = new Menu("audioMenu", Canvas.transform.Find("Options").GetChild(2).gameObject);

                // controls
                menu[3] = new Menu("controlsMenu", Canvas.transform.Find("Options").GetChild(3).gameObject);
            }
            else
            {
                Debug.LogError("Canvas does not have child called Options.");
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
        genreImage[selectedGenre].sprite = genreHighlightSprite;

        DpadCanPress = true;

        goldText.text = "" + player.Gold;

        UpdateSpeedSlider();


        // populate pause screen buttons

        for (int x = 0; x < menu.Length; x++)
        {
            Button[] buttons = menu[x].menuScreen.GetComponentsInChildren<Button>();

            menu[x].buttons = new Image[buttons.Length];

            for (int i = 0; i < buttons.Length; i++)
            {
                menu[x].buttons[i] = buttons[i].GetComponent<Image>();
                menu[x].buttons[0].sprite = pauseHighlightSprite;
            }
        }

        musicSlider = menu[2].buttons[0].GetComponentInChildren<Slider>();
        soundSlider = menu[2].buttons[1].GetComponentInChildren<Slider>();

        sensitivitySlider = menu[3].buttons[2].GetComponentInChildren<Slider>();
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
        float percent = playerController.MaxSpeed / playerController.CappedSpeed;
        speedSlider.value = percent;
    }

    private void Update()
    {
        if (paused)
        {
            currentButton = menu[currentMenu].selectedIndex;
            // move back to the pause menu
            if (Input.GetButtonDown("B_Button"))
            {
                ProcessInput(true);
            }

            // Interact with the currently selected button
            if (Input.GetButtonDown("A_Button"))
            {
                ProcessInput(false);
            }

            if (DpadCanPress)
            {
                // Move up the vertical menu
                if (Input.GetAxis("Dpad_Y") == 1)
                {
                    if (menu[currentMenu].selectedIndex > 0)
                    {
                        menu[currentMenu].buttons[menu[currentMenu].selectedIndex].sprite = defaultSprite;

                        ChangeSelectedButton(-1);

                        menu[currentMenu].buttons[menu[currentMenu].selectedIndex].sprite = pauseHighlightSprite;

                        DpadCanPress = false;
                    }
                }

                // Move down the vertical menu
                if (Input.GetAxis("Dpad_Y") == -1)
                {
                    if (menu[currentMenu].selectedIndex < menu[currentMenu].buttons.Length)
                    {
                        menu[currentMenu].buttons[menu[currentMenu].selectedIndex].sprite = defaultSprite;

                        ChangeSelectedButton(1);

                        menu[currentMenu].buttons[menu[currentMenu].selectedIndex].sprite = pauseHighlightSprite;

                        DpadCanPress = false;
                    }
                }

                if (sliderSelected)
                {
                    // Increase slider
                    if (Input.GetAxis("Dpad_X") == 1)
                    {
                        selectedSlider.value += 0.01f;

                        if(selectedSlider == sensitivitySlider)
                        {
                            CC.setSensitivity(selectedSlider.value);
                        }
                        else if(selectedSlider == soundSlider)
                        {
                            AudioManager.instance.SetSoundLevel(selectedSlider.value);
                        }
                        else if(selectedSlider == musicSlider)
                        {
                            AudioManager.instance.SetMusicLevel(selectedSlider.value);
                        }
                    }

                    if (Input.GetAxis("Dpad_X") == -1)
                    {
                        selectedSlider.value -= 0.01f;

                        if (selectedSlider == sensitivitySlider)
                        {
                            CC.setSensitivity(selectedSlider.value);
                        }
                        else if (selectedSlider == soundSlider)
                        {
                            AudioManager.instance.SetSoundLevel(selectedSlider.value);
                        }
                        else if (selectedSlider == musicSlider)
                        {
                            AudioManager.instance.SetMusicLevel(selectedSlider.value);
                        }
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
                    else
                    {
                        ChangeGenreSelection(horizontalMenu.Length - 1);
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
                    else
                    {
                        ChangeGenreSelection(-selectedGenre);

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

    private void ChangeSelectedButton(int _change)
    {
        menu[currentMenu].selectedIndex += _change;

        // Audio Menu
        if (currentMenu == 2)
        {
            if (menu[2].selectedIndex == 0)
            {
                sliderSelected = true;
                selectedSlider = musicSlider;
            }

            if (menu[2].selectedIndex == 1)
            {
                sliderSelected = true;
                selectedSlider = soundSlider;
            }

            if (menu[2].selectedIndex == 2)
            {
                sliderSelected = false;
                selectedSlider = null;
            }
        }

        if (currentMenu == 3)
        {
            if (menu[3].selectedIndex == 2)
            {
                sliderSelected = true;
                selectedSlider = sensitivitySlider;
            }
            else
            {
                sliderSelected = false;
                selectedSlider = null;
            }

        }
    }

    private void ProcessInput(bool _back)
    {
        // Return to pause screen
        if (_back)
        {
            // if in pause menu return to game
            if (currentMenu == 0)
            {
                ResumeGame();
            }
            else
            {
                // Don't change button settings when return from map
                if(currentMenu != 1)
                {
                    menu[currentMenu].buttons[menu[currentMenu].selectedIndex].sprite = defaultSprite;

                    menu[currentMenu].buttons[0].sprite = pauseHighlightSprite;

                    menu[currentMenu].selectedIndex = 0;

                    sliderSelected = false;
                }

                menu[currentMenu].menuScreen.SetActive(false);

                currentMenu = 0;

                menu[currentMenu].menuScreen.SetActive(true);
            }

            return;
        }

        // Pause menu
        if (currentMenu == 0)
        {
            PauseMenuInput();
        }

        // Map menu
        if (currentMenu == 1)
        {
        }

        // Sound Menu
        if (currentMenu == 2)
        {
            SoundMenuInput();
        }

        // controls Menu
        if (currentMenu == 3)
        {
            ControlsMenuInput();
        }
    }

    private void ResumeGame()
    {
        GM.Pause();
        showPauseMenu(false);
    }

    private void PauseMenuInput()
    {
        // Resume Game
        if (menu[0].selectedIndex == 0)
        {
            ResumeGame();
        }

        // Open Map
        if (menu[0].selectedIndex == 1)
        {
            menu[0].menuScreen.SetActive(false);
            menu[1].menuScreen.SetActive(true);
            currentMenu = 1;
        }

        // go to sound Menu
        if (menu[0].selectedIndex == 2)
        {
            menu[0].menuScreen.SetActive(false);
            menu[2].menuScreen.SetActive(true);
            currentMenu = 2;

            sliderSelected = true;

            selectedSlider = musicSlider;
        }

        // go to controls Menu
        if (menu[0].selectedIndex == 3)
        {
            menu[0].menuScreen.SetActive(false);
            menu[3].menuScreen.SetActive(true);
            currentMenu = 3;
        }

        if (menu[0].selectedIndex == 4)
        {
            Application.Quit();
        }
    }

    private void SoundMenuInput()
    {
        if (menu[2].selectedIndex == 0)
        {
            selectedSlider = musicSlider;
        }

        if (menu[2].selectedIndex == 1)
        {
            selectedSlider = soundSlider;
        }

        if (menu[2].selectedIndex == 2)
        {
            // go back to pause screen
            menu[2].menuScreen.SetActive(false);

            menu[2].buttons[menu[2].selectedIndex].sprite = defaultSprite;

            menu[2].buttons[0].sprite = pauseHighlightSprite;

            menu[2].selectedIndex = 0;

            currentMenu = 0;

            menu[currentMenu].menuScreen.SetActive(true);
        }
    }

    private void ControlsMenuInput()
    {
        if (menu[3].selectedIndex == 0)
        {
            CC.InvertY();
        }

        if (menu[3].selectedIndex == 1)
        {
            CC.InvertX();
        }

        if (menu[3].selectedIndex == 3)
        {
            // go back to pause screen
            menu[currentMenu].menuScreen.SetActive(false);

            menu[currentMenu].buttons[menu[currentMenu].selectedIndex].sprite = defaultSprite;

            menu[currentMenu].buttons[0].sprite = pauseHighlightSprite;

            menu[currentMenu].selectedIndex = 0;

            currentMenu = 0;

            menu[currentMenu].menuScreen.SetActive(true);
        }
    }

    public void showPauseMenu(bool _isPaused)
    {
        paused = _isPaused;

        // hide map when pausing the game
        if (paused)
        {
            ShowMap(false);
        }

        menu[currentMenu].menuScreen.SetActive(_isPaused);
        currentMenu = 0;
    }

    public void ShowMap(bool _show)
    {
        mapMenu.SetActive(_show);
    }

    private void ChangeGenreSelection(int _change)
    {
        horizontalMenu[selectedGenre].SetActive(false);
        genreImage[selectedGenre].sprite = defaultSprite;

        selectedAttachment = 0;
        horizontalMenu[selectedGenre].transform.GetChild(selectedAttachment).GetComponent<Image>().sprite = highlightSprite;

        selectedGenre += _change;

        builder.UpdatePreview(genres[selectedGenre] + "00" + (selectedAttachment + 1));

        horizontalMenu[selectedGenre].SetActive(true);
        genreImage[selectedGenre].sprite = genreHighlightSprite;
    }

    private void ChangeAttachmentSelection(int _change)
    {
        horizontalMenu[selectedGenre].transform.GetChild(selectedAttachment).GetComponent<Image>().sprite = defaultSprite;
        selectedAttachment += _change;

        horizontalMenu[selectedGenre].transform.GetChild(selectedAttachment).GetComponent<Image>().sprite = highlightSprite;

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

    public void FadeIn()
    {
        StartCoroutine(Fade(Color.black, Color.clear, 1));
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

[System.Serializable]
public class Menu
{
    public string name;
    public GameObject menuScreen;
    public Image[] buttons;
    public int selectedIndex;

    public Menu(string _name, GameObject _menuScreen)
    {
        name = _name;
        menuScreen = _menuScreen;
        selectedIndex = 0;
    }
}