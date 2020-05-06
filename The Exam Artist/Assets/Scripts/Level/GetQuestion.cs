using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/***************************************
Script to get questions from JSON file
***************************************/

[System.Serializable]
public class GetQuestion
{
    public JArray ques;
    public int current = -1;

    private string jsonString;
    private JObject ques_obj;
    private int numQuestion = 5, numFileQuestion;

    static System.Random _random = new System.Random();

    // Function to make a copy
    public GetQuestion copy()
    {
        GetQuestion res = new GetQuestion();
        res.jsonString = jsonString;
        res.ques_obj = ques_obj;
        res.ques = ques;
        return res;
    }

    // Function to read question from JSON file
    public void readQuestionFromJson(string data)
    {
        // Read JSON directly from a file
        // NOTE: Check out @Application.dataPath in Unity Documents.
        using (StreamReader file = File.OpenText(@Application.dataPath + "/GameData/" + data))
        using (JsonTextReader reader = new JsonTextReader(file))
        {
            ques_obj = (JObject)JToken.ReadFrom(reader);
        }

        getQuestionsArray();
    }

    // Function to get next question
    public JObject getNextQuestion()
    {
        current = (current < ques.Count - 1) ? (++current) : 0;
        return (JObject)ques[current];
    }

    // Function to get previous question
    public JObject getPrevQuestion()
    {
        current = (current > 0) ? (--current) : ques.Count - 1;
        return (JObject)ques[current];
    }

    // Function to update question number
    public void updateQuesNum(int n)
    {
        current = n;
    }

    // Function to get question ID
    public string getQuestionId(int idx)
    {
        return (string)ques[idx]["id"];
    }

    // Function to get question text using the index
    public string getQuestionTxt(int idx)
    {
        return (string)ques[idx]["question_txt"];
    }

    // Function to get correct answer
    public int getQuestionCorrectAns(int idx)
    {
        JArray options = (JArray)ques[idx]["options"];
        for (int i = 0; i < options.Count; i++)
        {
            if ((string)((JObject)options[i])["isCorrect"] == "True")
            {
                return i;
            }
        }
        return -1;
    }

    // Function to get correct answer's context
    public string getQuestionCorrectAnsContext(int idx)
    {
        JArray options = (JArray)ques[idx]["options"];
        for (int i = 0; i < options.Count; i++)
        {
            if ((string)((JObject)options[i])["isCorrect"] == "True")
            {
                return (string)((JObject)options[i])["option_txt"];
            }
        }
        return "";
    }

    // Function to get your questions array in JArray format
    public void getQuestionsArray()
    {
        ques = (JArray)ques_obj["questions"];
        numFileQuestion = ques.Count;
        Shuffle(ques);
        for (int i = 0; i < ques.Count; i++)
        {
            JArray t = (JArray)ques[i]["options"];
            Shuffle(t);
            ques[i]["options"] = t;
        }
        JArray temp = new JArray();
        if(numQuestion > ques.Count)
        {
            numQuestion = ques.Count;
        }
        else if(numQuestion < 0)
        {
            numQuestion = 1;
        }

        for (int i = 0; i < numQuestion; i++)
        {
            temp.Add(ques[i]);
        }
        ques = temp;
    }
    
    // Function to get number of questions
    public int getQuesCount()
    {
        return ques.Count;
    }

    // Function to shuffle the question order everytime
    static void Shuffle(JArray array)
    {
        int n = array.Count;
        for (int i = 0; i < (n - 1); i++)
        {
            // Use Next on random instance with an argument.
            // ... The argument is an exclusive bound.
            //     So we will not go past the end of the array.
            int r = i + _random.Next(n - i);
            JToken t = array[r];
            array[r] = array[i];
            array[i] = t;
        }
    }

    // Function to set number of questions to get
    public void setNumQuestion(int numQuestion)
    {
        this.numQuestion = numQuestion;
    }

    // Function to get number of questions in file
    public int getNumFileQuestion()
    {
        return numFileQuestion;
    }
}
