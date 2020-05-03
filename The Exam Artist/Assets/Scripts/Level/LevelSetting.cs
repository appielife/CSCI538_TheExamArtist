using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using Newtonsoft.Json.Linq;

public class LevelSetting : MonoBehaviour
{
    [Tooltip("This is used to store time after prepare and before test start")]
    public float offset;
    [Tooltip("Set time staying in washroom")]
    public float washroomDuration = 10.0f;
    [Tooltip("Set time for maximum freezing seconds")]
    public float maxFreezeTime = 10.0f;
    [Tooltip("Set number of questions")]
    public int numQuestion = 5;
    [Tooltip("Check to enable random seats")]
    public bool randomseats = false;
    [Tooltip("Check to enable prepare page")]
    public bool onPrepare = true;
    [Tooltip("Check to enable illegal detection")]
    public bool illegalDetect = true;

    [HideInInspector]
    public bool answerGrabbed = false; // Grabbed answer or not
    [HideInInspector]
    public float timeLeft = -1.0f, initialTime = 300.0f; // Remaining time and Test time
    [HideInInspector]
    public string[] answer; // Store all answers
    [HideInInspector]
    public GetQuestion question; // Store test questions
    [HideInInspector]
    public List<int> unansweredQues; // Store unanswered questions
    [HideInInspector]
    public Settings setting; // Store global setting
    [HideInInspector]
    public string subject; // Current sub
    [HideInInspector]
    public List<Sprite> bribeList = new List<Sprite>(); // Store bribe list
    [HideInInspector]
    public GameObject projectile; // Know which projectile

    private void Awake()
    {
        initialTime = numQuestion * 60;
    }

    private void Start()
    {
        timeLeft = -1.0f;
        question = null;
        if (GameObject.Find("Settings"))
        {
            setting = GameObject.Find("Settings").GetComponent<Settings>();
        }
        projectile = GameObject.FindGameObjectWithTag("Projectile");
    }

    private void Update()
    {
        if (offset > 0)
        {
            offset -= Time.deltaTime;
        }
        else
        {
            if (!answerGrabbed)
            {
                answer = GameObject.FindGameObjectWithTag("MainSelectHandler").GetComponent<TestPaperBehavior>().getAllAnswer();
                answerGrabbed = true;
            }
        }
    }

    public void setTime()
    {
        timeLeft = GameObject.Find("Timer").GetComponent<Timer>().timeLeft;
    }

    public void setQuestion()
    {
        TestPaperBehavior test = GameObject.FindGameObjectWithTag("MainSelectHandler").GetComponent<TestPaperBehavior>();
        question = test.getQuestion();
        /*quesTrack = test.setQuesTrack();
        scoreTrack = test.setScoreTrack();*/
        unansweredQues = test.getUnansweredQues();
    }

    public void setSubject(string s)
    {
        subject = s;
        GameObject.Find("Subject").GetComponent<Text>().text = s.ToUpper() + " TEST !";
        GameObject[] test = GameObject.FindGameObjectsWithTag("Subject");
        for(int i = 0; i< test.Length; i++)
        {
            test[i].GetComponent<Text>().text = s;
        }
    }

    public void setOnPrepare(bool b)
    {
        onPrepare = b;
    }
}
