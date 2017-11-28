using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private Transform currentPier = null;

    private Transform lastPier = null;

    [SerializeField]
    private Transform playerCentre;

    [SerializeField]
    private Image loading = null;

    [Header("Sounds")]

    [SerializeField]
    private string enterDockSound = "CHANGE";

    [SerializeField]
    private string defeatedEncounter = "CHANGE - defeatedEncounter";

    [SerializeField]
    private string dockMusic = "CHANGE DOCK MUSIC";

    [SerializeField]
    private string sailingMusic = "CHANGE SAILING MUSIC";

    [SerializeField]
    private float dockFadeIn = 2;

    [SerializeField]
    private float dockFadeOut = 2;

    [SerializeField]
    private float SailingFadeIn = 2;

    [SerializeField]
    private float SailingFadeOut = 2;

    [SerializeField]
    private float battleFadeIn = 2;

    [SerializeField]
    private float battleFadeOut = 2;

    [SerializeField]
    private float endBattleMusicDelay = 10;

    private bool cinematicPlaying = false;

    private bool playingBattleMusic = false;

    UIController UI;
    PlayerController PC;
    Player player;
    CameraController CC;
    ShipBuilding builder;

    private bool canPressY = true;

    private bool paused = false;

    private float nextTimeCancleBattle = 0;

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

        AudioManager.instance.FadeInMusic("ambientOcean", 2);
    }

    private void UnlockPlayerControl()
    {
        player.HasControl = true;
        PC.CanMove = true;

        if (loading != null)
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
        if (cinematicPlaying)
        {
            return;
        }

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
                UI.ShowMap(true);
            }

            if (Input.GetButtonDown("Start_Button"))
            {
                Pause();

                UI.showPauseMenu(paused);
            }

            if (Input.GetButtonUp("Back_Button"))
            {
                UI.ShowMap(false);
            }

            if(Time.time > nextTimeCancleBattle)
            {
                EndBattleMusic();
            }
        }
    }

    public void UnlockTrident()
    {
        builder.UnlockTrident();
    }

    public void UnlockArmour()
    {
        builder.UnlockArmour();
    }

    public void UnlockDropBear()
    {
        builder.UnlockDropBear();
    }

    public void EncounterDefeated(GameObject _cross)
    {
        AudioManager.instance.PlaySound(defeatedEncounter);

        StartCoroutine(PlayVictoryCinematic(4, _cross));
    }

    private IEnumerator PlayVictoryCinematic(float _delay, GameObject _cross)
    {
        yield return new WaitForSecondsRealtime(_delay);

        cinematicPlaying = true;
        Pause();
        UI.ShowMap(true);

        yield return new WaitForSecondsRealtime(1f);

        _cross.SetActive(true);

        yield return new WaitForSecondsRealtime(1.5f);

        AudioManager.instance.FadeOut(defeatedEncounter, 10f);
        UI.ShowMap(false);

        cinematicPlaying = false;
        Pause();
    }

    public void Pause()
    {
        paused = !paused;

        if (paused)
        {
            Debug.Log("Game is paused");
        }
        else
        {
            Debug.Log("Game is live");
        }

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

        AudioManager.instance.FadeOut(dockMusic, dockFadeOut);
        AudioManager.instance.FadeInMusic(sailingMusic, SailingFadeIn);

        GameState.BuildMode = false;

        CC.SwitchToPlayMode();
        PC.CanMove = true;
        player.HasControl = true;

        if (PC.HasSails)
        {
            Tutorial.instance.HasBuiltSail = true;
        }

        if (PC.HasCannons)
        {
            Tutorial.instance.HasBuiltCannon = true;
        }

        if (Tutorial.instance.HasBuiltSail == false)
        {
            Tutorial.instance.ShowSailsTip();
        }

        if (Tutorial.instance.HasBuiltCannon == false)
        {
            Tutorial.instance.ShowCannonsTip();
        }
    }

    private IEnumerator TransitionToBuildMode()
    {
        yield return new WaitForSeconds(1.2f);

        if (player.isDead)
        {
            player.Respawn();
        }

        AudioManager.instance.FadeOut(sailingMusic, SailingFadeOut);
        AudioManager.instance.FadeInMusic(dockMusic, dockFadeIn);

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
        else if (lastPier != null)
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

    public void RetreatPlayer()
    {
        UI.FadeScreen();
        PC.CanMove = false;
        player.HasControl = false;

        StartCoroutine(TransitionToBuildMode());
    }

    private void CanExitBuildMode()
    {
        canPressY = true;

        if (!GameState.BuildMode)
        {
            UI.ShowPierPopUp(true);
        }
    }

    public void PlayBattleMusic()
    {
        if (!playingBattleMusic)
        {
            AudioManager.instance.FadeOut(sailingMusic, SailingFadeOut);

            AudioManager.instance.FadeInMusic("BattleTheme", battleFadeIn / 2);

            if (!AudioManager.instance.isBattleMusicPlaying)
            {
                AudioManager.instance.FadeInMusic("BattleLoop", battleFadeIn);
                AudioManager.instance.isBattleMusicPlaying = true;
            }

            playingBattleMusic = true;
        }

        nextTimeCancleBattle = Time.time + endBattleMusicDelay;
    }

    public void EndBattleMusic()
    {
        if(playingBattleMusic)
        {
            AudioManager.instance.FadeOut("BattleLoop", battleFadeOut);

            AudioManager.instance.FadeInMusic(sailingMusic, SailingFadeIn);
            AudioManager.instance.isBattleMusicPlaying = false;

            playingBattleMusic = false;
        }
    }

    public void setPier(Transform _dockPos)
    {
        currentPier = _dockPos;

        if (_dockPos != null)
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