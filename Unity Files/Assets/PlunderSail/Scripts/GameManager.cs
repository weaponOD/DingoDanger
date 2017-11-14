using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform pier = null;

    [SerializeField]
    private Transform playerCentre;

    [Header("Sounds")]

    [SerializeField]
    private string enterDockSound = "CHANGE";

    UIController UI;
    PlayerController PC;
    CameraController CC;
    ShipBuilding builder;

    private bool canPressY = true;

    private bool paused = false;

    private bool mapActive = false;

    private void Awake()
    {
        UI = GetComponent<UIController>();
        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        CC = GetComponent<CameraController>();
        builder = GetComponent<ShipBuilding>();
    }

    private void Start()
    {
        string[] names = Input.GetJoystickNames();

        foreach (string n in names)
        {
            Debug.Log(n);
        }

        builder.PlayerCentre = playerCentre;
        CC.PlayerCentre = playerCentre;

        GameState.BuildMode = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Y_Button"))
        {
            if (canPressY)
            {
                if (!GameState.BuildMode)
                {
                    if (pier != null)
                    {
                        UI.FadeScreen();
                        StartCoroutine(TransitionToBuildMode());
                        UI.ShowPierPopUp(false);
                        canPressY = false;

                        AudioManager.instance.PlaySound(enterDockSound);

                        Invoke("CanExitBuildMode", 3);
                    }
                }
                else
                {
                    UI.FadeScreen();
                    StartCoroutine(TransitionToPlayMode());
                    canPressY = false;

                    Invoke("CanExitBuildMode", 3);
                }
            }
        }

        if (Input.GetButtonDown("Back_Button"))
        {
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            mapActive = !mapActive;
            UI.ShowMap(mapActive);
        }

        if (Input.GetButtonDown("Start_Button"))
        {
            Pause();
        }
    }

    public void Pause()
    {
        paused = !paused;

        if (paused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        UI.showPauseMenu(paused);
    }

    private IEnumerator TransitionToPlayMode()
    {
        yield return new WaitForSeconds(1.2f);

        GameState.BuildMode = false;

        CC.SwitchToPlayMode();
    }

    private IEnumerator TransitionToBuildMode()
    {
        yield return new WaitForSeconds(1.2f);

        GameState.BuildMode = true;

        if (pier != null)
        {
            PC.transform.position = pier.position;
            PC.transform.rotation = pier.rotation;

            PC.transform.GetChild(0).localPosition = Vector3.zero;
            PC.transform.GetChild(0).localRotation = Quaternion.identity;

            PC.ResetHeading();

            CC.SwitchToBuildMode();
            builder.moveGridToPlayer(pier);
        }
    }

    private void CanExitBuildMode()
    {
        canPressY = true;
    }


    public void setPier(Transform _dockPos)
    {
        pier = _dockPos;

        canPressY = (pier != null);

        CC.MoveBuildCameraToPier(pier);
        UI.ShowPierPopUp((pier != null));
    }
}

public static class GameState
{
    public delegate void BuildModeEnabled(bool isBuildMode);
    public static event BuildModeEnabled buildModeChanged;

    private static bool buildMode = false;

    public static bool BuildMode
    {
        get { return buildMode; }

        set
        {
            if (buildMode != value)
            {
                buildMode = value;
            }

            if (buildModeChanged != null)
            {
                buildModeChanged(buildMode);
            }
        }
    }
}