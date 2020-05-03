using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/***********************************************
Script to control EACH multiple choice behavior
***********************************************/

[System.Serializable]
public class MultipleChoiceBehavior
{
    public int correctAns = -1;                     // Correct answer
    public int isCorrect = 0;                       // Is current selected choice correct
    public int choice = -1;                         // Current selected choice
    public string choiceContent = "";               // Content of choice
    public string quesId = "";                      // Question ID of current question

    private string questionText;                    // Question text
    private string[] optionText = new string[4];    // Option text
    private string[] trueOrFalse = new string[4];   // Correct answer form file

    // Function to store question
    public void pushQuestion(JObject Q)
    {
        questionText = (string)Q["question_txt"];
        JArray options = (JArray)Q["options"];
        quesId = (string)Q["id"];
        for (int i = 0; i < 4; i++)
        {
            optionText[i] = (string)((JObject)options[i])["option_txt"];
            trueOrFalse[i] = (string)((JObject)options[i])["isCorrect"];
            if (trueOrFalse[i] == "True")
            {
                correctAns = i;
            }
        }
    }

    // Function to show question
    public void showQuestion(GameObject questionTextObj, GameObject[] choices, int questionNum)
    {
        // Set question text
        Text qText = questionTextObj.GetComponentInChildren<Text>();
        qText.text = (questionNum+1).ToString() + ". " + questionText;
        for (int i = 0; i < 4; i++)
        {
            // Set option text
            GameObject current = choices[i].transform.Find("choice").gameObject;
            Text txt= current.GetComponentInChildren<Text>();
            txt.text = optionText[i];
        }
        if (choice > -1)
        {
            // If selected, change color
            Color selectColor = new Color(0.8f, 0.9f, 1.0f, 1.0f);
            changeColor(choices[choice], selectColor);
        }
    }

    // Function to change button color
    void changeColor(GameObject Obj, Color c)
    {
        Button A = Obj.GetComponentInChildren<Button>();
        ColorBlock cb = A.colors;
        cb.normalColor = c;
        A.colors = cb;
    }

    // Function to select choice
    public void select(GameObject choiceObj, int idx)
    {
        Color c = new Color(0.8f, 0.9f, 1.0f, 1.0f);
        changeColor(choiceObj, c);
        choice = idx;
        choiceContent = optionText[choice];
        if (trueOrFalse[idx] == "True")
        {
            isCorrect = 1;
        }
        else
        {
            isCorrect = 0;
        }
    }
}
