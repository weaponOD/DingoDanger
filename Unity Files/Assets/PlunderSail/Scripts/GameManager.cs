using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform pier = null;

    [SerializeField]
    private Transform playerCentre;

    UIController UI;
    PlayerController PC;
    CameraController CC;
    ShipBuilding builder;

    private void Awake()
    {
        UI = GetComponent<UIController>();
        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        CC = GetComponent<CameraController>();
        builder = GetComponent<ShipBuilding>();
    }

    private void Start()
    {
        builder.PlayerCentre = playerCentre;
        CC.PlayerCentre = playerCentre;

        GameState.BuildMode = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Y_Button"))
        {
            if (!GameState.BuildMode)
            {
                if (pier != null)
                {
                    UI.FadeScreen();
                    StartCoroutine(TransitionToBuildMode());
                    UI.ShowPierPopUp(false);
                }
            }
            else
            {
                UI.FadeScreen();
                StartCoroutine(TransitionToPlayMode());
            }
        }
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

        PC.transform.position = pier.position;
        PC.transform.rotation = pier.rotation;

        PC.transform.GetChild(0).localPosition = Vector3.zero;
        PC.transform.GetChild(0).localRotation = Quaternion.identity;


        CC.SwitchToBuildMode();
        builder.moveGridToPlayer(pier);
    }

    public void setPier(Transform _dockPos)
    {
        pier = _dockPos;

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