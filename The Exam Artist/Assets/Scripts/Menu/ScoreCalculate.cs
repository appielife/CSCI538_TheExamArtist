using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;

public class scoreObject
{
    public int correct_ans; // Count of correct answered
    public int unans_count; // Count of unanswered questions
    public int total_count; // Count of total questions
}

public class ScoreCalculate : MonoBehaviour
{
    public Text score, report;
    public GameObject fail, yourscore;
    private JObject ans_obj;
    private JArray ans_arr;
    private scoreObject scoreObj = new scoreObject();
    private List<string> reporttext = new List<string>();
    private int index = 0;
    private bool failed = false;

    private void Start()
    {
        scoreObj = getScore();
        score.text = scoreObj.correct_ans.ToString() + " / " + scoreObj.total_count.ToString();
        getReport();
        report.text = reporttext[index];
        GameObject scorereport = GameObject.Find("BlackBoard").transform.Find("ScoreReport").gameObject;

        // Set arrow charactor
        scorereport.transform.Find("PrevReport").GetComponentInChildren<Text>().text = '\u25B2'.ToString();
        scorereport.transform.Find("NextReport").GetComponentInChildren<Text>().text = '\u25BC'.ToString();
        if (GameObject.Find("Settings"))
        {
            // Find out if failed.
            failed = GameObject.Find("Settings").GetComponent<Settings>().getFailed();
        }
        if (failed)
        {
            yourscore.SetActive(false);
            fail.SetActive(true);
        }
    }

    // Function to get score
    public scoreObject getScore()
    {
        ans_arr = readAnswersFromJson();
        scoreObj.total_count = ans_arr.Count;
        scoreObj.correct_ans = getCorrectAnswersCount();
        scoreObj.unans_count = getUnansweredCount();
        return scoreObj;
    }

    // Function to get report 
    public void getReport()
    {
        for (int i = 0; i < ans_arr.Count; i++)
        {
            reporttext.Add("Question " + (i + 1).ToString() + " : " + ans_arr[i]["question_txt"].ToString() + "\n Correct Answer: " + ans_arr[i]["MyAns"].ToString() + "\n Your Answer: " + ans_arr[i]["YourAns"].ToString()); 
        }
    }
    
    // Function to show next report 
    public void nextReport()
    {
        if (index + 1 < reporttext.Count)
        {
            index++;
        }
        else
        {
            index = 0;
        }
        report.text = reporttext[index];
    }

    // Function to show previous report 
    public void prevReport()
    {
        if (index - 1 > 0) {
            index--;
        }
        else {
            index = reporttext.Count - 1;
        }
        report.text = reporttext[index];
    }

    // Function to read from answers.json
    public JArray readAnswersFromJson()
    {
        // Read JSON directly from a file
        // NOTE: Check out @Application.dataPath in Unity Documents.
        using (StreamReader file = File.OpenText(@Application.dataPath + "/GameData/answers.json"))
        using (JsonTextReader reader = new JsonTextReader(file))
        {
            ans_obj = (JObject)JToken.ReadFrom(reader);
        }

        ans_arr = (JArray)ans_obj["Answers"];
        return ans_arr;
    }

    // Function to get number of correct answers
    public int getCorrectAnswersCount()
    {
        // Get data from json file
        int corr_count = 0;
        for (int i = 0; i < ans_arr.Count; i++)
        {
            // Make sure the question was answered
            if ((ans_arr[i]["YourAns"].ToString()) != "NA")
            {
                if (ans_arr[i]["YourAns"].ToString() == ans_arr[i]["MyAns"].ToString())
                {
                    ++corr_count;
                }
            }
        }
        return corr_count;
    }

    // Function to get number of unanswered questions
    public int getUnansweredCount()
    {
        int count = 0;
        for (int i = 0; i < ans_arr.Count; i++)
        {
            if ((ans_arr[i]["YourAns"].ToString()) == "NA")
            {
                count = count + 1;
            }
        }
        return count;
    }

}
