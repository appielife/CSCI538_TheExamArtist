using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class TriggerInput : MonoBehaviour
{
    public SteamVR_Action_Boolean Ypressed;
    public SteamVR_Action_Boolean Xpressed;
    public SteamVR_Action_Boolean Spressed;
    public SteamVR_Action_Boolean LeftEast;
    public SteamVR_Action_Boolean LeftWest;
    public SteamVR_Action_Boolean RightEast;
    public SteamVR_Action_Boolean RightWest;

    public SteamVR_Input_Sources left;
    public SteamVR_Input_Sources right;

    public GodOfWashroomBehavior washroom;
    public MagicCheatSheetBehavior hint;
    public HideAndShowSkills hns;
    public TestPaperBehavior test;

    private bool show = false;

    void Start()
    {
        Ypressed.AddOnStateDownListener(TriggerDownY, left);
        Xpressed.AddOnStateDownListener(TriggerDownX, left);
        Spressed.AddOnStateUpListener(TriggerUpS, left);
        Spressed.AddOnStateDownListener(TriggerDownS, left);
        Spressed.AddOnStateUpListener(TriggerUpS, right);
        Spressed.AddOnStateDownListener(TriggerDownS, right);

        LeftEast.AddOnStateDownListener(TriggerDownR, left);
        LeftWest.AddOnStateDownListener(TriggerDownL, left);
        RightEast.AddOnStateDownListener(TriggerDownR, right);
        RightWest.AddOnStateDownListener(TriggerDownL, right);

    }
    public void TriggerDownY(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        washroom.GodOfWashroom();
    }
    public void TriggerDownX(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        hint.MagicCheatSheet();
    }
    public void TriggerUpS(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if(show) hns.Hide();
        show = false;
    }
    public void TriggerDownS(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if(!show) hns.Show();
        show = true;
    }
    public void TriggerDownL(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        test.previous();
    }
    public void TriggerDownR(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        test.next();
    }

}
