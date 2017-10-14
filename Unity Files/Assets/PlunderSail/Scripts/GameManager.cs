using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform pier = null;

    UIManager Ui;
    PlayerController PC;
    CameraController CC;

    private void Awake()
    {
        Ui = GetComponent<UIManager>();
        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        CC = GetComponent<CameraController>();
    }

    private void Start()
    {
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
                    Ui.TransitionToBuild();
                    StartCoroutine(MovePlayerToPier());
                    Ui.ShowPierPopUp(false);
                }
            }
            else
            {
                GameState.BuildMode = false;
                PC.transform.Rotate(Vector3.up, 180f, Space.Self);
            }
        }
    }

    private IEnumerator MovePlayerToPier()
    {
        yield return new WaitForSeconds(1.2f);

        PC.transform.position = pier.position;
        PC.transform.rotation = pier.rotation;
        CC.MoveBuildCameraToPier(pier);

        GameState.BuildMode = true;
    }

    public void setPier(Transform _dockPos)
    {
        pier = _dockPos;
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