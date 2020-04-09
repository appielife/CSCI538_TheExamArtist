using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSetting : MonoBehaviour
{
    public bool start = false;
    public float offset;
    public float timeLeft = -1.0f, initialTime = 300.0f;
    public string[] answer;
    public bool washroomed = false;
    public GetQuestion question;
    public MultipleChoiceBehavior[] quesTrack;
    public int[] scoreTrack;
    public Settings setting;
    public string subject;
    public int numQuestion = 5;

    private void Start()
    {
        timeLeft = -1.0f;
        question = null;
        DontDestroyOnLoad(GameObject.Find("LevelSetting"));
        if (GameObject.Find("Settings"))
        {
            setting = GameObject.Find("Settings").GetComponent<Settings>();
        }
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

    public void resetTemp()
    {
        Debug.Log("reset");
        question = null;
        quesTrack = null;
        scoreTrack = null;
        washroomed = false;
        timeLeft = -1.0f;
        offset = 15.0f;
    }

    public void setSubject(string s)
    {
        subject = s;
        GameObject.Find("Subject").GetComponent<Text>().text = s.ToUpper() + " QUIZ !";
        GameObject[] test = GameObject.FindGameObjectsWithTag("Subject");
        for(int i = 0; i< test.Length; i++)
        {
            test[i].GetComponent<Text>().text = s;
        }
    }

}
