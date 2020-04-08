using System.Collections;
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
    private GameObject testPage, submitPage, initialPage;
    public GameObject questionTextObj, choiceA, choiceB, choiceC, choiceD;
    public GetQuestion question = new GetQuestion();
    public string JSON_file;
    private ScoreCalculate calculateScore;
    private int tempQuestion = -1;
    private MultipleChoiceBehavior[] quesTrack;
    private int[] scoreTrack;
    public int total_score = 0;
    private float offset;
    private bool start = false;

    void Start()
    {
        GameObject testPaper = GameObject.FindGameObjectWithTag("MainTestPaper");
        testPage = testPaper.transform.Find("TestPage").gameObject;
        submitPage = testPaper.transform.Find("SubmitPage").gameObject;
        initialPage = testPaper.transform.Find("InitialPage").gameObject;
        initialPage.SetActive(true);
        testPage.SetActive(false);
        submitPage.SetActive(false);

        string[] files = { "-World", "-Foreign", "-Chemistry", "-Math" };
        int index = Random.Range(0, 3);
        JSON_file = "questions" + files[index] + ".json";

        LevelSetting setting = GameObject.Find("LevelSetting").GetComponent<LevelSetting>();
        offset = setting.offset;
        if (setting.washroomed)
        {
            question = setting.question;
            quesTrack = setting.quesTrack;
            scoreTrack = setting.scoreTrack;
            tempQuestion = question.current - 1;
        }
        else
        {
            question.readQuestionFromJson(JSON_file);
            //Debug.Log(question.ques);
            quesTrack = new MultipleChoiceBehavior[question.getQuesCount()];
            scoreTrack = new int[question.getQuesCount()];
            for (int i = 0; i < question.getQuesCount(); i++)
            {
                scoreTrack[i] = 0;
                next();
            }
        }
    }

    void Update()
    {
        if (offset > 0)
        {
            offset -= Time.deltaTime;
        }
        else
        {
            if (!start)
            {
                initialPage.SetActive(false);
                testPage.SetActive(true);
                next();
                start = true;
            }
        }
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

    public GetQuestion setQuestion()
    {
        return question;
    }
    public MultipleChoiceBehavior[] setQuesTrack()
    {
        return quesTrack;
    }
    public int[] setScoreTrack()
    {
        return scoreTrack;
    }

    public string[] getAllAnswer()
    {
        int size = question.getQuesCount();
        string[] answer = new string[size];
        char[] abcd = { 'A', 'B', 'C', 'D' };
        for (int i = 0; i < size; i++)
        {
            char ans = abcd[question.getQuestionCorrectAns(i)];
            answer[i] = (i + 1).ToString() + ": " + ans;
        }
        return answer;
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
                writer.WriteValue(abcd[question.getQuestionCorrectAns(i)]);
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
