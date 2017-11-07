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

    // System References
    private Player player = null;

    private ShipBuilding builder = null;

    [SerializeField]
    private GameObject[] horizontalMenu = null;

    // Functionality variables
    [SerializeField]
    private int selectedGenre;

    private string[] genres;

    private float timeTillCanPress;

    [SerializeField]
    private float timeBetweenPresses;

    // Functionality variables
    [SerializeField]
    private int selectedAttachment;

    private bool DpadCanPress = false;

    private void Awake()
    {
        // Subscribe to game state
        GameState.buildModeChanged += SetBuildPanelStatus;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        builder = GameObject.FindGameObjectWithTag("GameManager").GetComponent<ShipBuilding>();

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
    }

    private void GoldChanged()
    {
        goldText.text = "" + player.Gold;
        goldHUD.SetActive(true);

        Invoke("HideGoldHUD", goldDisplayTime);
    }

    private void HideGoldHUD()
    {
        goldHUD.SetActive(false);
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
                Debug.Log(string.Format("There are {0} in the current genre", horizontalMenu[selectedGenre].transform.childCount));

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

            if(!DpadCanPress)
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

    private void ChangeGenreSelection(int _change)
    {
        horizontalMenu[selectedGenre].SetActive(false);

        selectedAttachment = 0;

        selectedGenre += _change;

        builder.UpdatePreview(genres[selectedGenre] + selectedAttachment);

        horizontalMenu[selectedGenre].SetActive(true);
    }

    private void ChangeAttachmentSelection(int _change)
    {
        selectedAttachment += _change;

        builder.UpdatePreview(genres[selectedGenre] + selectedAttachment);
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
    }
}