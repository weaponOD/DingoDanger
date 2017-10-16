﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform pier = null;

    UIManager UI;
    PlayerControllerTest PC;
    CameraController CC;

    private void Awake()
    {
        UI = GetComponent<UIManager>();
        PC = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerTest>();
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
                    UI.TransitionToBuild();
                    StartCoroutine(MovePlayerToPier());
                    UI.ShowPierPopUp(false);
                }
            }
            else
            {
                GameState.BuildMode = false;
                //PC.transform.Rotate(Vector3.up, 180f, Space.Self);
            }
        }
    }

    private IEnumerator MovePlayerToPier()
    {
        yield return new WaitForSeconds(1.2f);
        GameState.BuildMode = true;

        PC.transform.position = pier.position;
        PC.transform.rotation = pier.rotation;

        PC.transform.GetChild(0).localPosition = Vector3.zero;
        PC.transform.GetChild(0).localRotation = Quaternion.identity;
    }

    public void setPier(Transform _dockPos)
    {
        Debug.Log("Pier is set");
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