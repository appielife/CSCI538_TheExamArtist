using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class TestPaperAuto : MonoBehaviour
{
    private GameObject testPage;
    public GameObject questionTextObj;
    public GameObject choiceA;
    public GameObject choiceB;
    public GameObject choiceC;
    public GameObject choiceD;
    private getQuestions question;
    private int tempQuestion = -1;
    private MultipleChoiceBehavior[] quesTrack;
    // Start is called before the first frame update

    public float frequency = 0;
    private float startTime = 0;
    private GameObject mainTestPaper;

    void Start()
    {
        mainTestPaper = GameObject.FindGameObjectWithTag("MainTestPaper");
       
        startTime = Time.time;
        
    }

    void Update()
    {
        if (question == null)
        {
            question = mainTestPaper.GetComponentInChildren<TestPaperBehavior>().question.copy();
            /*question.readQuestionFromJson();*/
            quesTrack = new MultipleChoiceBehavior[question.getQuesCount()];
            next();
        }
        float t = Time.time - startTime;
        if (t > frequency)
        {
            startTime = Time.time;
            next();
        }
    }

    public void next()
    {
        reset();
        if (tempQuestion < question.getQuesCount() - 1)
        {
            tempQuestion += 1;
        }
        else
        {
            tempQuestion = 0;
        }
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

    public void reset()
    {
        Button A = choiceA.GetComponentInChildren<Button>();
        Button B = choiceB.GetComponentInChildren<Button>();
        Button C = choiceC.GetComponentInChildren<Button>();
        Button D = choiceD.GetComponentInChildren<Button>();
        ColorBlock cb = A.colors;
        cb.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        A.colors = cb;
        B.colors = cb;
        C.colors = cb;
        D.colors = cb;
    }

    public void selectA()
    {
        reset();
        quesTrack[tempQuestion].select(choiceA, 0);
    }
    public void selectB()
    {
        reset();
        quesTrack[tempQuestion].select(choiceB, 1);
    }
    public void selectC()
    {
        reset();
        quesTrack[tempQuestion].select(choiceC, 2);
    }
    public void selectD()
    {
        reset();
        quesTrack[tempQuestion].select(choiceD, 3);
    }
}

