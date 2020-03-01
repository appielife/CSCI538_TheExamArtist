using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class TestPaperBehavior : MonoBehaviour
{
    public GameObject scoreObj;
    public GameObject questionTextObj;
    public GameObject choiceA;
    public GameObject choiceB;
    public GameObject choiceC;
    public GameObject choiceD;
    private getQuestions question = new getQuestions();
    private int tempQuestion = -1;
    private MultipleChoiceBehavior[] quesTrack;
    private int[] scoreTrack;
    public int total_score = 0;
    // Start is called before the first frame update
    void Start()
    {
        question.readQuestionFromJson();
        quesTrack = new MultipleChoiceBehavior[question.getQuesCount()];
        scoreTrack = new int[question.getQuesCount()];
        for (int i = 0; i< question.getQuesCount(); i++)
        {
            scoreTrack[i] = 0;
        }
        next();
    }

    public void next()
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

        TextMesh scoreText = scoreObj.GetComponent<TextMesh>();
        scoreText.text = total_score.ToString();
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
        quesTrack[tempQuestion].showQuestion(questionTextObj, choices);
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

        TextMesh scoreText = scoreObj.GetComponent<TextMesh>();
        scoreText.text = total_score.ToString();
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
            JObject Q = question.getNextQuestion();
            quesTrack[tempQuestion].pushQuestion(Q);
        }
        GameObject[] choices = { choiceA, choiceB, choiceC, choiceD };
        quesTrack[tempQuestion].showQuestion(questionTextObj, choices);
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
