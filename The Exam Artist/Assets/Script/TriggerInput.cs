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
        /*MagicCheatSheetBehavior MCS = GameObject.Find("SkillScript").GetComponent<MagicCheatSheetBehavior>();
        GodOfWashroomBehavior GOW = GameObject.Find("SkillScript").GetComponent<GodOfWashroomBehavior>();
        MCS.imgCoolDown = GameObject.FindGameObjectsWithTag("skill")[0].GetComponentsInChildren<Image>()[0];
        MCS.imgExist = GameObject.FindGameObjectsWithTag("skill")[0].GetComponentsInChildren<Image>()[1];
        MCS.textCoolDown = GameObject.FindGameObjectsWithTag("skill")[0].GetComponentInChildren<Text>();
        GOW.imgCoolDown = GameObject.FindGameObjectsWithTag("skill")[1].GetComponentInChildren<Image>();
        GOW.textCoolDown = GameObject.FindGameObjectsWithTag("skill")[1].GetComponentInChildren<Text>();*/
        GameObject Playerer = GameObject.FindGameObjectWithTag("Player");
        IllegalMoveHandler illegal = Playerer.AddComponent<IllegalMoveHandler>();
        illegal.playerCam = GameObject.Find("VRCamera");
        illegal.wow = (AudioClip)Resources.Load("tindeck_1");
        //illegal.debugText = GameObject.FindGameObjectWithTag("IllegalText").GetComponent<Text>();

        MagicCheatSheetBehavior mcsb = Playerer.GetComponentInChildren<MagicCheatSheetBehavior>();

        mcsb.enabled = true;

        mcsb.testPaper = GameObject.Find("SelectHandler");
        mcsb.loadHint();

        mcsb.hintObj = GameObject.Find("Hints");

        Playerer.GetComponentInChildren<GodOfWashroomBehavior>().enabled = true;

        hns = GameObject.Find("SkillsScript").GetComponent<HideAndShowSkills>();
        hint = GameObject.Find("SkillsScript").GetComponent<MagicCheatSheetBehavior>();
        washroom = GameObject.Find("SkillsScript").GetComponent<GodOfWashroomBehavior>();




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
