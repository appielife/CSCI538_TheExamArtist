using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/************************
Script for timer control
************************/

public class Timer : MonoBehaviour
{
    [HideInInspector]
    public float timeLeft; // Time remaining

    private bool timesUp = false, onPrepare = false;
    private float offset;
    private Text timerText;
    private TestPaperBehavior test;
    private LevelSetting setting;
    private TimeFreezeBehavior tf;

    void Start()
    {
        timerText = gameObject.GetComponentInChildren<Text>();

        tf = GameObject.Find("SkillsScript").GetComponent<TimeFreezeBehavior>();
        test = GameObject.FindGameObjectWithTag("MainSelectHandler").gameObject.GetComponent<TestPaperBehavior>();
        setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();

        offset = setting.offset;
        onPrepare = setting.onPrepare;
        if (setting.timeLeft > 0)
        {
            timeLeft = setting.timeLeft;
        }
        else
        {
            timeLeft = setting.initialTime;
        }
        timeLeft += 1;
    }

    void Update()
    {
        // If ready for test
        if (!onPrepare)
        {
            // If still talking
            if (offset > 0)
            {
                offset -= Time.deltaTime;
            }
            else
            {
                // Update text
                if (timeLeft > 0)
                {
                    if (!tf.isExisting())
                    {
                        timeLeft -= Time.deltaTime;
                        string minutes = ((int)timeLeft / 60).ToString();
                        string seconds = ((int)timeLeft % 60).ToString();
                        if ((int)timeLeft / 60 < 10) { minutes = "0" + minutes; }
                        if ((int)timeLeft % 60 < 10) { seconds = "0" + seconds; }
                        timerText.text = minutes + ":" + seconds;
                    }
                }
                else if (timeLeft < 0 && !timesUp)
                {
                    // If time up, test end.
                    test.writeAnsToJson();
                    timesUp = true;
                }
            }
        }
        else
        {
            // Keep track of onPrepare
            onPrepare = setting.onPrepare;
        }
    }
}
