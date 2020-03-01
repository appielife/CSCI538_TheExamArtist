﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

[System.Serializable]
public class MultipleChoiceBehavior
{
    //public getQuestions question;
    private string questionText;
    private string[] optionText = new string[4];
    private string[] trueOrFalse = new string[4];
    public int correctAns = -1;
    public int isCorrect = 0;
    public int choice = -1;
    public string quesId = "";

    public void pushQuestion(JObject Q)
    {
        //Debug.Log(question.current);
        //JObject Q = question.getNextQuestion();
        questionText = (string)Q["question_txt"];
        JArray options = (JArray)Q["options"];
        quesId = (string)Q["id"];
        for (int i = 0; i < 4; i++)
        {
            optionText[i] = (string)((JObject)options[i])["option_txt"];
            trueOrFalse[i] = (string)((JObject)options[i])["isCorrect"];
            if (trueOrFalse[i] == "True") correctAns = i;
        }
    }

    public void showQuestion(GameObject questionTextObj, GameObject[] choices, int questionNum)
    {
        //questionTextObj.SetActive(false);
        TextMesh qText = questionTextObj.GetComponent<TextMesh>();
        
        qText.text = (questionNum+1).ToString() + ". " + questionText;
        for (int i = 0; i < 4; i++)
        {
            TextMesh Text = choices[i].GetComponentInChildren<TextMesh>();
            Text.text = optionText[i];
        }
        if (choice > -1)
        {
            Color selectColor = new Color(0.8f, 0.9f, 1.0f, 1.0f);
            changeColor(choices[choice], selectColor);
        }
    }

    /*public void reset()
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
    }*/

    void changeColor(GameObject Obj, Color c)
    {
        Button A = Obj.GetComponentInChildren<Button>();
        ColorBlock cb = A.colors;
        cb.normalColor = c;
        A.colors = cb;
    }
    public void select(GameObject choiceObj, int idx)
    {
        //reset();
        Color c = new Color(0.8f, 0.9f, 1.0f, 1.0f);
        changeColor(choiceObj, c);
        choice = idx;
        if (trueOrFalse[idx] == "True")
        {
            isCorrect = 1;
        }
        else
        {
            isCorrect = 0;
        }
    }
    /*public void selectB()
    {
        reset();
        Button B = choiceB.GetComponentInChildren<Button>();
        ColorBlock cb = B.colors;
        cb.normalColor = new Color(0.8f, 0.9f, 1.0f, 1.0f);
        B.colors = cb;
        choice = 1;
        if (trueOrFalse[1] == "True")
        {
            isCorrect = 1;
        }
        else
        {
            isCorrect = 0;
        }
    }
    public void selectC()
    {
        reset();
        Button C = choiceC.GetComponentInChildren<Button>();
        ColorBlock cb = C.colors;
        cb.normalColor = new Color(0.8f, 0.9f, 1.0f, 1.0f);
        C.colors = cb;
        choice = 2;
        if (trueOrFalse[2] == "True")
        {
            isCorrect = 1;
        }
        else
        {
            isCorrect = 0;
        }
    }
    public void selectD()
    {
        reset();
        Button D = choiceD.GetComponentInChildren<Button>();
        ColorBlock cb = D.colors;
        cb.normalColor = new Color(0.8f, 0.9f, 1.0f, 1.0f);
        D.colors = cb;
        choice = 3;
        if (trueOrFalse[3] == "True")
        {
            isCorrect = 1;
        }
        else
        {
            isCorrect = 0;
        }
    }*/
}
