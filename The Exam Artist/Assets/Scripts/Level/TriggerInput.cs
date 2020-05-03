using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

/****************************************************
Script for handling Trigger actions in Level scene(s)
****************************************************/

public class TriggerInput : MonoBehaviour
{
    // Bind action
    [Tooltip("Y Button Pressed")]
    public SteamVR_Action_Boolean Ypressed;
    [Tooltip("X Button Pressed")]
    public SteamVR_Action_Boolean Xpressed;
    [Tooltip("Button Touched")]
    public SteamVR_Action_Boolean Spressed;
    [Tooltip("A Button Pressed")]
    public SteamVR_Action_Boolean Apressed;
    [Tooltip("B Button Pressed")]
    public SteamVR_Action_Boolean Bpressed;
    [Tooltip("Grip Pressed")]
    public SteamVR_Action_Boolean Gpressed;
    [Tooltip("Right on Left Pad")]
    public SteamVR_Action_Boolean LeftEast;
    [Tooltip("Left on Left Pad")]
    public SteamVR_Action_Boolean LeftWest;
    [Tooltip("Right on Right Pad")]
    public SteamVR_Action_Boolean RightEast;
    [Tooltip("Left on Right Pad")]
    public SteamVR_Action_Boolean RightWest;

    // Set input source
    [Tooltip("Left hand")]
    public SteamVR_Input_Sources left;
    [Tooltip("Right hand")]
    public SteamVR_Input_Sources right;

    [Tooltip("Washroom Object")]
    public Washroom wash;
    [Tooltip("Meditation Object")]
    public MeditationHandler mh;

    // Skills scipts
    private GodOfWashroomBehavior washroom;
    private MagicCheatSheetBehavior hint;
    private HideAndShowSkills hns;
    private TestPaperBehavior test;
    private GiftBlindEyesBehavior gbe;
    private MeditationBehavior mb;
    private TeacherController tc;
    private TimeFreezeBehavior tf;

    private bool onPrepare = true, gameover = false, freeze = false;
    private float offset, holdTime = 2.0f;
    private List<Sprite> bribeList = new List<Sprite>();

    // IMPORTANT to destroy binded settings when scene change
    // Only if same setting for all scenes
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
        Gpressed.RemoveOnStateDownListener(TriggerDownG, left);
        Gpressed.RemoveOnStateUpListener(TriggerUpG, left);
        Gpressed.RemoveOnStateDownListener(TriggerDownG, right);
        Gpressed.RemoveOnStateUpListener(TriggerUpG, right);

        LeftEast.RemoveOnStateDownListener(TriggerDownR, left);
        LeftWest.RemoveOnStateDownListener(TriggerDownL, left);
        RightEast.RemoveOnStateDownListener(TriggerDownR, right);
        RightWest.RemoveOnStateDownListener(TriggerDownL, right);
    }

    void Start()
    {
        // Set skill scipts
        washroom = GameObject.Find("SkillsScript").GetComponent<GodOfWashroomBehavior>();
        hint = GameObject.Find("SkillsScript").GetComponent<MagicCheatSheetBehavior>();
        hns = GameObject.Find("SkillsScript").GetComponent<HideAndShowSkills>();
        gbe = GameObject.Find("SkillsScript").GetComponent<GiftBlindEyesBehavior>();
        mb = GameObject.Find("SkillsScript").GetComponent<MeditationBehavior>();
        test = GameObject.FindGameObjectWithTag("MainSelectHandler").gameObject.GetComponent<TestPaperBehavior>();
        tc = GameObject.FindGameObjectWithTag("TeacherAction").GetComponent<TeacherController>();
        tf = GameObject.Find("SkillsScript").GetComponent<TimeFreezeBehavior>();

        // Set parameters
        offset = GameObject.Find("LevelSetting").GetComponent<LevelSetting>().offset;
        onPrepare = GameObject.Find("LevelSetting").GetComponent<LevelSetting>().onPrepare;
        bribeList = GameObject.Find("LevelSetting").GetComponent<LevelSetting>().bribeList;

        // Bind funtions to actions
        Ypressed.AddOnStateDownListener(TriggerDownY, left);
        Xpressed.AddOnStateDownListener(TriggerDownX, left);
        Spressed.AddOnStateUpListener(TriggerUpS, left);
        Spressed.AddOnStateDownListener(TriggerDownS, left);
        Spressed.AddOnStateUpListener(TriggerUpS, right);
        Spressed.AddOnStateDownListener(TriggerDownS, right);
        Apressed.AddOnStateDownListener(TriggerDownA, right);
        Bpressed.AddOnStateDownListener(TriggerDownB, right);
        Gpressed.AddOnStateDownListener(TriggerDownG, left);
        Gpressed.AddOnStateUpListener(TriggerUpG, left);
        Gpressed.AddOnStateDownListener(TriggerDownG, right);
        Gpressed.AddOnStateUpListener(TriggerUpG, right);

        LeftEast.AddOnStateDownListener(TriggerDownR, left);
        LeftWest.AddOnStateDownListener(TriggerDownL, left);
        RightEast.AddOnStateDownListener(TriggerDownR, right);
        RightWest.AddOnStateDownListener(TriggerDownL, right);
    }

    void Update()
    {
        // If preparing, keep track
        if (onPrepare)
        {
            onPrepare = GameObject.Find("LevelSetting").GetComponent<LevelSetting>().onPrepare;
        }
        else
        {
            // If still talking
            if (offset >= 0)
            {
                offset -= Time.deltaTime;
            }
            // If not gameover, keep track
            if (!gameover)
            {
                gameover = tc.gameover;
            }
            else
            {
                hns.Hide();
            }
            // If grip hold
            if (freeze)
            {
                if (holdTime > 0)
                {
                    // If hold for < 2 seconds
                    holdTime -= Time.deltaTime;
                }
                else
                {
                    // If hold for 2 seconds
                    tf.hold = true;
                }
            }
            else
            {
                // If grip released
                holdTime = 2.0f;
            }
        }
    }

    // Function for Y button pressed
    public void TriggerDownY(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (offset < 0 && !gameover)
        {
            if (!wash.inWashroom() && !mh.inMeditation() && !freeze)
            {
                if (test.isBribeSkillActive())
                {
                    if (bribeList.Count >= 1)
                    {
                        GameObject option = GameObject.Find("BribeSkillPage").transform.Find("Opt1").gameObject;
                        GameObject image = option.transform.Find("Canvas").transform.Find("Image").gameObject;
                        test.ChooseBribee(image);
                    }
                }
                else
                {
                    hint.MagicCheatSheet();
                }
            }
        }
    }

    // Function for X button pressed
    public void TriggerDownX(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (offset < 0 && !gameover)
        {
            if(!wash.inWashroom() && !mh.inMeditation() && !freeze)
            {
                if (test.isBribeSkillActive())
                {
                    if (bribeList.Count >= 2)
                    {
                        GameObject option = GameObject.Find("BribeSkillPage").transform.Find("Opt2").gameObject;
                        GameObject image = option.transform.Find("Canvas").transform.Find("Image").gameObject;
                        test.ChooseBribee(image);
                    }
                }
                else
                {
                     washroom.GodOfWashroom();
                }
            }
        }
    }

    // Function for buttons untouched
    public void TriggerUpS(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (offset < 0 && !gameover)
        {
            if (!wash.inWashroom() && !mh.inMeditation())
            {
                hns.Hide();
            }
        }
    }

    // Function for buttons tounched
    public void TriggerDownS(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (offset < 0 && !gameover)
        {
            if (!wash.inWashroom() && !mh.inMeditation())
            {
                hns.Show();
            }
        }
    }

    // Function for left on pad
    public void TriggerDownL(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (test.isBribeActive())
        {
            test.bribePagePrev();
        }
        if (offset < 0 && !gameover)
        {
            if (!wash.inWashroom() && !mh.inMeditation())
            {
                test.previous();
            }
        }
    }

    // Function for right on pad
    public void TriggerDownR(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (test.isBribeActive())
        {
            test.bribePageNext();
        }
        if (offset < 0 && !gameover)
        {
            if (!wash.inWashroom() && !mh.inMeditation())
            {
                test.next();
            }
        }
    }

    // Function for A button pressed
    public void TriggerDownA(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (offset < 0 && !gameover)
        {
            if (!wash.inWashroom() && !mh.inMeditation() && !freeze)
            {
                if (test.isBribeSkillActive())
                {
                    if (bribeList.Count >= 3)
                    {
                        GameObject option = GameObject.Find("BribeSkillPage").transform.Find("Opt3").gameObject;
                        GameObject image = option.transform.Find("Canvas").transform.Find("Image").gameObject;
                        test.ChooseBribee(image);
                    }
                }
                else
                {
                    mb.Meditation();
                }
            }
        }
    }

    // Function for B button pressed
    public void TriggerDownB(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (offset < 0 && !gameover)
        {
            if (!wash.inWashroom() && !mh.inMeditation() && !freeze)
            {
                if (test.isBribeSkillActive())
                {
                    test.backToTest();
                }
                else
                {
                    test.showBribeSkillPage();
                }
            }
        }
    }

    // Function for grip pressed
    public void TriggerDownG(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (offset < 0 && !gameover)
        {
            if (!wash.inWashroom() && !mh.inMeditation())
            {
                freeze = true;
                //tf.hold = true;
            }
        }
    }

    // Function for grip released
    public void TriggerUpG(SteamVR_Action_Boolean fromAction, SteamVR_Input_Sources fromSource)
    {
        if (offset < 0 && !gameover)
        {
            if (!wash.inWashroom() && !mh.inMeditation())
            {
                freeze = false;
                tf.hold = false;
            }
        }
    }
}
