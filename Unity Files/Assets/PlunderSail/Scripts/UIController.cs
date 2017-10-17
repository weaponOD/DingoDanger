using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    // UI References
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
    private Image fadePlane;

    // System References
    private Player player = null;

    private ShipBuilder builder = null;

    [SerializeField]
    private GameObject[] horizontalMenu;

    private Image verticalMenu = null;

    // Functionality variables
    [SerializeField]
    private int selectedGenre;

    private float timeTillCanPress;

    [SerializeField]
    private float timeBetweenPresses;

    // Functionality variables
    private int selectedAttachment;

    private bool DpadCanPress = false;

    private void Awake()
    {
        // Subscribe to game state
        GameState.buildModeChanged += SetBuildPanelStatus;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        builder = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ShipBuilder>();

        Canvas = GameObject.FindGameObjectWithTag("Canvas");

        if (Canvas)
        {
            // Look for BuildingUI
            if (Canvas.transform.Find("BuildingUI"))
            {
                buildPanel = Canvas.transform.Find("BuildingUI").gameObject;
                buildPanel.SetActive(false);

                // Look for Vertical Panel in BuildingUI
                if (Canvas.transform.GetChild(0).Find("VerticalPanel"))
                {
                    verticalMenu = Canvas.transform.GetChild(0).Find("VerticalPanel").GetComponent<Image>();
                }
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
    }

    private void Start()
    {
        selectedGenre = horizontalMenu.Length - 1;

        selectedAttachment = 0;
        horizontalMenu[selectedGenre].SetActive(true);

        DpadCanPress = true;
    }

    private void Update()
    {
        if (DpadCanPress)
        {
            // Move up the vertical menu
            if (Input.GetAxis("Dpad_Y") == 1)
            {
                if (selectedGenre > 0)
                {
                    Debug.Log("Moving Up");

                    ChangeSelection(-1);

                    DpadCanPress = false;
                }
            }

            // Move down the vertical menu
            if (Input.GetAxis("Dpad_Y") == -1)
            {
                if (selectedGenre < horizontalMenu.Length - 1) 
                {
                    Debug.Log("Moving Down");

                    ChangeSelection(1);

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

            if (Input.GetAxis("Dpad_Y") == 0)
            {
                DpadCanPress = true;
            }
        }
    }

    private void ChangeSelection(int _change)
    {
        horizontalMenu[selectedGenre].SetActive(false);

        selectedGenre += _change;

        MoveVerticalMenu(_change);

        horizontalMenu[selectedGenre].SetActive(true);
    }

    private void MoveVerticalMenu(int _change)
    {
        verticalMenu.rectTransform.Translate(Vector2.up * _change * 60f);
    }

    public void ShowPierPopUp(bool _show)
    {
        DockPopUp.SetActive(_show);
    }

    public void TransitionToBuild()
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
        buildPanel.SetActive(_isEnabled);

        if (_isEnabled)
        {
            //buttons[selectedIndex].image.color = Color.green;
        }
    }

    private void OnDestroy()
    {
        // un-Subscribe to game state
        GameState.buildModeChanged -= SetBuildPanelStatus;
    }
}