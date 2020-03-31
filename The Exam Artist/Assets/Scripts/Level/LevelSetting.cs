﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSetting : MonoBehaviour
{
    public bool start = false;
    public float offset;
    public float timeLeft;
    public string[] answer;
    public bool washroomed = false;
    public GetQuestion question;
    public MultipleChoiceBehavior[] quesTrack;
    public int[] scoreTrack;

    private void Start()
    {
        timeLeft = -1.0f;
        question = null;
        DontDestroyOnLoad(GameObject.Find("LevelSetting"));
    }

    private void Update()
    {
        if (offset > 0)
        {
            offset -= Time.deltaTime;
        }
        else
        {
            if (!start)
            {
                answer = GameObject.FindGameObjectWithTag("MainSelectHandler").GetComponent<TestPaperBehavior>().getAllAnswer();
                //setQuestion();
                start = true;
            }
        }
    }

    public void setTime()
    {
        timeLeft = GameObject.Find("Timer").GetComponent<Timer>().timeLeft;
    }

    public void setWashroom()
    {
        washroomed = true;
    }

    public void setQuestion()
    {
        TestPaperBehavior test = GameObject.FindGameObjectWithTag("MainSelectHandler").GetComponent<TestPaperBehavior>();
        question = test.setQuestion();
        quesTrack = test.setQuesTrack();
        scoreTrack = test.setScoreTrack();
    }

}
