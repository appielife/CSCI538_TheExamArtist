using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

/*****************************************
Script for trigger input in GameOver scene 
*****************************************/

public class TriggerInputReport : MonoBehaviour
{
    public SteamVR_Action_Boolean LeftNorth;  // Up
    public SteamVR_Action_Boolean LeftSouth;  // Down
    public SteamVR_Action_Boolean RightNorth; // Up
    public SteamVR_Action_Boolean RightSouth; // Down

    public SteamVR_Input_Sources left;  // Left Controller
    public SteamVR_Input_Sources right; // Right Controller

    public ScoreCalculate report;
    public GameObject scoreboard;

    // IMPORTANT to destroy binded settings when scene change
    // Only if same setting for all scenes
    void OnDestroy()
    {
        LeftNorth.RemoveOnStateDownListener(TriggerDownU, left);
        LeftSouth.RemoveOnStateDownListener(TriggerDownD, left);
        RightNorth.RemoveOnStateDownListener(TriggerDownU, right);
        RightSouth.RemoveOnStateDownListener(TriggerDownD, right);
    }

    void Start()
    {
        LeftNorth.AddOnStateDownListener(TriggerDownU, left);
        LeftSouth.AddOnStateDownListener(TriggerDownD, left);
        RightNorth.AddOnStateDownListener(TriggerDownU, right);
        RightSouth.AddOnStateDownListener(TriggerDownD, right);
    }

    // Function for up 
    public void TriggerDownU(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (scoreboard.activeSelf)
        {
            report.prevReport();
        }
    }

    // Function for down
    public void TriggerDownD(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (scoreboard.activeSelf)
        {
            report.nextReport();
        }
    }
}
