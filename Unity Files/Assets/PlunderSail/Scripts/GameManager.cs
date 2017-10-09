using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform pier = null;

    UIManager Ui;
    PlayerController PC;

    private void Awake()
    {
        Ui = GetComponent<UIManager>();
        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    private void Start()
    {
        GameState.BuildMode = false;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Y_Button"))
        {
            if(!GameState.BuildMode)
            {
                if (pier != null)
                {
                    PC.moveToPier(true, pier);
                    Ui.ShowPierPopUp(false);
                }
            }
            else
            {
                GameState.BuildMode = false;
                PC.moveToPier(false, pier);

                PC.gameObject.transform.RotateAroundLocal(Vector3.up, 180f);
            }
        }
    }

    public void AtDock()
    {
        if(!GameState.BuildMode)
        {
            GameState.BuildMode = true;
        }
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
            if(buildMode != value)
            {
                buildMode = value;
            }

            if(buildModeChanged != null)
            {
                buildModeChanged(buildMode);
            }
        }
    }
}