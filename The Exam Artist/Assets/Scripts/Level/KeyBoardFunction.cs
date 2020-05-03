using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

/****************************** 
Script to play with keyboard 
(Non-VR fallback mode)
USE THIS TO DEBUG WITHOUT VR
******************************/

public class KeyBoardFunction : MonoBehaviour
{
    [Tooltip("Washroom Object (w/ Washroom.cs)")]
    public Washroom wash;
    [Tooltip("Meditation Object (w/ MeditationHandler.cs)")]
    public MeditationHandler mh;

    private float offset;           // Time after prepare and before test start
    private bool gameover = false, freeze = false;
    private float holdTime = 2.0f;  // Hold time to activate freeze skill
    // Skill scripts
    private HideAndShowSkills hns;
    private LevelSetting setting;
    private TeacherController tc;
    private TimeFreezeBehavior tf;

    private void Start()
    {
        hns = GameObject.Find("SkillsScript").GetComponent<HideAndShowSkills>();
        setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
        offset = setting.offset;

        tc = GameObject.FindGameObjectWithTag("TeacherAction").GetComponent<TeacherController>();
        tf = GameObject.Find("SkillsScript").GetComponent<TimeFreezeBehavior>();
    }

    private void Update()
    {
        if (!SteamVR.active && !setting.onPrepare)
        {
            if (offset > 0)
            {
                offset -= Time.deltaTime;
            }
            else
            {
                if (!gameover)
                {
                    // Check gameover or not
                    gameover = tc.gameover;
                    // Space for hide/show skill canvas
                    if (Input.GetKeyDown(KeyCode.Space))
                    {
                        if (!wash.inWashroom() && !mh.inMeditation())
                        {
                            hns.Show();
                        }
                    }
                    else if (Input.GetKeyUp(KeyCode.Space))
                    {
                        if (!wash.inWashroom() && !mh.inMeditation())
                        {
                            hns.Hide();
                        }
                    }
                    // F hold for time freeze
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        if (!wash.inWashroom() && !mh.inMeditation())
                        {
                            freeze = true;
                        }
                    }
                    else if (Input.GetKeyUp(KeyCode.F))
                    {
                        if (!wash.inWashroom() && !mh.inMeditation())
                        {
                            freeze = false;
                            tf.hold = false;
                        }
                    }
                    if (freeze)
                    {
                        if (holdTime > 0)
                        {
                            holdTime -= Time.deltaTime;
                        }
                        else
                        {
                            tf.hold = true;
                        }
                    }
                    else
                    {
                        holdTime = 2.0f;
                    }
                }
            }
        }
    }

    // Function to know to activate freeze skill after hold time
    public void freezeOn()
    {
        freeze = true;
    }


    // Function to deactivate freeze skill
    public void freezeOff()
    {
        freeze = false;
        tf.hold = false;
    }
}
