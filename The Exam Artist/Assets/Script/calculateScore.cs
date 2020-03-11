using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;




public class scoreObject {
    //  count of correct answered
    public int correct_ans;
    // count of unanswered questions
    public int unans_count;
    // count of total questions
    public int total_count;

}
public class calculateScore : MonoBehaviour
{
    public Text score; 
    private JObject ans_obj;    
    private JArray ans_arr;
    private scoreObject scoreObj = new scoreObject();


    private void Start()
    {
        scoreObj = getScore();
        score.text = scoreObj.correct_ans.ToString() + " / " + scoreObj.total_count.ToString();
    }


    public scoreObject getScore() {
        ans_arr = readAnswersFromJson();
        scoreObj.total_count = ans_arr.Count;
        scoreObj.correct_ans = getCorrectAnswersCount();
        scoreObj.unans_count = getUnansweredCount();
        Debug.Log(scoreObj);
        return scoreObj;    
    
    }

    public JArray readAnswersFromJson()
    {
        // read JSON directly from a file
        using (StreamReader file = File.OpenText(@Application.dataPath + "/GameData/answers.json"))
        using (JsonTextReader reader = new JsonTextReader(file))
        {
            ans_obj = (JObject)JToken.ReadFrom(reader);
        }
        // ans_obj has all the data inside it now
        ans_arr = (JArray)ans_obj["Answers"];
        return ans_arr;
    }


    public int getCorrectAnswersCount()
    {
        //to get data from json file
        int corr_count = 0;
        for (int i = 0; i < ans_arr.Count; i++)
        {
            // to make sure that the question was answered
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


    // to getcount of questios not anseed
    public int getUnansweredCount()
    {
        int count = 0;
        // 
        for (int i = 0; i < ans_arr.Count; i++)
        {
            // if the answer is equal to NA abbrevation for not answered
            if ((ans_arr[i]["YourAns"].ToString()) == "NA")
            {
                count = count + 1;

            }
        }
        return count;
    }      

}
