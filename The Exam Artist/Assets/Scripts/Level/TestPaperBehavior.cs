﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;
using Valve.VR;

public class TestPaperBehavior : MonoBehaviour
{
    public GameObject testPage;
    public GameObject submitPage;
    public GameObject questionTextObj;
    public GameObject choiceA;
    public GameObject choiceB;
    public GameObject choiceC;
    public GameObject choiceD;
    public GetQuestion question = new GetQuestion();
    public string JSON_file;
    private ScoreCalculate calculateScore;
    private int tempQuestion = -1;
    private MultipleChoiceBehavior[] quesTrack;
    private int[] scoreTrack;
    public int total_score = 0;
    // Start is called before the first frame update
    void Start()
    {
        testPage.SetActive(true);
        submitPage.SetActive(false);
        question.readQuestionFromJson(JSON_file);
        //Debug.Log(question.ques);
        quesTrack = new MultipleChoiceBehavior[question.getQuesCount()];
        scoreTrack = new int[question.getQuesCount()];
        for (int i = 0; i < question.getQuesCount(); i++)
        {
            scoreTrack[i] = 0;
        }
        next();
    }

    public int getCurrentQuesNum()
    {
        return tempQuestion;
    }

    public int getCurrentQuesId()
    {
        return int.Parse(question.getQuestionId(tempQuestion));
    }

    public int getCurrentQuesAns()
    {
        return quesTrack[tempQuestion].correctAns;
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
        //Debug.Log(tempQuestion);
        if (quesTrack[tempQuestion] == null)
        {
            quesTrack[tempQuestion] = new MultipleChoiceBehavior();
            JObject Q = question.getNextQuestion();
            //Debug.Log(Q);
            quesTrack[tempQuestion].pushQuestion(Q);
        }
        question.updateQuesNum(tempQuestion);
        GameObject[] choices = { choiceA, choiceB, choiceC, choiceD };
        quesTrack[tempQuestion].showQuestion(questionTextObj, choices, tempQuestion);
    }

    public void previous()
    {
        if (tempQuestion != -1)
        {
            if (quesTrack[tempQuestion].isCorrect != scoreTrack[tempQuestion])
            {
                if (scoreTrack[tempQuestion] == 0) total_score += 1;
                else total_score -= 1;
                scoreTrack[tempQuestion] = quesTrack[tempQuestion].isCorrect;
            }
        }

        reset();

        if (tempQuestion > 0)
        {
            tempQuestion -= 1;
        }
        else
        {
            tempQuestion = question.getQuesCount() - 1;
        }
        if (quesTrack[tempQuestion] == null)
        {
            quesTrack[tempQuestion] = new MultipleChoiceBehavior();
            JObject Q = question.getPrevQuestion();
            quesTrack[tempQuestion].pushQuestion(Q);
        }
        question.updateQuesNum(tempQuestion);
        GameObject[] choices = { choiceA, choiceB, choiceC, choiceD };
        quesTrack[tempQuestion].showQuestion(questionTextObj, choices, tempQuestion);
    }

    public void submit()
    {
        Button submit = testPage.GetComponentsInChildren<Button>()[6];
        ColorBlock cb = submit.colors;
        cb.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        submit.colors = cb;

        testPage.SetActive(false);
        submitPage.SetActive(true);
    }

    public void backToTest()
    {
        Button no = submitPage.GetComponentsInChildren<Button>()[1];
        ColorBlock cb = no.colors;
        cb.normalColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        no.colors = cb;

        submitPage.SetActive(false);
        testPage.SetActive(true);
    }

    public void writeAnsToJson()
    {
        char[] abcd = { 'A', 'B', 'C', 'D' };
        using (StreamWriter file = File.CreateText(@Application.dataPath + "/GameData/answers.json"))

        using (JsonWriter writer = new JsonTextWriter(file))
        {
            writer.Formatting = Formatting.Indented;

            writer.WriteStartObject();
            writer.WritePropertyName("Answers");
            writer.WriteStartArray();
            for (int i = 0; i < question.getQuesCount(); i++)
            {
                writer.WriteStartObject();
                writer.WritePropertyName("id");
                writer.WriteValue(question.getQuestionId(i));
                writer.WritePropertyName("YourAns");
                //Debug.Log(i);
                //Debug.Log(quesTrack.Length);
                if (quesTrack[i] == null || quesTrack[i].choice == -1) writer.WriteValue("NA");
                else writer.WriteValue(abcd[quesTrack[i].choice]);
                writer.WritePropertyName("MyAns");
                //Debug.Log(question.getQuestionCorrectAns(i));
                writer.WriteValue(question.getQuestionCorrectAns(i));
                writer.WriteEndObject();
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }
        FadeIn();
        Invoke("FadeOut", 2.0f);
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
   
   // fucntion to calulate all the scores
    public void showScore()
    {
        scoreObject a = calculateScore.getScore();
        Debug.Log(a.unans_count);
        Debug.Log(a.correct_ans);
        Debug.Log(a.total_count);       
    }

    private void FadeIn()
    {
        SteamVR_Fade.Start(Color.clear, 0.0f);
        SteamVR_Fade.Start(Color.black, 2.0f);
    }
    private void FadeOut()
    {
        SteamVR_Fade.Start(Color.black, 0.0f);
        SteamVR_Fade.Start(Color.clear, 2.0f);
        SceneManager.LoadScene(2);
    }
}
