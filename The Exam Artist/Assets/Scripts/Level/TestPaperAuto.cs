using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/************************************************************* 
Script to control student's test behavior:
Switches to next question with correct answer every X seconds.
Gets question from main test paper
*************************************************************/

public class TestPaperAuto : MonoBehaviour
{
    [Tooltip("Student test paper")]
    public GameObject testPaper;
    [Tooltip("Question on test paper")]
    public GameObject questionTextObj;
    [Tooltip("Choices on test paper")]
    public GameObject choiceA, choiceB, choiceC, choiceD;
    [Tooltip("Seconds to change question")]
    public float frequency = 0.0f;

    // Scripts
    private TimeFreezeBehavior tf;
    private TestPaperBehavior playerTest;
    private GetQuestion question;
    private MultipleChoiceBehavior[] quesTrack;

    private int tempQuestion = -1;              // Question counter
    private float timeChange = 0.0f;            // Time to change page
    private GameObject mainSelectHandler;       // Select Handler of player test paper
    private GameObject testPage, initialPage;   // Different pages
    private float offset;                       // Talking time
    private bool start = false;                 // Start test

    void Start()
    {
        tf = GameObject.Find("SkillsScript").GetComponent<TimeFreezeBehavior>();
        playerTest = GameObject.FindGameObjectWithTag("MainSelectHandler").GetComponent<TestPaperBehavior>();
        testPage = testPaper.transform.Find("TestPage").gameObject;
        initialPage = testPaper.transform.Find("InitialPage").gameObject;

        testPage.SetActive(false);
        initialPage.SetActive(true);
        offset = GameObject.Find("LevelSetting").GetComponent<LevelSetting>().offset;

        mainSelectHandler = GameObject.FindGameObjectWithTag("MainSelectHandler");
        timeChange = frequency;
    }

    void Update()
    {
        // If ready for test
        if (!playerTest.onPrepare)
        {
            // If not finish talking
            if (offset > 0)
            {
                offset -= Time.deltaTime;
            }
            else
            {
                // If haven't swith page
                if (!start)
                {
                    initialPage.SetActive(false);
                    testPage.SetActive(true);
                    if (question == null)
                    {
                        // Copy question for main test paper (IMPORTANT)
                        question = mainSelectHandler.GetComponent<TestPaperBehavior>().question.copy();
                        quesTrack = new MultipleChoiceBehavior[question.getQuesCount()];
                        next();
                    }
                    start = true;
                }
                // If not time freeze
                if (!tf.isExisting())
                {
                    timeChange -= Time.deltaTime;
                    if (timeChange <= 0)
                    {
                        timeChange = frequency;
                        next();
                    }
                }
            }
        }
    }

    // Function to next question
    public void next()
    {
        reset();
        // Set question counter
        if (tempQuestion < question.getQuesCount() - 1)
        {
            tempQuestion += 1;
        }
        else
        {
            tempQuestion = 0;
        }
        // Set question
        if (quesTrack[tempQuestion] == null)
        {
            quesTrack[tempQuestion] = new MultipleChoiceBehavior();
            JObject Q = question.getNextQuestion();
            quesTrack[tempQuestion].pushQuestion(Q);
        }
        GameObject[] choices = { choiceA, choiceB, choiceC, choiceD };
        quesTrack[tempQuestion].showQuestion(questionTextObj, choices, tempQuestion);
        int correctAns = quesTrack[tempQuestion].correctAns;
        quesTrack[tempQuestion].select(choices[correctAns], correctAns);
    }

    // Function to reset page choices
    public void reset()
    {
        Color white = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        Button A = choiceA.GetComponentInChildren<Button>();
        Button B = choiceB.GetComponentInChildren<Button>();
        Button C = choiceC.GetComponentInChildren<Button>();
        Button D = choiceD.GetComponentInChildren<Button>();
        ColorBlock cb = A.colors;
        cb.normalColor = white;
        A.colors = cb;
        B.colors = cb;
        C.colors = cb;
        D.colors = cb;
    }
}

