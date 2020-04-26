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
    public SteamVR_Action_Boolean Apressed;
    public SteamVR_Action_Boolean Bpressed;
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
    public GiftBlindEyesBehavior gbe;
    public MeditationBehavior mb;
    public TeacherController tc;

    private bool show = false, onPrepare = true, gameover = false;
    public float offset;

    void OnDestroy()
    {
        Ypressed.RemoveOnStateDownListener(TriggerDownY, left);
        Xpressed.RemoveOnStateDownListener(TriggerDownX, left);
        Spressed.RemoveOnStateUpListener(TriggerUpS, left);
        Spressed.RemoveOnStateDownListener(TriggerDownS, left);
        Spressed.RemoveOnStateUpListener(TriggerUpS, right);
        Spressed.RemoveOnStateDownListener(TriggerDownS, right);
        Apressed.RemoveOnStateDownListener(TriggerDownA, right);
        Bpressed.RemoveOnStateDownListener(TriggerDownB, right);

        LeftEast.RemoveOnStateDownListener(TriggerDownR, left);
        LeftWest.RemoveOnStateDownListener(TriggerDownL, left);
        RightEast.RemoveOnStateDownListener(TriggerDownR, right);
        RightWest.RemoveOnStateDownListener(TriggerDownL, right);
    }

    void Start()
    {
        washroom = GameObject.Find("SkillsScript").GetComponent<GodOfWashroomBehavior>();
        hint = GameObject.Find("SkillsScript").GetComponent<MagicCheatSheetBehavior>();
        hns = GameObject.Find("SkillsScript").GetComponent<HideAndShowSkills>();
        gbe = GameObject.Find("SkillsScript").GetComponent<GiftBlindEyesBehavior>();
        mb = GameObject.Find("SkillsScript").GetComponent<MeditationBehavior>();

        test = GameObject.FindGameObjectWithTag("MainSelectHandler").gameObject.GetComponent<TestPaperBehavior>();

        offset = GameObject.Find("LevelSetting").GetComponent<LevelSetting>().offset;
        onPrepare = GameObject.Find("LevelSetting").GetComponent<LevelSetting>().onPrepare;

        tc = GameObject.FindGameObjectWithTag("TeacherAction").GetComponent<TeacherController>();

        Ypressed.AddOnStateDownListener(TriggerDownY, left);
        Xpressed.AddOnStateDownListener(TriggerDownX, left);
        Spressed.AddOnStateUpListener(TriggerUpS, left);
        Spressed.AddOnStateDownListener(TriggerDownS, left);
        Spressed.AddOnStateUpListener(TriggerUpS, right);
        Spressed.AddOnStateDownListener(TriggerDownS, right);
        Apressed.AddOnStateDownListener(TriggerDownA, right);
        Bpressed.AddOnStateDownListener(TriggerDownB, right);

        LeftEast.AddOnStateDownListener(TriggerDownR, left);
        LeftWest.AddOnStateDownListener(TriggerDownL, left);
        RightEast.AddOnStateDownListener(TriggerDownR, right);
        RightWest.AddOnStateDownListener(TriggerDownL, right);
    }

    void Update()
    {
        if (onPrepare)
        {
            onPrepare = GameObject.Find("LevelSetting").GetComponent<LevelSetting>().onPrepare;
        }
        else
        {
            if (offset >= 0)
            {
                offset -= Time.deltaTime;
            }
            if (!gameover)
            {
                gameover = tc.gameover;
            }
        }
    }

    public void TriggerDownY(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (offset < 0 && !gameover)
        {
            if (test.isBribeSkillActive())
            {
                GameObject option = GameObject.Find("BribeSkillPage").transform.Find("Opt1").gameObject;
                GameObject image = option.transform.Find("Canvas").transform.Find("Image").gameObject;
                test.ChooseBribee(image);
            }
            else
            {
                hint.MagicCheatSheet();
            }
        }
    }
    public void TriggerDownX(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (offset < 0 && !gameover)
        {
            if (test.isBribeSkillActive())
            {
                GameObject option = GameObject.Find("BribeSkillPage").transform.Find("Opt2").gameObject;
                GameObject image = option.transform.Find("Canvas").transform.Find("Image").gameObject;
                test.ChooseBribee(image);
            }
            else
            {
                if (!washroom.isTrigger())
                {
                    washroom.GodOfWashroom();
                }
            }
        }
    }
    public void TriggerUpS(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (offset < 0 && !gameover)
        {
            if (show) hns.Hide();
            show = false;
        }
    }
    public void TriggerDownS(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (offset < 0 && !gameover)
        {
            if (!show) hns.Show();
            show = true;
        }
    }
    public void TriggerDownL(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (test.isBribeActive())
        {
            test.bribePagePrev();
        }
        if (offset < 0 && !gameover)
        {
            test.previous();
        }
    }
    public void TriggerDownR(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (test.isBribeActive())
        {
            test.bribePageNext();
        }
        if (offset < 0 && !gameover)
        {
            test.next();
        }
    }

    public void TriggerDownA(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (offset < 0 && !gameover)
        {
            if (test.isBribeSkillActive())
            {
                GameObject option = GameObject.Find("BribeSkillPage").transform.Find("Opt3").gameObject;
                GameObject image = option.transform.Find("Canvas").transform.Find("Image").gameObject;
                test.ChooseBribee(image);
            }
            else
            {
                if (!mb.isTrigger())
                {
                    mb.Meditation();
                }
            }
        }
    }

    public void TriggerDownB(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (offset < 0 && !gameover)
        {
            test.showBribeSkillPage();
        }
    }

}
