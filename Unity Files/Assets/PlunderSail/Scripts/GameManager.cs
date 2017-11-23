using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform currentPier = null;

    [SerializeField]
    private Transform lastPier = null;

    [SerializeField]
    private Transform playerCentre;

    [SerializeField]
    private Image loading = null;

    [Header("Sounds")]

    [SerializeField]
    private string enterDockSound = "CHANGE";

    UIController UI;
    PlayerController PC;
    Player player;
    CameraController CC;
    ShipBuilding builder;

    private bool canPressY = true;

    private bool paused = false;

    private bool mapActive = false;

    private void Awake()
    {
        UI = GetComponent<UIController>();
        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        player = PC.GetComponent<Player>();

        player.OnDeath += PlayerDead;

        CC = GetComponent<CameraController>();
        builder = GetComponent<ShipBuilding>();
    }

    private void Start()
    {
        builder.PlayerCentre = playerCentre;
        CC.PlayerCentre = playerCentre;

        GameState.BuildMode = false;

        Invoke("UnlockPlayerControl", 0.5f);
    }

    private void UnlockPlayerControl()
    {
        player.HasControl = true;
        PC.CanMove = true;
        
        if(loading != null)
        {
            loading.gameObject.SetActive(false);
        }

        UI.FadeIn();
    }

    private void PlayerDead(LivingEntity _player)
    {
        Invoke("RespawnPlayer", 1.7f);
    }

    private void RespawnPlayer()
    {
        UI.FadeScreen();
        PC.CanMove = false;
        player.HasControl = false;
        
        StartCoroutine(TransitionToBuildMode());
    }

    private void Update()
    {
        if (Input.GetButtonDown("Y_Button"))
        {
            if (canPressY && !GameState.Paused)
            {
                if (!GameState.BuildMode)
                {
                    if (currentPier != null)
                    {
                        UI.FadeScreen();
                        PC.CanMove = false;
                        player.HasControl = false;
                        StartCoroutine(TransitionToBuildMode());
                        UI.ShowPierPopUp(false);
                        canPressY = false;

                        player.RepairAttachments();

                        AudioManager.instance.PlaySound(enterDockSound);

                        Invoke("CanExitBuildMode", 2);
                    }
                }
                else
                {
                    UI.FadeScreen();
                    StartCoroutine(TransitionToPlayMode());
                    canPressY = false;

                    Invoke("CanExitBuildMode", 2);
                }
            }
        }

        if (!GameState.BuildMode)
        {
            if (Input.GetButtonDown("Back_Button") && !paused)
            {
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                //Pause();

                mapActive = true;
                UI.ShowMap(mapActive);
            }

            if (Input.GetButtonDown("Start_Button"))
            {
                Pause();

                UI.showPauseMenu(paused);
            }

            if (Input.GetButtonUp("Back_Button") )
            {
                //Pause();

                mapActive = false;
                UI.ShowMap(mapActive);
            }
        }
    }

    public void Pause()
    {
        paused = !paused;

        GameState.Paused = paused;

        if (paused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    private IEnumerator TransitionToPlayMode()
    {
        yield return new WaitForSeconds(1.2f);

        GameState.BuildMode = false;

        CC.SwitchToPlayMode();
        PC.CanMove = true;
        player.HasControl = true;
    }

    private IEnumerator TransitionToBuildMode()
    {
        yield return new WaitForSeconds(1.2f);

        if(player.isDead)
        {
            player.Respawn();
        }

        GameState.BuildMode = true;

        if (currentPier != null)
        {
            PC.transform.position = currentPier.position;
            PC.transform.rotation = currentPier.rotation;

            PC.transform.GetChild(0).localPosition = Vector3.zero;
            PC.transform.GetChild(0).localRotation = Quaternion.identity;

            PC.ResetHeading();

            CC.SwitchToBuildMode();
            builder.moveGridToPlayer(currentPier);
        }
        else if(lastPier != null)
        {
            PC.transform.position = lastPier.position;
            PC.transform.rotation = lastPier.rotation;

            PC.transform.GetChild(0).localPosition = Vector3.zero;
            PC.transform.GetChild(0).localRotation = Quaternion.identity;

            PC.ResetHeading();

            CC.SwitchToBuildMode();
            builder.moveGridToPlayer(lastPier);
        }
    }

    private void CanExitBuildMode()
    {
        canPressY = true;
    }


    public void setPier(Transform _dockPos)
    {
        currentPier = _dockPos;

        if(_dockPos != null)
        {
            lastPier = _dockPos;
        }

        canPressY = (currentPier != null);

        CC.MoveBuildCameraToPier(currentPier);
        UI.ShowPierPopUp((currentPier != null));
    }
}

public static class GameState
{
    public delegate void BuildModeEnabled(bool isBuildMode);
    public static event BuildModeEnabled buildModeChanged;

    private static bool buildMode = false;

    private static bool paused = false;

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

    public static bool Paused
    {
        get { return paused; }

        set { paused = value; }
    }
}