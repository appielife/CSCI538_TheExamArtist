using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


[System.Serializable]
public class getQuestions
{
    private string jsonString;
    private JObject ques_obj;
    private JArray ques;
    static System.Random _random = new System.Random();
    public int current = -1;


    // Start is called before the first frame update
    public void readQuestionFromJson()
    {
        //Debug.Log(Application.dataPath);
        
        // read JSON directly from a file
        using (StreamReader file = File.OpenText(@Application.dataPath + "/GameData/questions.json"))
        using (JsonTextReader reader = new JsonTextReader(file))
        {
            ques_obj = (JObject)JToken.ReadFrom(reader);
        }
        // Now all our data is on ques_obj
        // IMPORTANT : THIS INE WILL PRINT YOUR RESULT
        getQuestionsArray();
        //Debug.Log(getNextQuestion());
        //Debug.Log(getNextQuestion());
    }

    public JObject getNextQuestion()
    {
        if (current < ques.Count-1)
        {
            current += 1;
        }
        else
        {
            current = 0;
        }
        return (JObject)ques[current];
    }

    public JObject getPrevQuestion()
    {
        if (current > 0)
        {
            current -= 1;
        }
        else
        {
            current = ques.Count - 1;
        }
        return (JObject)ques[current];
    }

    public string getQuestionId(int idx)
    {
        return (string)ques[idx]["id"];
    }

    public string getQuestionCorrectAns(int idx)
    {
        return (string)ques[idx]["correct_ans"];
    }

    //Call this function to get your questions array in JArray format
    public void getQuestionsArray()
    {
        ques = (JArray)ques_obj["questions"];
        JArray temp = new JArray();
        for (int i = 0; i < 5; i++) {
            temp.Add(ques[i]);
        }
        ques = temp;
        //Shuffle(ques);
    }

    public int getQuesCount()
    {
        return ques.Count;
    }

    // This function shuffes the question order everytime
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


}
