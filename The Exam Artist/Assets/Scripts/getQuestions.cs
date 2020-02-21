using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



public class getQuestions : MonoBehaviour
{
    private string jsonString;
    private JObject ques_obj;
    static System.Random _random = new System.Random();


    // Start is called before the first frame update
    void Start()
    {
       // read JSON directly from a file
        using (StreamReader file = File.OpenText(@Application.dataPath + "/GameData/questions.json"))
        using (JsonTextReader reader = new JsonTextReader(file))
        {    ques_obj = (JObject)JToken.ReadFrom(reader);            
        }
        // Now all our data is on ques_obj
        // IMPORTANT : THIS INE WILL PRINT YOUR RESULT
        Debug.Log(getQuestionsArray());
    }

    //Call this function to get your questions array in JArray format
    public JArray getQuestionsArray()
    {
        JArray items = (JArray)ques_obj["questions"];
        Shuffle(items);        
        return items;
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



