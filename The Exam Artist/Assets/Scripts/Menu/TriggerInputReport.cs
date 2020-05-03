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
    // Bind Actions
    [Tooltip("Up on Left Pad")]
    public SteamVR_Action_Boolean LeftNorth; 
    [Tooltip("Down on Left Pad")]
    public SteamVR_Action_Boolean LeftSouth;  
    [Tooltip("Up on Right Pad")]
    public SteamVR_Action_Boolean RightNorth;
    [Tooltip("Down on Right Pad")]
    public SteamVR_Action_Boolean RightSouth;

    // Input Sources
    [Tooltip("Left hand")]
    public SteamVR_Input_Sources left;
    [Tooltip("Right hand")]
    public SteamVR_Input_Sources right;

    [Tooltip("Script ScoreCalulate")]
    public ScoreCalculate report;
    [Tooltip("Score board object")]
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
        // Bind functions to actions
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
